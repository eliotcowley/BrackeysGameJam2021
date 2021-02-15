using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerbilDieFX : MonoBehaviour
{
    [SerializeField]
    private float destroyDelay = 1f;

    private void Start()
    {
        Destroy(this.gameObject, this.destroyDelay);
    }
}
