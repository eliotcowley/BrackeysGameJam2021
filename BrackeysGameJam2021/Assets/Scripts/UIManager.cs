using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField]
    private TextMeshProUGUI swarmCountText;

    [SerializeField]
    private GameObject gameOverText;

    [SerializeField]
    private GameObject pausedText;

    [SerializeField]
    private TextMeshProUGUI fpsText;

    public void UpdateSwarmCountText()
    {
        this.swarmCountText.SetText(GameManager.Instance.GerbilsInSwarm.Count.ToString());
    }

    public void ShowGameOverText()
    {
        this.gameOverText.SetActive(true);
    }

    public void TogglePauseText(bool show)
    {
        this.pausedText.SetActive(show);
    }

    public void SetFpsText(int fps)
    {
        this.fpsText.SetText($"FPS: {fps}");
    }
}
