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
    
    private Material targetMaterial;
    private NavMeshAgent agent;
    private Rigidbody rb;

    private void Start()
    {
        this.Target = this.transform.GetChild(0).gameObject;
        this.Health = this.maxHealth;
        this.targetMaterial = this.Target.GetComponent<Renderer>().material;
        this.agent = GetComponent<NavMeshAgent>();
        this.rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        this.canvasTransform.LookAt(this.canvasTransform.position + Camera.main.transform.forward);

        if (this.RunningAway)
        {
            // Take average position of gerbils (add up and divide by # gerbils), then run in opposite direction
            Vector3 total = GameManager.Instance.GerbilsInSwarm.Aggregate(
                Vector3.zero,
                (workingTotal, next) => workingTotal + next.transform.position,
                workingTotal => workingTotal);

            Vector3 average = total / GameManager.Instance.GerbilsInSwarm.Count;
            Vector3 direction = this.rb.position - average;
            float distance = Vector3.Distance(this.rb.position, average);

            if (distance < this.distanceToRunAway)
            {
                this.agent.SetDestination(this.rb.position + direction);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.Tag_GerbilMain) && !GerbilAttack.Instance.IsAttacking)
        {
            this.Target.SetActive(true);
            this.ButtonIcon.SetActive(true);
            GerbilAttack.Instance.TargetHuman = this;
            this.InAttackRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
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
            Destroy(this.gameObject);
        }
    }

    public void SetTargetAttackColor(bool attack)
    {
        this.targetMaterial.color = attack ? this.targetAttackingColor : Color.white;
    }
}
