using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : Singleton<PlayerMovement>
{
    [HideInInspector]
    public Rigidbody Rb;

    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    [Range(0f, 1f)]
    private float turnSpeed = 0.2f;

    [SerializeField]
    private float groundCheckRayDistance = 0.3f;

    private void Awake()
    {
        this.Rb = GetComponent<Rigidbody>();
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

        Vector3 newPos = this.Rb.position + direction * this.speed * Time.fixedDeltaTime;
        this.Rb.MovePosition(newPos);

        if (direction != Vector3.zero)
        {
            Quaternion lookRot = Quaternion.LookRotation(direction, Vector3.up);
            Quaternion smoothRot = Quaternion.Lerp(this.Rb.rotation, lookRot, this.turnSpeed);
            this.Rb.MoveRotation(smoothRot);
        }

        Debug.DrawRay(this.Rb.position, Vector3.down * this.groundCheckRayDistance);
        
        if (Physics.Raycast(this.Rb.position, Vector3.down, this.groundCheckRayDistance))
        {
            this.Rb.isKinematic = true;
        }
        else
        {
            this.Rb.isKinematic = false;
        }
    }
}
