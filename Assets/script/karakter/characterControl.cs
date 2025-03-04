using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class characterControl : MonoBehaviour
{
    public Image healthBar;
    float saglik;
    public gameManager gameManager;
    void Start()
    {
        saglik = 100;
        healthBar.fillAmount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void saglikDurumu(float darbeGucu)
    {
        saglik -= darbeGucu;

        healthBar.fillAmount = saglik / 100;


        if (saglik <= 0)
        {
            gameManager.GetComponent<gameManager>().kaybettin();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("oyunSonu"))
        {
            gameManager.GetComponent<gameManager>().kazandin();
        }
    }
}
