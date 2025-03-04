using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject winPanel;
    void Start()
    {
        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);
    }

   public void kaybettin()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
    }

    public void kazandin()
    {
        winPanel.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
    }

    public void yenidenOyna()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void anaMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
