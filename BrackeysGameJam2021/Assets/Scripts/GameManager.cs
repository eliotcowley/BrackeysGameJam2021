using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector]
    public List<GerbilFollower> GerbilsInSwarm;

    private void Start()
    {
        this.GerbilsInSwarm = new List<GerbilFollower>();
    }
}
