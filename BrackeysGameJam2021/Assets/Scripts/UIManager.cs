using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField]
    private TextMeshProUGUI swarmCountText;

    [SerializeField]
    private GameObject gameOverText;

    [SerializeField]
    private GameObject pausePanel;

    [SerializeField]
    private TextMeshProUGUI fpsText;

    [SerializeField]
    private Selectable pauseFirstSelected;

    [SerializeField]
    private GameObject winText;

    public void UpdateSwarmCountText()
    {
        this.swarmCountText.SetText(GameManager.Instance.GerbilsInSwarm.Count.ToString());
    }

    public void ShowGameOverText()
    {
        this.gameOverText.SetActive(true);
    }

    public void TogglePause(bool show)
    {
        this.pausePanel.SetActive(show);

        if (show)
        {
            EventSystem.current.SetSelectedGameObject(null);
            this.pauseFirstSelected.Select();
        }
    }

    public void SetFpsText(int fps)
    {
        this.fpsText.SetText($"FPS: {fps}");
    }

    public void OnResumeButtonPressed()
    {
        GameManager.Instance.TogglePause(false);
    }

    public void OnRestartButtonPressed()
    {
        GameManager.Instance.Restart();
    }

    public void OnQuitButtonPressed()
    {
        GameManager.Instance.Quit();
    }

    public void ShowWinText()
    {
        this.winText.SetActive(true);
    }
}
