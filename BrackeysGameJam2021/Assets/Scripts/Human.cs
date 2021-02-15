using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
    private GameObject target;

    private void Start()
    {
        this.target = this.transform.GetChild(0).gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.Tag_GerbilMain))
        {
            this.target.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.Tag_GerbilMain))
        {
            this.target.SetActive(false);
        }
    }
}
