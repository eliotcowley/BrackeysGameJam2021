using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector]
    public List<GerbilFollower> GerbilsInSwarm;

    private bool gameOver = false;
    private bool paused = false;

    private void Start()
    {
        this.GerbilsInSwarm = new List<GerbilFollower>();
    }

    private void Update()
    {
        if (this.gameOver)
        {
            if (Input.GetButtonDown(Constants.Input_Attack))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

            if (Input.GetButtonDown(Constants.Input_Back))
            {
                Quit();
            }
        }
        else
        {
            if (Input.GetButtonDown(Constants.Input_Pause))
            {
                TogglePause(!this.paused);
            }
        }

        if (this.paused)
        {
            if (Input.GetButtonDown(Constants.Input_Attack))
            {
                TogglePause(false);
            }

            if (Input.GetButtonDown(Constants.Input_Back))
            {
                Quit();
            }
        }
    }

    public void GameOver()
    {
        PlayerMovement.Instance.CanMove = false;
        UIManager.Instance.ShowGameOverText();
        this.gameOver = true;
    }

    private void TogglePause(bool paused)
    {
        this.paused = paused;
        UIManager.Instance.TogglePauseText(this.paused);
        Time.timeScale = this.paused ? 0f : 1f;
    }

    private void Quit()
    {
        if (Application.isEditor)
        {
            EditorApplication.isPlaying = false;
        }
        else
        {
            Application.Quit();
        }
    }
}
