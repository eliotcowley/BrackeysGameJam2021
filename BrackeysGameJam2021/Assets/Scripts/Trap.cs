using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private Animator animator;
    private Animation anim;

    private void Start()
    {
        this.animator = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.Tag_GerbilFollower))
        {
            this.animator.SetTrigger(Constants.Anim_TrapCatch);
            other.GetComponentInParent<GerbilFollower>().Die();
        }
    }
}
