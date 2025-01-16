using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace kutuphanem
{
    public class animasyon
    {
        public void deneme()
        {
            Debug.Log("selam");
        }

        // ileri hareket fonksiyonunda iþlenen deðerleri karakterControl scriptine aktarmak için kullanýlacak
        float maxSpeedClass;
        float inputXClass;
       
        // ileri hareket iþlemlerinin iþlenmesi
        public void ileriHareket()
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                maxSpeedClass = 1f;
            }
            else if (Input.GetKey(KeyCode.W))
            {             
                maxSpeedClass = 0.2f;
                inputXClass = 1;
            }
            else
            {
                maxSpeedClass = 0;
                inputXClass = 0;
            }

        }

        // deðiþkenlerin yeni oluþan deðerlerini, tekrardan karakterControl scriptine göndermeliyiz ki deðerler iþlensin
        public float hiziDisariAktar()
        {
            return maxSpeedClass;
        }
        public float yonuDisariAktar()
        {
            return inputXClass;
        }
    }
}


