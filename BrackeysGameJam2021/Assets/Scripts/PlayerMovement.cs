using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : Singleton<PlayerMovement>
{
    [HideInInspector]
    public Rigidbody Rb;

    [HideInInspector]
    public bool CanMove = true;

    [SerializeField]
    private float speed = 5f;

    private void Awake()
    {
        this.Rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (this.CanMove)
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
        }
    }
}
