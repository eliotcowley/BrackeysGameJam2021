using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private float moveY = -5f;

    [SerializeField]
    private float lerpTime = 1f;

    public void Open()
    {
        LeanTween.moveY(this.gameObject, this.transform.position.y + this.moveY, this.lerpTime);
    }
}
