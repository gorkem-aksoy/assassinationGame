using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class subMachine : MonoBehaviour
{
    [Header("Silah Ayarları")]
    float atesEtmeSikliği_1; // içeride dönecek
    public float atesEtmeSikliği_2; // dışarıdan girilecek
    public float menzil;
    int kalanMermiSayisi;
    public int sarjorKapasitesi = 5;
    int toplamMermiSayisi = 150;
    public TextMeshProUGUI kalanMermiText;
    public TextMeshProUGUI toplamMermiText;
    float darbeGucu = 15;

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

        // animasyon bittiğinde animasyona eklenen olay yani reloadControl scriptinde parametre true olunca
        // teknik işlem fonksiyonu çalışacak
        // ve ses oynatılacak
        if (karakterAnimator.GetBool("reload"))
        {
            reloadTeknikIslemler();
            sesler[2].Play();
        }


        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (Time.time > atesEtmeSikliği_1 && kalanMermiSayisi != 0)
            {
                atesEt();
                atesEtmeSikliği_1 = Time.time + atesEtmeSikliği_2;
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
            if (hit.transform.gameObject.CompareTag("dusman"))
            {
                Instantiate(efektler[2], hit.point, Quaternion.LookRotation(hit.normal));
                hit.transform.gameObject.GetComponent<dusman>().saglikDurumu(darbeGucu);
            }
            else
            {
                Instantiate(efektler[1], hit.point, Quaternion.LookRotation(hit.normal));
            }

        }
    }

    // animasyon çalıştırma
    void reloadControl()
    {
        if (kalanMermiSayisi < sarjorKapasitesi && toplamMermiSayisi != 0)
        {
            karakterAnimator.Play("reload");
            
        }
    }
    // matematiksel işlemleri çalıştırma
    void reloadTeknikIslemler()
    {


        // harcanacak mermi yok, reload olmadan ateş edilemez
        if (kalanMermiSayisi == 0)
        {
            // kalan mermi ye sarjor kapasitesi kadar mermi eklenemez
            // son relaod işlemi
            // toplam mermi 0 olucak
            // kalan mermi 0, toplam mermi 3, sarjor kapasite 5
            if (toplamMermiSayisi <= sarjorKapasitesi)
            {
                kalanMermiSayisi = toplamMermiSayisi;
                toplamMermiSayisi = 0;
            }
            // toplam mermi sajor kapasitesinden fazla, birden fazla reload yapılabilir
            // kalan mermi 0, toplam mermi 10, sarjor kapasite 5
            else
            {
                toplamMermiSayisi -= sarjorKapasitesi;
                kalanMermiSayisi = sarjorKapasitesi;
            }


        }
        // harcanacak mermi daha var, reload olmadan ateş edilebilir.
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
                    // ne kadar mermi artığını hesaplıyoruz.
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
            // toplam mermi sajor kapasitesinden fazla, birden fazla reload yapılabilir
            // kalan mermi 4, toplam mermi 10, sarjor kapasite 5
            else
            {
                int harcananMermi = sarjorKapasitesi - kalanMermiSayisi;

                toplamMermiSayisi -= harcananMermi;

                kalanMermiSayisi = sarjorKapasitesi;
            }



        }

        // canvas elemanlarına yazdırma
        kalanMermiText.text = kalanMermiSayisi.ToString();
        toplamMermiText.text = toplamMermiSayisi.ToString();

        // değeri false yaparak, reload işlemi tekrar olduğunda matematiksel işlem tekrar çalışabilsin
        karakterAnimator.SetBool("reload", false);
    }

}
