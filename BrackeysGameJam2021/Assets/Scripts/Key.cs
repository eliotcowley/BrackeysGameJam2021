using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 50f;

    [SerializeField]
    private Door door;

    private void Update()
    {
        Vector3 newRot = new Vector3(
            0f, 
            this.rotationSpeed * Time.deltaTime, 
            0f);

        this.transform.Rotate(newRot);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.Tag_GerbilFollower))
        {
            this.door.Open();
            Destroy(this.gameObject);
        }
    }
}
