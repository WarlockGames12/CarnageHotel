using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIsClose : MonoBehaviour
{
    [Header("UI Settings: ")]
    [SerializeField] private GameObject PressButton;
    public static bool isInRange;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PressButton.SetActive(true);
            isInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PressButton.SetActive(false);
        isInRange = false;
    }
}
