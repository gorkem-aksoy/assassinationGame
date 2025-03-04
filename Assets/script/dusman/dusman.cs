using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class dusman : MonoBehaviour
{
    // public GameObject dusmanKafaObjesi; raycast ���n denemesi i�in kullan�ld�

    [Header("Genel Ayarlar")]
    NavMeshAgent agent;
    Animator dusmanAnimator;
    GameObject hedef;
    public GameObject hedefUzaktanTemas;
    // d��man�n etraf�nda ki collider �ap�
    public float suphelenmeMenzil = 10;
    public float atesEtmeMenzil = 7;
    Vector3 baslangicNoktasi;
    bool suphelenmeVarmi = false;
    public float darbeGucu = 10;
    public float saglik = 100;

    [Header("Devriye Ayarlar�")]

    public GameObject[] devriyeNoktalari_1;
    public GameObject[] devriyeNoktalari_2;
    public GameObject[] devriyeNoktalari_3;
    //public GameObject[] denemeDevriye;
    bool devriyeVarmi = false;
    Coroutine devriyeAt;
    Coroutine devriyeZaman;
    // devriye fonksiyonu update fonksiyonunda �al��t�r�laca�� i�in kilit konulmal�
    // yoksa s�rekli fonksiyonu �al��t�rmaya �al��acak.
    // ba�lang��ta true, fonksiyona girdi�inde ise false olucak.
    bool devriyeKilit;

    // devriye atmamas� gereken d��manlar i�in kullan�lacak
    public bool devriyeAtabilirmi;

    GameObject[] aktifOlanGuzergah;

    [Header("Silah Ayarlar�")]
    float atesEtmeSikli�i_1; // i�eride d�necek
    public float atesEtmeSikli�i_2; // d��ar�dan girilecek
    public float menzil;
    public AudioSource[] sesler;
    public ParticleSystem[] efektler;
    bool atesEdiliyormu = false;
    public Transform mermiCikisNoktasi;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        dusmanAnimator = GetComponent<Animator>();

        // d��man�n ilk bulundu�u pozisyon. Oyuncu alandan ��karsa geri d�nebilmesi i�in.
        baslangicNoktasi = transform.position;

        agent.isStopped = false;
        agent.updateRotation = true;
        agent.updatePosition = true;

        if (devriyeAtabilirmi)
            devriyeZaman = StartCoroutine(devriyeZamanlay�c�());

    }


    void Update()
    {

    }

    private void LateUpdate()
    {
        suphelenmeB�lgesi();
        atesEtmeB�lgesi();


        // izin verildiyse, g�zargah se� ve devriye ba�las�n.
        if (devriyeKilit)
        {
            guzergahSe�();

        }

    }

    // devriye yi belirli bir saniye de bir ba�lat�yoruz.
    //start methodunda ve devriTeknik methodunun bitiminde ba�l�yor.
    IEnumerator devriyeZamanlay�c�()
    {
        while (!devriyeVarmi && devriyeAtabilirmi)
        {
            int zaman = Random.Range(5, 10);
            yield return new WaitForSeconds(5f);

            // devriye ye ��kabilir. izin verildi
            devriyeKilit = true;

            // d��man hareket edebilir. devriye ba�lad�.
            agent.isStopped = false;
        }
    }

    // d��man karakterlere random olarak g�zergah atanacak.
    // lateUpdate methodunda ba�lat�l�yor.
    void guzergahSe�()
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

        // devriye ba�las�n
        devriyeAt = StartCoroutine(devriyeTeknikIslem());

    }

    // devriye hedef i�lemleri ve dahas�
    //g�zergah se� methodunda ba�lat�l�yor.
    IEnumerator devriyeTeknikIslem()
    {
        // devriye ba�lad�. kilit kapand�
        devriyeKilit = false;
        // �uan devriye var
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

                // karakter kaymas�n diye durdurduk.
                agent.isStopped = true;

                // devriye bitti, tekrardan ba�layabilir.
                devriyeVarmi = false;

                // zamanlay�c� ba�las�n, devriyede d��man yok
                devriyeZaman = StartCoroutine(devriyeZamanlay�c�());

                // devriye tamamland�. devriye fonksiyonunu durdurduk.
                StopCoroutine(devriyeAt);


                break;
            }
            // her frame de kontrol et
            yield return null;
        }
    }

    // player tagl� obje yakalan�rsa �al��acak
    private void suphelenmeB�lgesi()
    {
        Collider[] supheHitColliders = Physics.OverlapSphere(transform.position, suphelenmeMenzil);

        /*
          * ihtiya� olursa bak�lacak
         float enYakinMesafe = Mathf.Infinity;
         GameObject enYakinHedef = null;
         */

        foreach (var objeler in supheHitColliders)
        {
           

            if (objeler.gameObject.CompareTag("Player"))
            {
                // saglik durumunda true yap�lan animasyonu false yap�yoruz.
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

        // devriye atmayan sabit olarak bir konumda bekleyen d��man objeleri i�in
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
        // saglik durumu kosma animasyonu i�in gerekli.
        suphelenmeVarmi = false;
    }

    // player tagl� obje yakalan�rsa atesEt fonksiyonu �al��acak
    private void atesEtmeB�lgesi()
    {
        Collider[] atesHitColliders = Physics.OverlapSphere(transform.position, atesEtmeMenzil);

        /*
         * ihtiya� olursa bak�lacak
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

    // ate� etme teknik i�lemler
    void atesEt(GameObject hedefim)
    {
        atesEdiliyormu = true;
        // d��man karakterin kaymas�n� �nlemek i�in
        agent.isStopped = true;
        transform.LookAt(hedefim.transform);

        dusmanAnimator.SetBool("yurume", false);

        dusmanAnimator.SetBool("atesEt", true);


        // raycast ���n� karakter d���nda bir yere �arpt���nda �arpt��� objede characterControl scriptini bulmaya �al���yor.
        // bu nedenle hata veriyor ancak �nemli de�il...
        RaycastHit hit;
        if (Physics.Raycast(mermiCikisNoktasi.position, mermiCikisNoktasi.forward, out hit, menzil))
        {

            if (Time.time > atesEtmeSikli�i_1)
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

                atesEtmeSikli�i_1 = Time.time + atesEtmeSikli�i_2;
            } 
            
        }

        Debug.DrawRay(mermiCikisNoktasi.position, mermiCikisNoktasi.forward * 10f, Color.blue);
    }
     
    // submachine scriptinden oyuncuya tan�ml� darbeGucu parametresi gelicek.
    public void saglikDurumu(float darbeGucu)
    {
        saglik -= darbeGucu;

        // menzillere girmeden uzaktan ate� edilirse
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
    // �rnek Physics.Raycast kullan�m�, Physics.OverlapSphere ile birlikte de kullan�labilir. ayr� olarak ta
    // yap� d��man�n kafa objesinden bir ���n ��kar�yor. d��man sahip oldu�u idle animasyonu gere�i
    // kafas� ile sa�a sola bak�yor. bu sayede ileri do�ru g�nderilen ���n kafa nereye d�nerse oraya do�ru d�n�yor.
    // ���n i�in belirlene mesafede Player tag�na sahip bir obje var ise if blo�u �al���yor.
    RaycastHit hit;
    if (Physics.Raycast(dusmanKafaObjesi.transform.position, dusmanKafaObjesi.transform.forward, out hit, 10f))
    {
        if (hit.transform.gameObject.CompareTag("Player"))
        {
            Debug.Log("�arpt�");
        }
    }

    Debug.DrawRay(dusmanKafaObjesi.transform.position, dusmanKafaObjesi.transform.forward * 10f, Color.blue);
    */
