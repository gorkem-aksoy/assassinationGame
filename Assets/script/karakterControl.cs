using System.Collections;
using System.Collections.Generic;
using UnityVector3 = UnityEngine.Vector3;
using UnityEngine;
using kutuphanem; // olu�turulan namespace

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

    // k�t�phanem de ki animasyon class�m �rneklendi. kullan�ma haz�r
    animasyon animasyon = new animasyon(); 

    void Start()
    {
        anim = GetComponent<Animator>();
        mainCam = Camera.main;
    }

    // karakter y�netimleri genellikle late update fonksiyonunda yap�l�r. daha yumu�ak bir hareket sa�lan�r.
    void LateUpdate()
    {
        // �rnek fonksiyonu �a��rmak.
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

        // -------------------------- sa� hareket ----------------------------------
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
        // -------------------------- sa� hareket ----------------------------------

        // -------------------------- e�ilme hareket ----------------------------------
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
        // -------------------------- e�ilme hareket ----------------------------------

        mevcutYon = new Vector3(inputX, 0, inputZ);

        inputMove();
        inputRotation();
    }

    void inputMove()
    {
        // animator�n speed parametresine, karakter h�z�m�z� g�ndererek animasyonlar aras�n ge�i� yapabiliyoruz.
        // clampMagnitude ile h�z ge�i�ini normalle�tiriyoruz. Daha yumu�ak bir ge�i� sa�l�yor.
        anim.SetFloat("speed", Vector3.ClampMagnitude(mevcutYon,maksimumHiz).magnitude,maksimumUzunluk,Time.deltaTime * 10);
        
    }

    void inputRotation()
    {
        // kameran�n bak�� a��s�.
        // TransformDirection : kameran�n bakt��� y�n� alabiliyoruz.

        // mevcut yonden gelen vectore g�re, horizontal yada vertical, transformDirection al�cak ve de�i�kene tan�mlayacak
        // sonras�nda ise karakterin �n� ilgili de�i�kenin vector y�n�ne d�n�cek.
        //Vector3 camOfSet = mainCam.transform.TransformDirection(mevcutYon);

        //----------------------------------------------

        // kamera hangi y�ne d�nerse mevcut yon almadan karakter d�nmesini istiyorsak.
        Vector3 camOfSet = mainCam.transform.forward;

        camOfSet.y = 0;

        // slerp: iki vekt�r aras�nda yumu�ak ge�i� yapmak i�in kullan�l�r.
        karakter.forward = Vector3.Slerp(karakter.forward, camOfSet, Time.deltaTime * donusHizi);
    }
}
