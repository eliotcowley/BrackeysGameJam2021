using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField]
    private GameObject gerbilDieFXPrefab;

    [SerializeField]
    private float gerbilDieFXDestroyDelay = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.Tag_GerbilFollower))
        {
            GameObject gerbilDieFX = Instantiate(this.gerbilDieFXPrefab, other.attachedRigidbody.position, Quaternion.identity);
            Destroy(other.gameObject);
            GerbilMain.Instance.SwarmCount--;
            Destroy(gerbilDieFX, this.gerbilDieFXDestroyDelay);
        }
    }
}
