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

    [SerializeField]
    private float fpsRefreshTime = 0.5f;

    private int frameCount = 0;
    private float fpsTimer = 0f;
    private int lastFps = 0;

    private void Start()
    {
        this.GerbilsInSwarm = new List<GerbilFollower>();
    }

    private void Update()
    {
        CalculateFps();

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
        PlayerMovement.Instance.CanMove = !this.paused;
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

    private void CalculateFps()
    {
        if (this.fpsTimer < this.fpsRefreshTime)
        {
            this.fpsTimer += Time.deltaTime;
            this.frameCount++;
        }
        else
        {
            this.lastFps = (int)(this.frameCount / this.fpsTimer);
            this.frameCount = 0;
            this.fpsTimer = 0f;
        }

        UIManager.Instance.SetFpsText(this.lastFps);
    }
}
