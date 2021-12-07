using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public static MenuController singleton;
    public GameObject InstructionsBg;

    // audio
    public AudioSource source;
    public AudioClip click;

    private void Start()
    {
        singleton = this;
    }

    static public void Setup()
    {
        singleton.InstructionsBg.SetActive(true);
    }


    public void EasyButton()
    {
        source.PlayOneShot(click);
        SceneManager.LoadScene("Wizard");
    }

    public void BossButton()
    {
        source.PlayOneShot(click);
        SceneManager.LoadScene("Main");
    }

    public void InstructionButton()
    {
        source.PlayOneShot(click);
        singleton.InstructionsBg.SetActive(true);
    }

    public void Back()
    {
        source.PlayOneShot(click);
        singleton.InstructionsBg.SetActive(false);
    }
}