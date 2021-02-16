using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerbilAttack : Singleton<GerbilAttack>
{
    [HideInInspector]
    public Human TargetHuman;

    public float AttackSpeed = 100f;
    public float DamagePerGerbil = 1f;

    private void Update()
    {
        if (Input.GetButtonDown(Constants.Input_Attack))
        {
            if (this.TargetHuman != null)
            {
                Attack();
            }
        }
    }

    private void Attack()
    {
        this.TargetHuman.ButtonIcon.SetActive(false);

        // Move gerbils toward human
        foreach (GerbilFollower gerbil in GameManager.Instance.GerbilsInSwarm)
        {
            gerbil.Attacking = true;
        }
    }

    public void StopAttacking()
    {
        foreach (GerbilFollower gerbil in GameManager.Instance.GerbilsInSwarm)
        {
            gerbil.Attacking = false;
        }
    }
}
