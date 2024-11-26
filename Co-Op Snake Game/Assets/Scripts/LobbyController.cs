using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{

    public Button buttonPlay;
    public Button buttonExit;
    public Button buttonCoOp;

    private void Awake()
    {
        buttonPlay.onClick.AddListener(PlayGame);
        buttonExit.onClick.AddListener(QuitGame);
        buttonCoOp.onClick.AddListener(CoOpGame);
    }

    private void PlayGame()
    {
        SceneManager.LoadScene(1);
        this.gameObject.SetActive(false);
    }
    
    private void CoOpGame()
    {
        SceneManager.LoadScene(2);
        this.gameObject.SetActive(false);
    }

    public void QuitGame()

    {

        Application.Quit();

        Debug.Log("Quit");

    }
}
