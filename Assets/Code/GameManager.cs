using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool isGameOver = false;
    public bool isPaused = false;
    public Text gameOverText;
    public static GameManager singleton;
    public GameObject GameOverBg;
    public Button ContinueButton;
    public Text ContinueButtonText;

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MenuButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }


    public void Pause()
    {
        isPaused = true;
        Setup();
        singleton.gameOverText.text = "PAUSED";
        Time.timeScale = 0;
    }
    public void Resume()
    {
        isPaused = false;
        singleton.GameOverBg.SetActive(false);
        Time.timeScale = 1;
    }

    public void pressContinueButton()
    {
        if (isGameOver)
        {
            RestartButton();
        } else
        {
            Resume();
        }
    }

    private void Update()
    {
        if (isGameOver)
        {
            ContinueButtonText.text = "RESTART";
            return;
        }
        else
        {
            ContinueButtonText.text = "RESUME";
        }
        
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                Resume();
            } else
            {
                Pause();
            }
        }
    }


}
