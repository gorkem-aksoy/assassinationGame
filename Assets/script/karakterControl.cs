using System.Collections;
using System.Collections.Generic;
using UnityVector3 = UnityEngine.Vector3;
using UnityEngine;
using kutuphanem; // oluþturulan namespace

public class karakterControl : MonoBehaviour
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

    void Start()
    {
        anim = GetComponent<Animator>();
        mainCam = Camera.main;
    }

    // karakter yönetimleri genellikle late update fonksiyonunda yapýlýr. daha yumuþak bir hareket saðlanýr.
    void LateUpdate()
    {
        // örnek fonksiyonu çaðýrmak.
        animasyon.deneme();

        // -------------------- ileri hareket --------------------------
        animasyon.ileriHareket();
        maksimumHiz = animasyon.hiziDisariAktar();
        inputX = animasyon.yonuDisariAktar();
        // -------------------- ileri hareket --------------------------

        // ----------------------------- geri hareket -------------------------------
        if (Input.GetKeyDown(KeyCode.S))
        {
            anim.SetBool("geriYuru", true);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            anim.SetBool("geriYuru", false);
        }
        // ----------------------------- geri hareket -------------------------------

        // ------------------------ sol hareket ------------------------------
        if (Input.GetKey(KeyCode.A))
        {
            anim.SetBool("solAktifmi", true);
            if (Input.GetKey(KeyCode.LeftShift))
            {
                anim.SetFloat("solHareket", .35f);
            }
            else if (Input.GetKey(KeyCode.W))
            {
                anim.SetFloat("solHareket", .5f);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                anim.SetFloat("solHareket", 1f);
            }
            else
            {
                anim.SetFloat("solHareket", .15f);
            } 
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetFloat("solHareket", 0);
            anim.SetBool("solAktifmi", false);
        }
        // ------------------------ sol hareket ------------------------------

        // -------------------------- sað hareket ----------------------------------
        if (Input.GetKey(KeyCode.D))
        {
            anim.SetBool("sagAktifmi", true);
            if (Input.GetKey(KeyCode.LeftShift))
            {
                anim.SetFloat("sagHareket", .35f);
            }
            else if (Input.GetKey(KeyCode.W))
            {
                anim.SetFloat("sagHareket", .5f);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                anim.SetFloat("sagHareket", 1f);
            }
            else
            {
                anim.SetFloat("sagHareket", .15f);
            }
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetFloat("sagHareket", 0);
            anim.SetBool("sagAktifmi", false);
        }
        // -------------------------- sað hareket ----------------------------------

        // -------------------------- eðilme hareket ----------------------------------
        if (Input.GetKey(KeyCode.C))
        {
            anim.SetBool("egilmeAktifmi", true);
            if (Input.GetKey(KeyCode.W))
            {
                anim.SetFloat("egilmeHareket", .32f);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                anim.SetFloat("egilmeHareket", .49f);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                anim.SetFloat("egilmeHareket", .66f);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                anim.SetFloat("egilmeHareket", .83f);
            }
            else
            {
                anim.SetFloat("egilmeHareket", .15f);
            }
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            anim.SetFloat("egilmeHareket", 0);
            anim.SetBool("egilmeAktifmi", false);
        }
        // -------------------------- eðilme hareket ----------------------------------

        mevcutYon = new Vector3(inputX, 0, inputZ);

        inputMove();
        inputRotation();
    }

    void inputMove()
    {
        // animatorün speed parametresine, karakter hýzýmýzý göndererek animasyonlar arasýn geçiþ yapabiliyoruz.
        // clampMagnitude ile hýz geçiþini normalleþtiriyoruz. Daha yumuþak bir geçiþ saðlýyor.
        anim.SetFloat("speed", Vector3.ClampMagnitude(mevcutYon,maksimumHiz).magnitude,maksimumUzunluk,Time.deltaTime * 10);
        
    }

    void inputRotation()
    {
        // kameranýn bakýþ açýsý.
        // TransformDirection : kameranýn baktýðý yönü alabiliyoruz.

        // mevcut yonden gelen vectore göre, horizontal yada vertical, transformDirection alýcak ve deðiþkene tanýmlayacak
        // sonrasýnda ise karakterin önü ilgili deðiþkenin vector yönüne dönücek.
        //Vector3 camOfSet = mainCam.transform.TransformDirection(mevcutYon);

        //----------------------------------------------

        // kamera hangi yöne dönerse mevcut yon almadan karakter dönmesini istiyorsak.
        Vector3 camOfSet = mainCam.transform.forward;

        camOfSet.y = 0;

        // slerp: iki vektör arasýnda yumuþak geçiþ yapmak için kullanýlýr.
        karakter.forward = Vector3.Slerp(karakter.forward, camOfSet, Time.deltaTime * donusHizi);
    }
}
