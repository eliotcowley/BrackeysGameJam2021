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
    }

    public void GameOver()
    {
        PlayerMovement.Instance.CanMove = false;
        UIManager.Instance.ShowGameOverText();
        this.gameOver = true;
    }
}
