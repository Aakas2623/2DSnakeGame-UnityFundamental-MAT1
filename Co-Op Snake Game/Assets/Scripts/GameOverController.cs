using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private Button buttonRestart;
    [SerializeField] private Button buttonLobby;

    private void Awake()
    {
        buttonRestart.onClick.AddListener(ReloadLevel);
        buttonLobby.onClick.AddListener(MainMenu);
    }

    public void PlayerDied()
    {
        this.gameObject.SetActive(true);
    }

    private void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        this.gameObject.SetActive(false);
    }
}

