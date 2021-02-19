using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingBall : MonoBehaviour
{
    [SerializeField]
    private float speed = 100f;

    [SerializeField]
    private GameObject explodeFX;

    private Rigidbody rb;
    private Vector3 startPos;

    private void Start()
    {
        this.rb = GetComponent<Rigidbody>();
        this.startPos = this.rb.position;
    }

    private void FixedUpdate()
    {
        Vector3 force = Vector3.right * this.speed * Time.fixedDeltaTime;
        this.rb.velocity = new Vector3(force.x, this.rb.velocity.y, force.z);

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
