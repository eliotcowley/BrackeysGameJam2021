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

    public void UpdateSwarmCountText()
    {
        this.swarmCountText.SetText(GameManager.Instance.GerbilsInSwarm.Count.ToString());
    }

    public void ShowGameOverText()
    {
        this.gameOverText.SetActive(true);
    }
}
