using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public Vacuum vacuum;
    public Axe axe;

    GameObject activeItem;

    private void Awake()
    {
        axe.gameObject.SetActive(false);
        vacuum.gameObject.SetActive(true);

        axe.owner = vacuum.owner = Camera.main.transform;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            // activate axe
            axe.gameObject.SetActive(true);
            vacuum.gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            // activate va
            axe.gameObject.SetActive(false);
            vacuum.gameObject.SetActive(true);
        }
    }
}
