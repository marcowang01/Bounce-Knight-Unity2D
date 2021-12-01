using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool isGameOver = false;
    public Text gameOverText;
    public static GameManager singleton;
    public GameObject GameOverBg;

    private void Start()
    {
        singleton = this;
    }

    static public void setGameOver()
    {
        Setup();
        singleton.gameOverText.text = "GAME OVER";
        singleton.isGameOver = true;
        PlayerController p = FindObjectOfType<PlayerController>();
        p.gameObject.SetActive(false);
    }
    static public void setGameWin()
    {
        Setup();
        singleton.gameOverText.text = "YOU WIN";
        singleton.isGameOver = true;
    }

    static public bool checkGameOver()
    {
        return singleton.isGameOver;
    }

    static public void Setup()
    {
        singleton.GameOverBg.SetActive(true);
    }


    public void RestartButton()
    {
        SceneManager.LoadScene("Main");
    }
}
