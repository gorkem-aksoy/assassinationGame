using System.Collections;
using System.Collections.Generic;
using UnityVector3 = UnityEngine.Vector3;
using UnityEngine;
using kutuphanem; // olu�turulan namespace

public class characterMove : MonoBehaviour
{
    float inputX;
    float inputZ;
    public Transform karakter;
    Animator anim;
    Vector3 mevcutYon;
    Camera mainCam;
    float maksimumUzunluk = 1;
    float donusHizi = 10;
    float maksimumHiz;

    // k�t�phanem de ki animasyon class�m �rneklendi. kullan�ma haz�r
    animasyon animasyon = new animasyon();

    // sol y�n ge�i� parametrelerini tutucak. Ge�i� parametrelerini burdan k�t�phaneye g�ndericez. 
    // k�t�phane dinamikle�icek.
    // sol ge�i� parametreleri. Bu veriler ilgili y�nden k�t�phanedeki ilgili fonksiyona g�nderilecek.
    float[] solYonGecisParametreleri = { .15f, .35f, .5f, 1f };
    float[] sagYonGecisParametreleri = { .15f, .35f, .5f, 1f };
    float[] egilmeYonGecisParametreleri = { .15f, .32f, .49f, .66f, .83f };

    void Start()
    {
        anim = GetComponent<Animator>();
        mainCam = Camera.main;
    }

    // karakter y�netimleri genellikle late update fonksiyonunda yap�l�r. daha yumu�ak bir hareket sa�lan�r.
    void LateUpdate()
    {
        // �rnek fonksiyonu �a��rmak.
        //animasyon.deneme();

        // -------------------- ileri hareket --------------------------
        // ko�ma h�z�n� ve y�r�me h�z�n� parametre olarak g�ndererek animasyon k�t�phanemizin dinamik olmas�n� sa�l�yoruz.
        // g�ncelleme sonras� k�t�phanemize animator�, animasyonda kullan�lan parametre de�erini,
        // uzunlu�u ve ge�i� de�erlerini g�nderdik. b�ylece dinamik bir yap� elde ettik.
        animasyon.karakterHareket(anim,"speed", maksimumUzunluk, 1f, 0.2f);
        // -------------------- ileri hareket --------------------------

        // inputRotation G�ncelleme

        animasyon.karakterDonus(mainCam, donusHizi, karakter);

        // ----------------------------- geri hareket -------------------------------
        // animatorde i�lenen parametreyi g�nderdik. k�t�phane dinamikle�ti.
        animasyon.geriHareket(anim, "geriYuru");
        // ----------------------------- geri hareket -------------------------------

        // ------------------------ sol hareket ------------------------------
        // animatorde i�lenen parametreleri g�nderdik. k�t�phane dinamikle�ti.
        animasyon.solHareket(anim, "solHareket", "solAktifmi", animasyon.gecisDegerleri(solYonGecisParametreleri));
        // ------------------------ sol hareket ------------------------------

        // -------------------------- sa� hareket ----------------------------------
        // animatorde i�lenen parametreleri g�nderdik. k�t�phane dinamikle�ti.
        animasyon.sagHareket(anim, "sagHareket", "sagAktifmi", animasyon.gecisDegerleri(sagYonGecisParametreleri));
        // -------------------------- sa� hareket ----------------------------------

        // -------------------------- e�ilme hareket ----------------------------------
        // animatorde i�lenen parametreleri g�nderdik. k�t�phane dinamikle�ti.
        animasyon.egilmeHareket(anim, "egilmeHareket", "egilmeAktifmi", animasyon.gecisDegerleri(egilmeYonGecisParametreleri));
        // -------------------------- e�ilme hareket ----------------------------------

        

        
    }

  
}
