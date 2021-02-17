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
    public Vector3 NewVelocity;

    [HideInInspector]
    public bool IsUnderground = false;

    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private float undergroundDepth = 5f;
    
    private bool flagForFixedUpdate = false;

    private void Awake()
    {
        this.Rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (this.CanMove)
        {
            if (Input.GetButtonDown(Constants.Input_Attack) && !this.flagForFixedUpdate && GerbilAttack.Instance.TargetHuman == null)
            {
                ToggleUnderground();
            }

            float horizontal = Input.GetAxisRaw(Constants.Input_Horizontal);
            float vertical = Input.GetAxisRaw(Constants.Input_Vertical);
            Vector3 direction = new Vector3(horizontal, 0, vertical);

            if (direction.magnitude > 1f)
            {
                direction.Normalize();
            }

            this.NewVelocity = direction * this.speed;
            //this.NewPos.x = newVelocity.x;
            //this.NewPos.z = newVelocity.z;

            //if (!this.flagForFixedUpdate)
            //{
            //    this.NewVelocity.y = this.Rb.position.y;
            //}
        }
    }

    private void FixedUpdate()
    {
        //Vector3 moveVector = new Vector3(
        //    this.NewVelocity.x * Time.fixedDeltaTime, 
        //    this.NewVelocity.y, 
        //    this.NewVelocity.z * Time.fixedDeltaTime);

        //this.Rb.MovePosition(moveVector);
        this.Rb.velocity = this.NewVelocity * Time.fixedDeltaTime;

        //if (this.flagForFixedUpdate)
        //{
        //    foreach (GerbilFollower gerbil in GameManager.Instance.GerbilsInSwarm)
        //    {
        //        gerbil.GoUnderground(this.IsUnderground);
        //    }
        //}

        this.flagForFixedUpdate = false;
    }

    public void ToggleUnderground()
    {
        StartCoroutine(ToggleUndergroundCoroutine());
    }

    private IEnumerator ToggleUndergroundCoroutine()
    {
        this.flagForFixedUpdate = true;
        this.IsUnderground = !this.IsUnderground;
        float newY = this.IsUnderground ? this.Rb.position.y - this.undergroundDepth : this.Rb.position.y + this.undergroundDepth;
        Vector3 newPos = new Vector3(this.Rb.position.x, newY, this.Rb.position.z);
        this.Rb.MovePosition(newPos);

        yield return new WaitForFixedUpdate();

        foreach (GerbilFollower gerbil in GameManager.Instance.GerbilsInSwarm)
        {
            gerbil.GoUnderground(this.IsUnderground);
        }
    }
}
