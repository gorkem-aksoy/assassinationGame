using System.Collections;
using System.Collections.Generic;
using UnityVector3 = UnityEngine.Vector3;
using UnityEngine;
using kutuphanem; // oluþturulan namespace

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

    // kütüphanem de ki animasyon classým örneklendi. kullanýma hazýr
    animasyon animasyon = new animasyon();

    // sol yön geçiþ parametrelerini tutucak. Geçiþ parametrelerini burdan kütüphaneye göndericez. 
    // kütüphane dinamikleþicek.
    // sol geçiþ parametreleri. Bu veriler ilgili yönden kütüphanedeki ilgili fonksiyona gönderilecek.
    float[] solYonGecisParametreleri = { .15f, .35f, .5f, 1f };
    float[] sagYonGecisParametreleri = { .15f, .35f, .5f, 1f };
    float[] egilmeYonGecisParametreleri = { .15f, .32f, .49f, .66f, .83f };

    void Start()
    {
        anim = GetComponent<Animator>();
        mainCam = Camera.main;
    }

    // karakter yönetimleri genellikle late update fonksiyonunda yapýlýr. daha yumuþak bir hareket saðlanýr.
    void LateUpdate()
    {
        // örnek fonksiyonu çaðýrmak.
        //animasyon.deneme();

        // -------------------- ileri hareket --------------------------
        // koþma hýzýný ve yürüme hýzýný parametre olarak göndererek animasyon kütüphanemizin dinamik olmasýný saðlýyoruz.
        // güncelleme sonrasý kütüphanemize animatorü, animasyonda kullanýlan parametre deðerini,
        // uzunluðu ve geçiþ deðerlerini gönderdik. böylece dinamik bir yapý elde ettik.
        animasyon.karakterHareket(anim,"speed", maksimumUzunluk, 1f, 0.2f);
        // -------------------- ileri hareket --------------------------

        // inputRotation Güncelleme

        animasyon.karakterDonus(mainCam, donusHizi, karakter);

        // ----------------------------- geri hareket -------------------------------
        // animatorde iþlenen parametreyi gönderdik. kütüphane dinamikleþti.
        animasyon.geriHareket(anim, "geriYuru");
        // ----------------------------- geri hareket -------------------------------

        // ------------------------ sol hareket ------------------------------
        // animatorde iþlenen parametreleri gönderdik. kütüphane dinamikleþti.
        animasyon.solHareket(anim, "solHareket", "solAktifmi", animasyon.gecisDegerleri(solYonGecisParametreleri));
        // ------------------------ sol hareket ------------------------------

        // -------------------------- sað hareket ----------------------------------
        // animatorde iþlenen parametreleri gönderdik. kütüphane dinamikleþti.
        animasyon.sagHareket(anim, "sagHareket", "sagAktifmi", animasyon.gecisDegerleri(sagYonGecisParametreleri));
        // -------------------------- sað hareket ----------------------------------

        // -------------------------- eðilme hareket ----------------------------------
        // animatorde iþlenen parametreleri gönderdik. kütüphane dinamikleþti.
        animasyon.egilmeHareket(anim, "egilmeHareket", "egilmeAktifmi", animasyon.gecisDegerleri(egilmeYonGecisParametreleri));
        // -------------------------- eðilme hareket ----------------------------------

        

        
    }

  
}
