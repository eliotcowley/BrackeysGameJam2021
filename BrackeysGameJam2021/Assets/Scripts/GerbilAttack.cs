﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerbilAttack : Singleton<GerbilAttack>
{
    [HideInInspector]
    public Human TargetHuman;

    [HideInInspector]
    public bool IsAttacking = false;

    public float AttackSpeed = 100f;
    public float DamagePerGerbil = 1f;

    private void Update()
    {
        if (Input.GetButtonDown(Constants.Input_Attack))
        {
            if (this.TargetHuman != null && !this.IsAttacking)
            {
                Attack();
            }
        }

        if (Input.GetButtonDown(Constants.Input_Back))
        {
            if (this.IsAttacking)
            {
                StopAttacking();
            }
        }
    }

    private void Attack()
    {
        this.TargetHuman.ButtonIcon.SetActive(false);
        this.IsAttacking = true;
        this.TargetHuman.SetTargetAttackColor(true);

        // Move gerbils toward human
        foreach (GerbilFollower gerbil in GameManager.Instance.GerbilsInSwarm)
        {
            gerbil.Attacking = true;
        }
    }

    public void StopAttacking()
    {
        this.IsAttacking = false;
        this.TargetHuman.SetTargetAttackColor(false);
        
        if (this.TargetHuman.InAttackRange && this.TargetHuman.Health > 0f)
        {
            this.TargetHuman.ButtonIcon.SetActive(true);
        }

        foreach (GerbilFollower gerbil in GameManager.Instance.GerbilsInSwarm)
        {
            gerbil.Attacking = false;
        }
    }
}
