using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerbilMenu : MonoBehaviour
{
    private void Start()
    {
        // Set random values for the fur shader
        MeshRenderer gerbelMesh = transform.Find("gerbel").GetComponent<MeshRenderer>();
        gerbelMesh.material.SetColor("Primary", Random.ColorHSV());
        gerbelMesh.material.SetColor("Secondary", Random.ColorHSV());
        gerbelMesh.material.SetFloat("Rotation", Random.Range(-10, 10));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GerbilKiller"))
        {
            Destroy(this.gameObject);
        }
    }
}
