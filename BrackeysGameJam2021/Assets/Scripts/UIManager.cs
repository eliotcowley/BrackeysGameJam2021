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
}
