using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingBall : MonoBehaviour
{
    [SerializeField]
    private float speed = 100f;

    [SerializeField]
    private GameObject explodeFX;

    [SerializeField]
    private GameObject spawnPoint;

    private Rigidbody rb;
    private Vector3 startPos;
    private Vector3 force;

    private void Start()
    {
        this.rb = GetComponentInChildren<Rigidbody>();
        this.startPos = this.spawnPoint.transform.position;
        this.force = this.transform.right * this.speed;
    }

    private void FixedUpdate()
    {
        this.rb.velocity = new Vector3(this.force.x, this.rb.velocity.y, this.force.z);

        if (this.rb.position.y <= GameManager.Instance.DeathY)
        {
            Explode();
        }
    }

    private void Explode()
    {
        Instantiate(this.explodeFX, this.rb.position, Quaternion.identity);
        this.rb.position = this.startPos;
    }
}
