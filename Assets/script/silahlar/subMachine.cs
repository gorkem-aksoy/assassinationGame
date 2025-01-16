using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class subMachine : MonoBehaviour
{
    [Header("Silah Ayarlarý")]
    float atesEtmeSikliði_1; // içeride dönecek
    public float atesEtmeSikliði_2; // dýþarýdan girilecek
    public float menzil;
    int kalanMermiSayisi;
    public int sarjorKapasitesi = 5;
    int toplamMermiSayisi = 10;
    public TextMeshProUGUI kalanMermiText;
    public TextMeshProUGUI toplamMermiText;

    [Header("Sesler")]
    public AudioSource[] sesler;

    [Header("Efektler")]
    public ParticleSystem[] efektler;

    [Header("Genel Ayarlar")]
    public Camera benimCam;
    public Animator karakterAnimator;

    void Start()
    {
        kalanMermiSayisi = sarjorKapasitesi;
        kalanMermiText.text = kalanMermiSayisi.ToString();
        toplamMermiText.text = toplamMermiSayisi.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            
            reloadControl();

        }

        // animasyon bittiðinde animasyona eklenen olay yani reloadControl scriptinde parametre true olunca
        // teknik iþlem fonksiyonu çalýþacak
        // ve ses oynatýlacak
        if (karakterAnimator.GetBool("reload"))
        {
            reloadTeknikIslemler();
            sesler[2].Play();
        }


        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (Time.time > atesEtmeSikliði_1 && kalanMermiSayisi != 0)
            {
                atesEt();
                atesEtmeSikliði_1 = Time.time + atesEtmeSikliði_2;
            }
            else if (kalanMermiSayisi == 0)
            {
                sesler[1].Play();
            }

        }
    }

    void atesEt()
    {
        kalanMermiSayisi--;
        kalanMermiText.text = kalanMermiSayisi.ToString();

        efektler[0].Play();
        sesler[0].Play();

        RaycastHit hit;
        if (Physics.Raycast(benimCam.transform.position, benimCam.transform.forward, out hit, menzil))
        {

            Instantiate(efektler[1], hit.point, Quaternion.LookRotation(hit.normal));
            Debug.Log("ates edildi");
            Debug.Log(hit.transform.gameObject.name);
        }
    }

    // animasyon çalýþtýrma
    void reloadControl()
    {
        if (kalanMermiSayisi < sarjorKapasitesi && toplamMermiSayisi != 0)
        {
            karakterAnimator.Play("reload");
            
        }
    }
    // matematiksel iþlemleri çalýþtýrma
    void reloadTeknikIslemler()
    {


        // harcanacak mermi yok, reload olmadan ateþ edilemez
        if (kalanMermiSayisi == 0)
        {
            // kalan mermi ye sarjor kapasitesi kadar mermi eklenemez
            // son relaod iþlemi
            // toplam mermi 0 olucak
            // kalan mermi 0, toplam mermi 3, sarjor kapasite 5
            if (toplamMermiSayisi <= sarjorKapasitesi)
            {
                kalanMermiSayisi = toplamMermiSayisi;
                toplamMermiSayisi = 0;
            }
            // toplam mermi sajor kapasitesinden fazla, birden fazla reload yapýlabilir
            // kalan mermi 0, toplam mermi 10, sarjor kapasite 5
            else
            {
                toplamMermiSayisi -= sarjorKapasitesi;
                kalanMermiSayisi = sarjorKapasitesi;
            }


        }
        // harcanacak mermi daha var, reload olmadan ateþ edilebilir.
        else
        {
            // son reload
            // kalan mermi 4, toplam mermi 3, sarjor kapasitesi 5
            if (toplamMermiSayisi <= sarjorKapasitesi)
            {
                int olusanToplamMermi = kalanMermiSayisi + toplamMermiSayisi;

                // olusan toplam mermi 7, sarjor kapasitesi 5
                if (olusanToplamMermi > sarjorKapasitesi)
                {
                    kalanMermiSayisi = sarjorKapasitesi;
                    // ne kadar mermi artýðýný hesaplýyoruz.
                    toplamMermiSayisi = olusanToplamMermi - sarjorKapasitesi;
                }
                // olusan toplam mermi 3, sarjor kapasitesi 5
                // kalan mermi 3, toplam mermi 1 , sarjor kapasitesi 5
                else
                {
                    kalanMermiSayisi += toplamMermiSayisi;
                    toplamMermiSayisi = 0;
                }
            }
            // toplam mermi sajor kapasitesinden fazla, birden fazla reload yapýlabilir
            // kalan mermi 4, toplam mermi 10, sarjor kapasite 5
            else
            {
                int harcananMermi = sarjorKapasitesi - kalanMermiSayisi;

                toplamMermiSayisi -= harcananMermi;

                kalanMermiSayisi = sarjorKapasitesi;
            }



        }

        // canvas elemanlarýna yazdýrma
        kalanMermiText.text = kalanMermiSayisi.ToString();
        toplamMermiText.text = toplamMermiSayisi.ToString();

        // deðeri false yaparak, reload iþlemi tekrar olduðunda matematiksel iþlem tekrar çalýþabilsin
        karakterAnimator.SetBool("reload", false);
    }

}
