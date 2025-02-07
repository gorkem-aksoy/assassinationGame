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

        // ileri hareket fonksiyonunda işlenen değerleri karakterControl scriptine aktarmak için kullanılacak
        float maxSpeedClass;
        float inputXClass;
       
        // ileri hareket işlemlerinin işlenmesi
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

        // değişkenlerin yeni oluşan değerlerini, tekrardan karakterControl scriptine göndermeliyiz ki değerler işlensin
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


