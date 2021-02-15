using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class GerbilFollower : MonoBehaviour
{
    [HideInInspector]
    public NavMeshAgent Agent;

    [HideInInspector]
    public bool InSwarm = false;

    private void Start()
    {
        this.Agent = GetComponent<NavMeshAgent>();
    }

    private void FixedUpdate()
    {
        if (this.InSwarm)
        {
            this.Agent.SetDestination(PlayerMovement.Instance.Rb.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.Tag_GerbilMain))
        {
            if (!this.InSwarm)
            {
                this.InSwarm = true;
                GerbilMain.Instance.SwarmCount++;
            }
        }
    }
}
