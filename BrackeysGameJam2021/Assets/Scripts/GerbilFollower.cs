﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class GerbilFollower : MonoBehaviour
{
    [HideInInspector]
    public bool InSwarm = false;

    [HideInInspector]
    public bool Attacking = false;

    [SerializeField]
    private float speed = 0.5f;

    [SerializeField]
    [Range(0f, 1f)]
    private float turnSpeed = 0.5f;

    [SerializeField]
    private float maxDistanceFromCenter = 2f;

    [SerializeField]
    private float deathY = -10f;

    [SerializeField]
    private GameObject gerbilDieFXPrefab;

    [SerializeField]
    private float attackInterval = 0.5f;

    private Rigidbody rb;
    private float attackTimer = 0f;

    private void Start()
    {
        this.rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (this.InSwarm && Vector3.Distance(this.rb.position, PlayerMovement.Instance.Rb.position) > this.maxDistanceFromCenter && !this.Attacking)
        {
            Vector3 direction = (PlayerMovement.Instance.Rb.position - this.rb.position) * this.speed * Time.fixedDeltaTime;
            this.rb.velocity = new Vector3(direction.x, this.rb.velocity.y, direction.z);

            Quaternion lookRot = Quaternion.LookRotation(direction, Vector3.up);
            Quaternion smoothRot = Quaternion.Lerp(this.rb.rotation, lookRot, this.turnSpeed);
            this.rb.MoveRotation(smoothRot);
        }

        if (this.Attacking)
        {
            Vector3 direction = 
                (GerbilAttack.Instance.TargetHuman.transform.position - this.rb.position) 
                * GerbilAttack.Instance.AttackSpeed 
                * Time.fixedDeltaTime;

            this.rb.velocity = direction;

            Quaternion lookRot = Quaternion.LookRotation(direction, Vector3.up);
            Quaternion smoothRot = Quaternion.Lerp(this.rb.rotation, lookRot, this.turnSpeed);
            this.rb.MoveRotation(smoothRot);

            this.attackTimer += Time.fixedDeltaTime;

            if (this.attackTimer >= this.attackInterval)
            {
                GerbilAttack.Instance.TargetHuman.TakeDamage(GerbilAttack.Instance.DamagePerGerbil);
                this.attackTimer = 0f;
            }
        }

        if (this.rb.position.y < this.deathY)
        {
            Die();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.Tag_GerbilMain))
        {
            if (!this.InSwarm)
            {
                this.InSwarm = true;
                GameManager.Instance.GerbilsInSwarm.Add(this);
                UIManager.Instance.UpdateSwarmCountText();
            }
        }
    }

    public void Die()
    {
        Instantiate(this.gerbilDieFXPrefab, this.rb.position, Quaternion.identity);
        GameManager.Instance.GerbilsInSwarm.Remove(this);
        UIManager.Instance.UpdateSwarmCountText();
        Destroy(this.gameObject);
    }
}