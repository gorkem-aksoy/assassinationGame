using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace kutuphanem
{
    // animasyon i�lemlerinin tamam� burada
    public class animasyon
    {
        /*public void deneme()
        {
            Debug.Log("selam");
        }*/

        // ileri hareket fonksiyonunda i�lenen de�erleri karakterControl scriptine aktarmak i�in kullan�lacak
        float maxSpeedClass;
        float inputXClass;
       
        // ileri hareket i�lemlerinin i�lenmesi
        // karakterControl scriptinden ilgili h�zlar� alarak, k�t�phanemizi dinamikle�tiriyoruz.
        // ileri hareket ve inputMove karakter Hareket fonksiyonunda birle�ti.
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
            // animator�n speed parametresine, karakter h�z�m�z� g�ndererek animasyonlar aras�n ge�i� yapabiliyoruz.
            // clampMagnitude ile h�z ge�i�ini normalle�tiriyoruz. Daha yumu�ak bir ge�i� sa�l�yor.
            anim.SetFloat(animHizDegeri, Vector3.ClampMagnitude(new Vector3(inputXClass,0,0), maxSpeedClass).magnitude, maksimumUzunluk, Time.deltaTime * 10);
        }

        // karakterControl scriptinde ki inputRotation fonksiyonu
        public void karakterDonus(Camera mainCam, float donusHizi, Transform karakter)
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

        // geri hareket i�lemlerinin i�lenmesi
        // animatordeki ana parametreyi ald�k.
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

        // sol hareket i�lemlerinin i�lenmesi
        // sol ana parametreyi ve kontor parametreyi d��ar�dan ald�k. k�t�phane dinamikle�ti
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

        // sa� hareket i�lemlerinin i�lenmesi
        // sa� ana parametreyi ve kontor parametreyi d��ar�dan ald�k. k�t�phane dinamikle�ti
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

        // egilme hareket i�lemlerinin i�lenmesi
        // egilme ana parametreyi ve kontor parametreyi d��ar�dan ald�k. k�t�phane dinamikle�ti
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

        // animasyon ge�i� de�erleri 
        // karakterControl scriptinin ilgili y�nlerinden de�erler gelicek
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


