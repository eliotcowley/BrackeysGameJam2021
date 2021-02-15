using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    [Range(0f, 1f)]
    private float turnSpeed = 0.2f;

    private Rigidbody rb;

    private void Start()
    {
        this.rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw(Constants.Input_Horizontal);
        float vertical = Input.GetAxisRaw(Constants.Input_Vertical);
        Vector3 direction = new Vector3(horizontal, 0f, vertical);

        if (direction.magnitude > 1f)
        {
            direction.Normalize();
        }

        Vector3 newPos = this.rb.position + direction * this.speed * Time.fixedDeltaTime;
        this.rb.MovePosition(newPos);

        if (direction != Vector3.zero)
        {
            Quaternion lookRot = Quaternion.LookRotation(direction, Vector3.up);
            Quaternion smoothRot = Quaternion.Lerp(this.rb.rotation, lookRot, this.turnSpeed);
            this.rb.MoveRotation(smoothRot);
        }
    }
}
