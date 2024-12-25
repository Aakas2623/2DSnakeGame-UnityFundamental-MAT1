using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    public Button buttonLobby;
    public Button buttonPause;
    public Button buttonResume;
    //public Button buttonRestart;
    public GameObject pauseUI;


    public void Start()
    {
        buttonLobby.onClick.AddListener(MainMenu);
        buttonPause.onClick.AddListener(PauseLevel);
        buttonResume.onClick.AddListener(ResumeLevel);
        //buttonRestart.onClick.AddListener(ReloadLevel);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void PauseLevel()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeLevel()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
    }

    //private void ReloadLevel()
    //{
        
    //    SceneManager.LoadScene(1);
    //}
  
}

