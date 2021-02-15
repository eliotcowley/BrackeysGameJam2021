using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerbilMain : Singleton<GerbilMain>
{
    private int swarmCount = 0;
    public int SwarmCount
    {
        get
        {
            return this.swarmCount;
        }
        set
        {
            this.swarmCount = value;
            UIManager.Instance.UpdateSwarmCountText();
        }
    }
}
