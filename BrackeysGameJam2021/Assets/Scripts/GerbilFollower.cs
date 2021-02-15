using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class GerbilFollower : MonoBehaviour
{
    private NavMeshAgent agent;

    private void Start()
    {
        this.agent = GetComponent<NavMeshAgent>();
    }

    private void FixedUpdate()
    {
        this.agent.SetDestination(PlayerMovement.Instance.Rb.position);
    }
}
