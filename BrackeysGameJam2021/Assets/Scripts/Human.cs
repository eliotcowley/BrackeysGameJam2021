﻿using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Human : MonoBehaviour
{
    public GameObject ButtonIcon;

    [HideInInspector]
    public bool InAttackRange = false;

    [HideInInspector]
    public float Health;

    [HideInInspector]
    public bool RunningAway = false;

    [HideInInspector]
    public GameObject Target;

    [SerializeField]
    private float maxHealth = 100f;

    [SerializeField]
    private GameObject dieFXPrefab;

    [SerializeField]
    private Transform canvasTransform;

    [SerializeField]
    private Image healthBar;

    [SerializeField]
    private Color targetAttackingColor;

    [SerializeField]
    private float distanceToRunAway;

    [SerializeField]
    private float stationaryTimer = 1f;

    [SerializeField]
    private float newDestinationTimer = 5f;

    [SerializeField]
    private GameObject gerbilPrefab;

    [SerializeField]
    private GameObject key;

    [SerializeField]
    private bool dropsKey = false;
    
    private Material targetMaterial;
    private NavMeshAgent agent;
    private Rigidbody rb;
    private Vector3 lastVelocity = Vector3.zero;
    private float timer = 0f;
    private bool hasSetNewDestination = false;
    private Animator animator;
    private CinemachineImpulseSource cinemachineImpulseSource;

    private void Start()
    {
        this.Target = this.transform.GetChild(0).gameObject;
        this.Health = this.maxHealth;
        this.targetMaterial = this.Target.GetComponent<Renderer>().material;
        this.agent = GetComponent<NavMeshAgent>();
        this.rb = GetComponent<Rigidbody>();
        this.animator = GetComponent<Animator>();
        this.cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();

        if (this.dropsKey)
        {
            this.key.SetActive(false);
        }

        if (GameManager.Instance.Level == 10)
        {
            this.distanceToRunAway = 100f;
            this.RunningAway = true;
            SetRunningAnimation();
        }
    }

    private void Update()
    {
        //if (GameManager.Instance.Level == 10)
        //{
        //    Vector3 average = GameManager.Instance.GetAverageGerbilPosition();
        //    float distance = Vector3.Distance(this.rb.position, average);

        //    if (distance <= 20f)
        //    {
        //        this.RunningAway = true;
        //        SetRunningAnimation();
        //    }
        //}

        if (this.RunningAway)
        {
            Vector3 average = GameManager.Instance.GetAverageGerbilPosition();
            Vector3 direction = this.rb.position - average;
            float distance = Vector3.Distance(this.rb.position, average);

            this.timer += Time.deltaTime;

            if (this.timer >= this.stationaryTimer)
            {
                if (this.agent.velocity == Vector3.zero && this.lastVelocity == Vector3.zero)
                {
                    this.agent.SetDestination(this.rb.position - direction);
                    this.hasSetNewDestination = true;
                    StartCoroutine(BackToSimpleRunAway());
                }

                this.timer = 0f;
                this.lastVelocity = this.agent.velocity;
            }

            if (distance < this.distanceToRunAway)
            {
                if (!this.hasSetNewDestination)
                {
                    this.agent.SetDestination(this.rb.position + direction);
                }
            }
            else
            {
                StopRunning();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.Tag_GerbilMain) && !GerbilAttack.Instance.IsAttacking && GameManager.Instance.Level != 10)
        {
            if (GerbilAttack.Instance.TargetHuman != null)
            {
                GerbilAttack.Instance.TargetHuman.Target.SetActive(false);
                GerbilAttack.Instance.TargetHuman.ButtonIcon.SetActive(false);
            }

            this.Target.SetActive(true);
            this.ButtonIcon.SetActive(true);
            GerbilAttack.Instance.TargetHuman = this;
            this.InAttackRange = true;
        }
        else if (other.CompareTag("GerbilBEEG"))
        {
            other.transform.parent.Find("laugh").GetComponent<AudioSource>().Play();
            TakeDamage(100f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (GameManager.Instance.Level == 10)
        {
            return;
        }

        if (other.gameObject.CompareTag(Constants.Tag_GerbilMain))
        {
            this.ButtonIcon.SetActive(false);
            this.InAttackRange = false;

            if (!GerbilAttack.Instance.IsAttacking)
            {
                this.Target.SetActive(false);
                GerbilAttack.Instance.TargetHuman = null;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        this.Health -= damage;
        this.healthBar.fillAmount = this.Health / this.maxHealth;

        if (this.Health <= 0f)
        {
            Instantiate(this.dieFXPrefab, this.transform.position, Quaternion.identity);
            GerbilAttack.Instance.StopAttacking();
            GerbilAttack.Instance.TargetHuman = null;
            this.cinemachineImpulseSource.GenerateImpulse();

            if (this.dropsKey)
            {
                this.key.transform.position = new Vector3(this.transform.position.x, this.key.transform.position.y, this.transform.position.z);
                this.key.SetActive(true);
            }
            else
            {
                GameObject gerbil = Instantiate(this.gerbilPrefab, this.transform.position, Quaternion.identity);
                gerbil.GetComponent<Rigidbody>().velocity = Vector3.zero;
                gerbil.GetComponent<GerbilFollower>().TryAddToSwarm();
            }

            GameManager.Instance.PlayHumanDieSFX();
            Destroy(this.gameObject);
        }
    }

    public void SetTargetAttackColor(bool attack)
    {
        this.targetMaterial.color = attack ? this.targetAttackingColor : Color.white;
    }

    private IEnumerator BackToSimpleRunAway()
    {
        yield return new WaitForSeconds(this.newDestinationTimer);
        this.hasSetNewDestination = false;
    }

    public void SetRunningAnimation()
    {
        this.animator.SetBool(Constants.Anim_Run, true);
    }

    private void StopRunning()
    {
        //if (GameManager.Instance.Level == 10)
        //{
        //    return;
        //}

        this.animator.SetBool(Constants.Anim_Run, false);
        this.RunningAway = false;
        this.agent.SetDestination(this.rb.position);
    }
}
