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

    [HideInInspector]
    public Vector3 NewPos;

    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private float undergroundDepth = 5f;

    private bool isUnderground = false;
    private bool flagForFixedUpdate = false;

    private void Awake()
    {
        this.Rb = GetComponent<Rigidbody>();
        this.NewPos = this.Rb.position;
    }

    private void Update()
    {
        if (this.CanMove)
        {
            if (Input.GetButtonDown(Constants.Input_Attack) && !this.flagForFixedUpdate && GerbilAttack.Instance.TargetHuman == null)
            {
                this.flagForFixedUpdate = true;
                this.isUnderground = !this.isUnderground;
                this.NewPos.y = this.isUnderground ? this.Rb.position.y - this.undergroundDepth : this.Rb.position.y + this.undergroundDepth;
            }

            float horizontal = Input.GetAxisRaw(Constants.Input_Horizontal);
            float vertical = Input.GetAxisRaw(Constants.Input_Vertical);
            Vector3 direction = new Vector3(horizontal, 0, vertical);

            if (direction.magnitude > 1f)
            {
                direction.Normalize();
            }

            Vector3 newPos = this.Rb.position + direction * this.speed * Time.deltaTime;
            this.NewPos.x = newPos.x;
            this.NewPos.z = newPos.z;

            if (!this.flagForFixedUpdate)
            {
                this.NewPos.y = this.Rb.position.y;
            }
        }
    }

    private void FixedUpdate()
    {
        this.Rb.MovePosition(this.NewPos);

        if (this.flagForFixedUpdate)
        {
            foreach (GerbilFollower gerbil in GameManager.Instance.GerbilsInSwarm)
            {
                gerbil.GoUnderground(this.isUnderground);
            }
        }

        this.flagForFixedUpdate = false;
    }
}
