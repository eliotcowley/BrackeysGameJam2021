﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector]
    public List<GerbilFollower> GerbilsInSwarm;

    [HideInInspector]
    public float DeathY = -10f;

    [SerializeField]
    private AudioMixer sfxMixer;

    [SerializeField]
    private AudioSource gerbilDieSFX;

    [SerializeField]
    private AudioSource winSFX;

    [SerializeField]
    private AudioSource humanDieSFX;

    private int level = 0;
    public int Level
    {
        get
        {
            return this.level;
        }
        set
        {
            this.level = value;
            UIManager.Instance.UpdateLevelText(this.level);
        }
    }

    private bool gameOver = false;
    private bool paused = false;
    private bool levelWon = false;

    [SerializeField]
    private float fpsRefreshTime = 0.5f;

    private int frameCount = 0;
    private float fpsTimer = 0f;
    private int lastFps = 0;

    private void Awake()
    {
        this.GerbilsInSwarm = new List<GerbilFollower>();
        this.Level = SceneManager.GetActiveScene().buildIndex;
    }

    private void Start()
    {
        if (this.Level == 10)
        {
            MusicController.Instance.PlayBossSong();
        }
    }

    private void Update()
    {
        //CalculateFps();

        if (this.gameOver)
        {
            if (Input.GetButtonDown(Constants.Input_Attack))
            {
                Restart();
            }

            if (Input.GetButtonDown(Constants.Input_Back))
            {
                Quit();
            }
        }
        else
        {
            if (Input.GetButtonDown(Constants.Input_Pause) && !this.levelWon)
            {
                TogglePause(!this.paused);
            }
        }

        if (this.levelWon)
        {
            if (Input.GetButtonDown(Constants.Input_Attack))
            {
                GoToNextLevel();
            }
        }
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Win();
        }
#endif
    }

    public void GameOver()
    {
        PlayerMovement.Instance.CanMove = false;
        UIManager.Instance.ShowGameOverText();
        this.gameOver = true;
        PlayerMovement.Instance.NewVelocity = Vector3.zero;
    }

    public void TogglePause(bool paused)
    {
        this.paused = paused;
        UIManager.Instance.TogglePause(this.paused);
        Time.timeScale = this.paused ? 0f : 1f;

        if (this.paused)
        {
            PlayerMovement.Instance.CanMove = false;
            this.sfxMixer.SetFloat("Volume", -80f);
        }
        else
        {
            StartCoroutine(EnableMovementAfterDelay());
            this.sfxMixer.SetFloat("Volume", 0f);
        }
    }

    private IEnumerator EnableMovementAfterDelay()
    {
        yield return new WaitForEndOfFrame();
        PlayerMovement.Instance.CanMove = true;
    }

    public void Quit()
    {
        SceneManager.LoadScene(0);
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

    public void Restart()
    {
        TogglePause(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Win()
    {
        if (!this.levelWon)
        {
            UIManager.Instance.ShowWinText();
            this.levelWon = true;
            PlayerMovement.Instance.CanMove = false;
            PlayerMovement.Instance.NewVelocity = Vector3.zero;
            this.winSFX.Play();
        }
    }

    private void GoToNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    public Vector3 GetAverageGerbilPosition()
    {
        Vector3 total = this.GerbilsInSwarm.Aggregate(
            Vector3.zero,
            (workingTotal, next) => workingTotal + next.transform.position,
            workingTotal => workingTotal);

        return total / this.GerbilsInSwarm.Count;
    }

    public void PlayGerbilDieSFX()
    {
        this.gerbilDieSFX.Play();
    }

    public void PlayHumanDieSFX()
    {
        this.humanDieSFX.Play();
    }
}
