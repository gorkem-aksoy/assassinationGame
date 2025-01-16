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

        // ileri hareket fonksiyonunda i�lenen de�erleri karakterControl scriptine aktarmak i�in kullan�lacak
        float maxSpeedClass;
        float inputXClass;
       
        // ileri hareket i�lemlerinin i�lenmesi
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

        // de�i�kenlerin yeni olu�an de�erlerini, tekrardan karakterControl scriptine g�ndermeliyiz ki de�erler i�lensin
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


