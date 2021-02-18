using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField]
    private int gerbilsRequired = 0;

    [SerializeField]
    private GameObject buttonGO;

    [SerializeField]
    private float onYPosition = -0.02f;

    [SerializeField]
    private Animator doorAnimator;

    private int gerbilsOnSwitch = 0;
    public int GerbilsOnSwitch
    {
        get
        {
            return this.gerbilsOnSwitch;
        }
        set
        {
            this.gerbilsOnSwitch = value;
            UpdateCounterText();
        }
    }

    private TextMeshProUGUI counterText;
    private bool on = false;

    private void Start()
    {
        this.counterText = GetComponentInChildren<TextMeshProUGUI>();
        UpdateCounterText();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!this.on && other.CompareTag(Constants.Tag_GerbilFollower))
        {
            this.GerbilsOnSwitch++;

            if (this.GerbilsOnSwitch >= this.gerbilsRequired)
            {
                TurnOn();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!this.on && other.CompareTag(Constants.Tag_GerbilFollower))
        {
            this.GerbilsOnSwitch--;
        }
    }

    private void TurnOn()
    {
        this.on = true;
        this.counterText.gameObject.SetActive(false);

        this.buttonGO.transform.localPosition = new Vector3(
            this.buttonGO.transform.localPosition.x, 
            this.onYPosition, 
            this.buttonGO.transform.localPosition.z);

        this.doorAnimator.SetTrigger(Constants.Anim_DoorOpen);
    }

    private void UpdateCounterText()
    {
        this.counterText.SetText($"{this.gerbilsOnSwitch} / {this.gerbilsRequired}");
    }
}
