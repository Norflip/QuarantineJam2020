using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemController : MonoBehaviour
{
    public Vacuum vacuum;
    public Axe axe;

    public TextMeshProUGUI axeInstruction;
    public TextMeshProUGUI vacuumInstruction;


    bool hasInstructions => axeInstruction != null && vacuumInstruction != null;
    GameObject activeItem;

    private void Awake()
    {
        axe.gameObject.SetActive(false);
        vacuum.gameObject.SetActive(true);
        if (hasInstructions)
        {
            axeInstruction.gameObject.SetActive(false);
            vacuumInstruction.gameObject.SetActive(true);
        }
        axe.owner = vacuum.owner = Camera.main.transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Alpha1))
        {
            axe.enabled = true;
            vacuum.enabled = false;
            // activate axe
            axe.gameObject.SetActive(true);
            vacuum.gameObject.SetActive(false);

            if(hasInstructions)
            {
                axeInstruction.gameObject.SetActive(true);
                vacuumInstruction.gameObject.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            axe.enabled = false;
            vacuum.enabled = true;
            // activate va
            axe.gameObject.SetActive(false);
            vacuum.gameObject.SetActive(true);

            if (hasInstructions)
            {
                axeInstruction.gameObject.SetActive(false);
                vacuumInstruction.gameObject.SetActive(true);
            }

        }
    }

}
