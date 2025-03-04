using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class dusman : MonoBehaviour
{
    // public GameObject dusmanKafaObjesi; raycast ýþýn denemesi için kullanýldý

    [Header("Genel Ayarlar")]
    NavMeshAgent agent;
    Animator dusmanAnimator;
    GameObject hedef;
    public GameObject hedefUzaktanTemas;
    // düþmanýn etrafýnda ki collider çapý
    public float suphelenmeMenzil = 10;
    public float atesEtmeMenzil = 7;
    Vector3 baslangicNoktasi;
    bool suphelenmeVarmi = false;
    public float darbeGucu = 10;
    public float saglik = 100;

    [Header("Devriye Ayarlarý")]

    public GameObject[] devriyeNoktalari_1;
    public GameObject[] devriyeNoktalari_2;
    public GameObject[] devriyeNoktalari_3;
    //public GameObject[] denemeDevriye;
    bool devriyeVarmi = false;
    Coroutine devriyeAt;
    Coroutine devriyeZaman;
    // devriye fonksiyonu update fonksiyonunda çalýþtýrýlacaðý için kilit konulmalý
    // yoksa sürekli fonksiyonu çalýþtýrmaya çalýþacak.
    // baþlangýçta true, fonksiyona girdiðinde ise false olucak.
    bool devriyeKilit;

    // devriye atmamasý gereken düþmanlar için kullanýlacak
    public bool devriyeAtabilirmi;

    GameObject[] aktifOlanGuzergah;

    [Header("Silah Ayarlarý")]
    float atesEtmeSikliði_1; // içeride dönecek
    public float atesEtmeSikliði_2; // dýþarýdan girilecek
    public float menzil;
    public AudioSource[] sesler;
    public ParticleSystem[] efektler;
    bool atesEdiliyormu = false;
    public Transform mermiCikisNoktasi;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        dusmanAnimator = GetComponent<Animator>();

        // düþmanýn ilk bulunduðu pozisyon. Oyuncu alandan çýkarsa geri dönebilmesi için.
        baslangicNoktasi = transform.position;

        agent.isStopped = false;
        agent.updateRotation = true;
        agent.updatePosition = true;

        if (devriyeAtabilirmi)
            devriyeZaman = StartCoroutine(devriyeZamanlayýcý());

    }


    void Update()
    {

    }

    private void LateUpdate()
    {
        suphelenmeBölgesi();
        atesEtmeBölgesi();


        // izin verildiyse, güzargah seç ve devriye baþlasýn.
        if (devriyeKilit)
        {
            guzergahSeç();

        }

    }

    // devriye yi belirli bir saniye de bir baþlatýyoruz.
    //start methodunda ve devriTeknik methodunun bitiminde baþlýyor.
    IEnumerator devriyeZamanlayýcý()
    {
        while (!devriyeVarmi && devriyeAtabilirmi)
        {
            int zaman = Random.Range(5, 10);
            yield return new WaitForSeconds(5f);

            // devriye ye çýkabilir. izin verildi
            devriyeKilit = true;

            // düþman hareket edebilir. devriye baþladý.
            agent.isStopped = false;
        }
    }

    // düþman karakterlere random olarak güzergah atanacak.
    // lateUpdate methodunda baþlatýlýyor.
    void guzergahSeç()
    {
        int deger = Random.Range(1, 3);

        switch (deger)
        {
            case 1:
                aktifOlanGuzergah = devriyeNoktalari_1;
                break;
            case 2:
                aktifOlanGuzergah = devriyeNoktalari_2;
                break;
            case 3:
                aktifOlanGuzergah = devriyeNoktalari_3;
                break;

        }

        // devriye baþlasýn
        devriyeAt = StartCoroutine(devriyeTeknikIslem());

    }

    // devriye hedef iþlemleri ve dahasý
    //güzergah seç methodunda baþlatýlýyor.
    IEnumerator devriyeTeknikIslem()
    {
        // devriye baþladý. kilit kapandý
        devriyeKilit = false;
        // þuan devriye var
        devriyeVarmi = true;
        dusmanAnimator.SetBool("yurume", true);
        int toplamNokta = aktifOlanGuzergah.Length - 1;
        int hedefNokta = 0;

        while (true && devriyeAtabilirmi)
        {


            if (Vector3.Distance(transform.position, aktifOlanGuzergah[hedefNokta].transform.position) <= 1)
            {
                hedefNokta++;
            }

            if (toplamNokta >= hedefNokta)
            {
                agent.SetDestination(aktifOlanGuzergah[hedefNokta].transform.position);
            }
            else
            {

                // durma mesafesi belirledik
                agent.stoppingDistance = 0.5f;
                agent.SetDestination(baslangicNoktasi);

                while (agent.remainingDistance > 0.5f)
                {
                    yield return null; // Her frame kontrol et
                }

                dusmanAnimator.SetBool("yurume", false);
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);

                // karakter kaymasýn diye durdurduk.
                agent.isStopped = true;

                // devriye bitti, tekrardan baþlayabilir.
                devriyeVarmi = false;

                // zamanlayýcý baþlasýn, devriyede düþman yok
                devriyeZaman = StartCoroutine(devriyeZamanlayýcý());

                // devriye tamamlandý. devriye fonksiyonunu durdurduk.
                StopCoroutine(devriyeAt);


                break;
            }
            // her frame de kontrol et
            yield return null;
        }
    }

    // player taglý obje yakalanýrsa çalýþacak
    private void suphelenmeBölgesi()
    {
        Collider[] supheHitColliders = Physics.OverlapSphere(transform.position, suphelenmeMenzil);

        /*
          * ihtiyaç olursa bakýlacak
         float enYakinMesafe = Mathf.Infinity;
         GameObject enYakinHedef = null;
         */

        foreach (var objeler in supheHitColliders)
        {
           

            if (objeler.gameObject.CompareTag("Player"))
            {
                // saglik durumunda true yapýlan animasyonu false yapýyoruz.
                if (dusmanAnimator.GetBool("kosma"))
                {
                    dusmanAnimator.SetBool("kosma", false);
                }

                suphelenmeVarmi = true;
                hedef = objeler.gameObject;
                dusmanAnimator.SetBool("yurume", true);
                agent.isStopped = false;
                agent.SetDestination(hedef.transform.position);
                return;
            }
           
        }

        // devriye atmayan sabit olarak bir konumda bekleyen düþman objeleri için
        if (suphelenmeVarmi && !devriyeAtabilirmi)
        {
            hedef = null;
            agent.stoppingDistance = 0.5f;
            agent.SetDestination(baslangicNoktasi);

            if(agent.remainingDistance <= 0.5f)
            {
                dusmanAnimator.SetBool("yurume", false);
                agent.isStopped = true;

            }

        }
        // saglik durumu kosma animasyonu için gerekli.
        suphelenmeVarmi = false;
    }

    // player taglý obje yakalanýrsa atesEt fonksiyonu çalýþacak
    private void atesEtmeBölgesi()
    {
        Collider[] atesHitColliders = Physics.OverlapSphere(transform.position, atesEtmeMenzil);

        /*
         * ihtiyaç olursa bakýlacak
        float enYakinMesafe = Mathf.Infinity;
        GameObject enYakinHedef = null;
        */

        foreach (var objeler in atesHitColliders)
        {
            if (objeler.gameObject.CompareTag("Player"))
            {
                
                atesEt(objeler.gameObject);
                return;
            }
            else
            {
                if (atesEdiliyormu)
                {
                    dusmanAnimator.SetBool("atesEt", false);
                    agent.isStopped = false;
                    atesEdiliyormu = false;
                    dusmanAnimator.SetBool("yurume", true);
                }
            }
        }
    }

    // ateþ etme teknik iþlemler
    void atesEt(GameObject hedefim)
    {
        atesEdiliyormu = true;
        // düþman karakterin kaymasýný önlemek için
        agent.isStopped = true;
        transform.LookAt(hedefim.transform);

        dusmanAnimator.SetBool("yurume", false);

        dusmanAnimator.SetBool("atesEt", true);


        // raycast ýþýný karakter dýþýnda bir yere çarptýðýnda çarptýðý objede characterControl scriptini bulmaya çalýþýyor.
        // bu nedenle hata veriyor ancak önemli deðil...
        RaycastHit hit;
        if (Physics.Raycast(mermiCikisNoktasi.position, mermiCikisNoktasi.forward, out hit, menzil))
        {

            if (Time.time > atesEtmeSikliði_1)
            {

                if (hit.transform.gameObject.CompareTag("Player"))
                {
                    hit.transform.gameObject.GetComponent<characterControl>().saglikDurumu(darbeGucu);
                    Instantiate(efektler[1], hit.point, Quaternion.LookRotation(hit.normal));
                }
                else
                {
                    Instantiate(efektler[1], hit.point, Quaternion.LookRotation(hit.normal));
                }
                


                if (!sesler[0].isPlaying)
                {
                    sesler[0].Play();
                    efektler[0].Play();
                }

                atesEtmeSikliði_1 = Time.time + atesEtmeSikliði_2;
            } 
            
        }

        Debug.DrawRay(mermiCikisNoktasi.position, mermiCikisNoktasi.forward * 10f, Color.blue);
    }
     
    // submachine scriptinden oyuncuya tanýmlý darbeGucu parametresi gelicek.
    public void saglikDurumu(float darbeGucu)
    {
        saglik -= darbeGucu;

        // menzillere girmeden uzaktan ateþ edilirse
        if (!suphelenmeVarmi)
        {
            dusmanAnimator.SetBool("kosma", true);
            dusmanAnimator.SetBool("yurume", false);
            agent.SetDestination(hedefUzaktanTemas.transform.position);
        }
        
        if (saglik <= 0)
        {
            dusmanAnimator.Play("olme");
            /*agent.enabled = false;
            agent.isStopped = true;
            StopCoroutine(devriyeAt);*/
            Destroy(gameObject,5f);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, atesEtmeMenzil);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, suphelenmeMenzil);
    }
}


    /*
    // Örnek Physics.Raycast kullanýmý, Physics.OverlapSphere ile birlikte de kullanýlabilir. ayrý olarak ta
    // yapý düþmanýn kafa objesinden bir ýþýn çýkarýyor. düþman sahip olduðu idle animasyonu gereði
    // kafasý ile saða sola bakýyor. bu sayede ileri doðru gönderilen ýþýn kafa nereye dönerse oraya doðru dönüyor.
    // ýþýn için belirlene mesafede Player tagýna sahip bir obje var ise if bloðu çalýþýyor.
    RaycastHit hit;
    if (Physics.Raycast(dusmanKafaObjesi.transform.position, dusmanKafaObjesi.transform.forward, out hit, 10f))
    {
        if (hit.transform.gameObject.CompareTag("Player"))
        {
            Debug.Log("çarptý");
        }
    }

    Debug.DrawRay(dusmanKafaObjesi.transform.position, dusmanKafaObjesi.transform.forward * 10f, Color.blue);
    */
