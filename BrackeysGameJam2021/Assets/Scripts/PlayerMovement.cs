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

    [SerializeField]
    private Renderer groundRenderer;

    [SerializeField]
    private float groundAlphaWhenUnderground = 0.5f;

    [SerializeField]
    private float digDelay = 0.5f;

    [SerializeField]
    private float minDistanceToJump = 1f;

    private bool flagForFixedUpdate = false;
    private Color groundMaterialColor;
    private float digTimer = 0f;

    private void Awake()
    {
        this.Rb = GetComponent<Rigidbody>();
        this.groundMaterialColor = this.groundRenderer.material.color;
    }

    private void Update()
    {
        if (this.digTimer < this.digDelay)
        {
            this.digTimer += Time.deltaTime;
        }

        if (this.CanMove)
        {
            if (Input.GetButtonDown(Constants.Input_Attack) 
                && !this.flagForFixedUpdate 
                && GerbilAttack.Instance.TargetHuman == null
                && this.digTimer >= this.digDelay
                && LevelBounds.Instance.IsInLevelBounds(this.Rb.position))
            {
                ToggleUnderground();
            }

            if (Input.GetButtonDown(Constants.Input_Jump))
            {
                Vector3 average = GameManager.Instance.GetAverageGerbilPosition();

                if (Vector3.Distance(this.Rb.position, average) > this.minDistanceToJump)
                {
                    this.Rb.MovePosition(average);
                }
            }

            float horizontal = Input.GetAxisRaw(Constants.Input_Horizontal);
            float vertical = Input.GetAxisRaw(Constants.Input_Vertical);
            Vector3 direction = new Vector3(horizontal, 0, vertical);

            if (direction.magnitude > 1f)
            {
                direction.Normalize();
            }

            this.NewVelocity = direction * this.speed;
        }
    }

    private void FixedUpdate()
    {
        this.Rb.velocity = this.NewVelocity * Time.fixedDeltaTime;
        this.flagForFixedUpdate = false;
    }

    public void ToggleUnderground()
    {
        StartCoroutine(ToggleUndergroundCoroutine());
    }

    private IEnumerator ToggleUndergroundCoroutine()
    {
        this.digTimer = 0f;
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

        float newAlpha = this.IsUnderground ? this.groundAlphaWhenUnderground : 1f;

        this.groundRenderer.material.color = new Color(
            this.groundMaterialColor.r, 
            this.groundMaterialColor.g, 
            this.groundMaterialColor.b, 
            newAlpha);
    }
}
