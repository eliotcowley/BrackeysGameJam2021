using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField]
    private TextMeshProUGUI swarmCountText;

    public void UpdateSwarmCountText()
    {
        this.swarmCountText.SetText(GerbilMain.Instance.SwarmCount.ToString());
    }
}
