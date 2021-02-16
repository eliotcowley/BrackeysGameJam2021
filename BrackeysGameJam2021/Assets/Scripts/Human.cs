using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Human : MonoBehaviour
{
    public GameObject ButtonIcon;

    [SerializeField]
    private float maxHealth = 100f;

    [SerializeField]
    private GameObject dieFXPrefab;

    [SerializeField]
    private Transform canvasTransform;

    [SerializeField]
    private Image healthBar;

    private GameObject target;
    private float health;

    private void Start()
    {
        this.target = this.transform.GetChild(0).gameObject;
        this.health = this.maxHealth;
    }

    private void Update()
    {
        this.canvasTransform.LookAt(this.canvasTransform.position + Camera.main.transform.forward);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.Tag_GerbilMain))
        {
            this.target.SetActive(true);
            this.ButtonIcon.SetActive(true);
            GerbilAttack.Instance.TargetHuman = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.Tag_GerbilMain))
        {
            this.target.SetActive(false);
            this.ButtonIcon.SetActive(false);
            GerbilAttack.Instance.StopAttacking();
            GerbilAttack.Instance.TargetHuman = null;
        }
    }

    public void TakeDamage(float damage)
    {
        this.health -= damage;
        this.healthBar.fillAmount = this.health / this.maxHealth;

        if (this.health <= 0f)
        {
            Instantiate(this.dieFXPrefab, this.transform.position, Quaternion.identity);
            GerbilAttack.Instance.StopAttacking();
            Destroy(this.gameObject);
        }
    }
}
