using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Human : MonoBehaviour
{
    public GameObject ButtonIcon;

    [HideInInspector]
    public bool InAttackRange = false;

    [HideInInspector]
    public float Health;

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

    private GameObject target;
    private Material targetMaterial;

    private void Start()
    {
        this.target = this.transform.GetChild(0).gameObject;
        this.Health = this.maxHealth;
        this.targetMaterial = this.target.GetComponent<Renderer>().material;
    }

    private void Update()
    {
        this.canvasTransform.LookAt(this.canvasTransform.position + Camera.main.transform.forward);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.Tag_GerbilMain) && !GerbilAttack.Instance.IsAttacking)
        {
            this.target.SetActive(true);
            this.ButtonIcon.SetActive(true);
            GerbilAttack.Instance.TargetHuman = this;
            this.InAttackRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.Tag_GerbilMain) && !GerbilAttack.Instance.IsAttacking)
        {
            this.target.SetActive(false);
            this.ButtonIcon.SetActive(false);
            GerbilAttack.Instance.TargetHuman = null;
            this.InAttackRange = false;
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
            Destroy(this.gameObject);
        }
    }

    public void SetTargetAttackColor(bool attack)
    {
        this.targetMaterial.color = attack ? this.targetAttackingColor : Color.white;
    }
}
