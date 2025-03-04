using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace kutuphanem
{
    // animasyon iþlemlerinin tamamý burada
    public class animasyon
    {
        /*public void deneme()
        {
            Debug.Log("selam");
        }*/

        // ileri hareket fonksiyonunda iþlenen deðerleri karakterControl scriptine aktarmak için kullanýlacak
        float maxSpeedClass;
        float inputXClass;
       
        // ileri hareket iþlemlerinin iþlenmesi
        // karakterControl scriptinden ilgili hýzlarý alarak, kütüphanemizi dinamikleþtiriyoruz.
        // ileri hareket ve inputMove karakter Hareket fonksiyonunda birleþti.
        public void karakterHareket(Animator anim, string animHizDegeri, float maksimumUzunluk, float kosmaHizi, float yurumeHizi)
        {

            if (Input.GetKey(KeyCode.LeftShift))
            {
                maxSpeedClass = kosmaHizi;
            }
            else if (Input.GetKey(KeyCode.W))
            {
                maxSpeedClass = yurumeHizi;
                inputXClass = 1;
            }
            else
            {
                maxSpeedClass = 0;
                inputXClass = 0;
            }
            // animatorün speed parametresine, karakter hýzýmýzý göndererek animasyonlar arasýn geçiþ yapabiliyoruz.
            // clampMagnitude ile hýz geçiþini normalleþtiriyoruz. Daha yumuþak bir geçiþ saðlýyor.
            anim.SetFloat(animHizDegeri, Vector3.ClampMagnitude(new Vector3(inputXClass,0,0), maxSpeedClass).magnitude, maksimumUzunluk, Time.deltaTime * 10);
        }

        // karakterControl scriptinde ki inputRotation fonksiyonu
        public void karakterDonus(Camera mainCam, float donusHizi, Transform karakter)
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

        // geri hareket iþlemlerinin iþlenmesi
        // animatordeki ana parametreyi aldýk.
        public void geriHareket(Animator anim, string geriAnimParametre)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                anim.SetBool(geriAnimParametre, true);
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                anim.SetBool(geriAnimParametre, false);
            }
        }

        // sol hareket iþlemlerinin iþlenmesi
        // sol ana parametreyi ve kontor parametreyi dýþarýdan aldýk. kütüphane dinamikleþti
        public void solHareket(Animator anim, string solAnimParametre, string solAnimKontrolParametre, 
            List<float> solYonGecisParametreleri)
        {
            if (Input.GetKey(KeyCode.A))
            {
                anim.SetBool(solAnimKontrolParametre, true);
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    anim.SetFloat(solAnimParametre, solYonGecisParametreleri[1]);
                }
                else if (Input.GetKey(KeyCode.W))
                {
                    anim.SetFloat(solAnimParametre, solYonGecisParametreleri[2]);
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    anim.SetFloat(solAnimParametre, solYonGecisParametreleri[3]);
                }
                else
                {
                    anim.SetFloat(solAnimParametre, solYonGecisParametreleri[0]);
                }
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                anim.SetFloat(solAnimParametre, 0);
                anim.SetBool(solAnimKontrolParametre, false);
            }
        }

        // sað hareket iþlemlerinin iþlenmesi
        // sað ana parametreyi ve kontor parametreyi dýþarýdan aldýk. kütüphane dinamikleþti
        public void sagHareket(Animator anim, string sagAnimParametre, string sagAnimKontrolParametre,
            List<float> sagYonGecisParametreleri)
        {
            if (Input.GetKey(KeyCode.D))
            {
                anim.SetBool(sagAnimKontrolParametre, true);
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    anim.SetFloat(sagAnimParametre, sagYonGecisParametreleri[1]);
                }
                else if (Input.GetKey(KeyCode.W))
                {
                    anim.SetFloat(sagAnimParametre, sagYonGecisParametreleri[2]);
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    anim.SetFloat(sagAnimParametre, sagYonGecisParametreleri[3]);
                }
                else
                {
                    anim.SetFloat(sagAnimParametre, sagYonGecisParametreleri[0]);
                }
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                anim.SetFloat(sagAnimParametre, 0);
                anim.SetBool(sagAnimKontrolParametre, false);
            }
        }

        // egilme hareket iþlemlerinin iþlenmesi
        // egilme ana parametreyi ve kontor parametreyi dýþarýdan aldýk. kütüphane dinamikleþti
        public void egilmeHareket(Animator anim, string egilmeAnimParametre, string egilmeAnimKontrolParametre,
            List<float> egilmeYonGecisParametreleri)
        {
            if (Input.GetKey(KeyCode.C))
            {
                anim.SetBool(egilmeAnimKontrolParametre, true);
                if (Input.GetKey(KeyCode.W))
                {
                    anim.SetFloat(egilmeAnimParametre, egilmeYonGecisParametreleri[1]);
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    anim.SetFloat(egilmeAnimParametre, egilmeYonGecisParametreleri[2]);
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    anim.SetFloat(egilmeAnimParametre, egilmeYonGecisParametreleri[3]);
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    anim.SetFloat(egilmeAnimParametre, egilmeYonGecisParametreleri[4]);
                }
                else
                {
                    anim.SetFloat(egilmeAnimParametre, egilmeYonGecisParametreleri[0]);
                }
            }
            if (Input.GetKeyUp(KeyCode.C))
            {
                anim.SetFloat(egilmeAnimParametre, 0);
                anim.SetBool(egilmeAnimKontrolParametre, false);
            }
        }

        // animasyon geçiþ deðerleri 
        // karakterControl scriptinin ilgili yönlerinden deðerler gelicek
        public List<float> gecisDegerleri(float[] deger)
        {
            List<float> animGecisDegerleri = new List<float>();

            foreach ( float item in deger)
            {
                animGecisDegerleri.Add(item);
            }

            return animGecisDegerleri;
        }
    }
}


