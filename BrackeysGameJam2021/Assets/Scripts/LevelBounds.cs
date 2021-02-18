using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBounds : Singleton<LevelBounds>
{
    private Bounds bounds;

    private void Start()
    {
        this.bounds = GetComponent<Collider>().bounds;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Constants.Tag_GerbilFollower))
        {
            other.GetComponent<GerbilFollower>().Fall();
        }
    }

    public bool IsInLevelBounds(Vector3 position)
    {
        return this.bounds.Contains(position);
    }
}
