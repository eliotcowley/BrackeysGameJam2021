using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class GerbilFollower : MonoBehaviour
{
    [HideInInspector]
    public bool InSwarm = false;

    [HideInInspector]
    public bool Attacking = false;

    [SerializeField]
    private float speed = 0.5f;

    [SerializeField]
    [Range(0f, 1f)]
    private float turnSpeed = 0.5f;

    [SerializeField]
    private float maxDistanceFromCenter = 2f;

    [SerializeField]
    private GameObject gerbilDieFXPrefab;

    [SerializeField]
    private float attackInterval = 0.5f;

    [SerializeField]
    private float heightAbovePlayerWhenDigging = 1f;

    [SerializeField]
    private float idleSpeed = 0.1f;

    [SerializeField]
    private bool BEEG = false;

    private Rigidbody rb;
    private float attackTimer = 0f;
    private CinemachineImpulseSource cinemachineImpulseSource;
    private Animator animator;

    private void Start()
    {
        this.rb = GetComponent<Rigidbody>();
        this.cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
        this.animator = GetComponentInChildren<Animator>();
        this.animator.SetBool(Constants.Anim_IsWalking, false);
        this.animator.speed = 0f;
        this.rb.velocity = Vector3.zero;
        
        // Set random values for the fur shader
        MeshRenderer gerbelMesh = transform.Find("gerbel").GetComponent<MeshRenderer>();
        gerbelMesh.material.SetColor("Primary", Random.ColorHSV());
        gerbelMesh.material.SetColor("Secondary", Random.ColorHSV());
        gerbelMesh.material.SetFloat("Rotation", Random.Range(-10, 10));
    }

    private void FixedUpdate()
    {
        if (this.InSwarm && Vector3.Distance(this.rb.position, PlayerMovement.Instance.Rb.position) > this.maxDistanceFromCenter && !this.Attacking)
        {
            Vector3 direction = (PlayerMovement.Instance.Rb.position - this.rb.position) * this.speed * Time.fixedDeltaTime;
            this.rb.velocity = new Vector3(direction.x, this.rb.velocity.y, direction.z);

            Quaternion lookRot = Quaternion.LookRotation(direction, Vector3.up);
            Quaternion smoothRot = Quaternion.Lerp(this.rb.rotation, lookRot, this.turnSpeed);
            this.rb.MoveRotation(smoothRot);
        }

        if (this.Attacking)
        {
            Vector3 direction = 
                (GerbilAttack.Instance.TargetHuman.transform.position - this.rb.position) 
                * GerbilAttack.Instance.AttackSpeed 
                * Time.fixedDeltaTime;

            this.rb.velocity = direction;

            Quaternion lookRot = Quaternion.LookRotation(direction, Vector3.up);
            Quaternion smoothRot = Quaternion.Lerp(this.rb.rotation, lookRot, this.turnSpeed);
            this.rb.MoveRotation(smoothRot);

            this.attackTimer += Time.fixedDeltaTime;

            if (this.attackTimer >= this.attackInterval)
            {
                GerbilAttack.Instance.TargetHuman.TakeDamage(GerbilAttack.Instance.DamagePerGerbil);
                this.attackTimer = 0f;
            }
        }

        if (this.rb.position.y < GameManager.Instance.DeathY)
        {
            Die();
        }

        if (!this.BEEG)
        {
            if (this.animator.GetBool(Constants.Anim_IsWalking))
            {
                if (this.rb.velocity.sqrMagnitude <= this.idleSpeed)
                {
                    this.animator.SetBool(Constants.Anim_IsWalking, false);
                    this.animator.speed = 0f;
                }
            }
            else
            {
                if (this.rb.velocity.sqrMagnitude >= this.idleSpeed)
                {
                    this.animator.SetBool(Constants.Anim_IsWalking, true);
                    this.animator.speed = 1f;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.Tag_GerbilMain))
        {
            TryAddToSwarm();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Constants.Tag_BowlingBall))
        {
            Die();
        }
        else if (collision.gameObject.CompareTag(Constants.Tag_GerbilFollower))
        {
            GerbilFollower gerbil = collision.gameObject.GetComponentInParent<GerbilFollower>();

            if (!gerbil.InSwarm)
            {
                gerbil.TryAddToSwarm();
            }
        }
    }

    public void Die()
    {
        Instantiate(this.gerbilDieFXPrefab, this.rb.position, Quaternion.identity);
        GameManager.Instance.GerbilsInSwarm.Remove(this);
        UIManager.Instance.UpdateSwarmCountText();

        if (GameManager.Instance.GerbilsInSwarm.Count <= 0)
        {
            GameManager.Instance.GameOver();
        }

        this.cinemachineImpulseSource.GenerateImpulse();
        Destroy(this.gameObject);
    }

    public void GoUnderground(bool underground)
    {
        if (!this.BEEG)
        {
            StartCoroutine(GoUndergroundCoroutine(underground));
        }
    }

    private IEnumerator GoUndergroundCoroutine(bool underground)
    {
        if (LevelBounds.Instance.IsInLevelBounds(this.rb.position))
        {
            this.rb.constraints = RigidbodyConstraints.None;
            Vector3 newPos = new Vector3(this.rb.position.x, PlayerMovement.Instance.Rb.position.y + this.heightAbovePlayerWhenDigging, this.rb.position.z);
            this.rb.MovePosition(newPos);
            this.rb.useGravity = !underground;
            yield return new WaitForFixedUpdate();

            if (underground)
            {
                this.rb.constraints = RigidbodyConstraints.FreezePositionY;
            }
        }
    }

    public void Fall()
    {
        this.rb.constraints = RigidbodyConstraints.None;
        this.rb.useGravity = true;
        this.InSwarm = false;
        GameManager.Instance.GerbilsInSwarm.Remove(this);
    }

    public void TryAddToSwarm()
    {
        if (!this.InSwarm && !GerbilAttack.Instance.IsAttacking && !PlayerMovement.Instance.IsUnderground)
        {
            this.InSwarm = true;
            GameManager.Instance.GerbilsInSwarm.Add(this);
            UIManager.Instance.UpdateSwarmCountText();
        }
    }
}
