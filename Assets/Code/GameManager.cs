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
    public GameObject InstructionsBg;
    public Text InstructionsButtonText;

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

    public void ShowInstructions()
    {

        if (isGameOver)
        {
            InstructionsButtonText.text = "Restart";
        } else
        {
            InstructionsButtonText.text = "Resume";
        }
        singleton.InstructionsBg.SetActive(true);
        singleton.GameOverBg.SetActive(false);
    }

    public void Pause()
    {
        isPaused = true;
        Setup();
        singleton.gameOverText.text = "Game Paused";
        Time.timeScale = 0;
    }
    public void Resume()
    {
        isPaused = false;
        singleton.GameOverBg.SetActive(false);
        singleton.InstructionsBg.SetActive(false);
        Time.timeScale = 1;
    }

    public void pressInstructionsButton()
    {
        if(isGameOver)
        {
            RestartButton();
        } else
        {
            Resume();
        }
        singleton.InstructionsBg.SetActive(false);
    }

    private void Update()
    {
        if(isGameOver)
        {
            return;
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
