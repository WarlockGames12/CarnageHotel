using System;
using UnityEngine;

public class TurnOn : MonoBehaviour
{
    public static bool isInRange = false;
    [SerializeField] private GameObject PressE;
    [SerializeField] private GameObject[] SetOn;
    [SerializeField] private GameObject[] AllLights;
    [SerializeField] private GameObject EnemyGone;
    [SerializeField] private GameObject keyWillAppear;
    [SerializeField] private GameObject ClerkAppears;

    private void Update()
    {
        switch (PlayerMovement.pressEs)
        {
            case true:
            {
                SetOn[0].SetActive(false);
                SetOn[1].SetActive(true);
                EnemyGone.SetActive(false);
                ClerkAppears.SetActive(true);
                keyWillAppear.SetActive(true);
                foreach (var t in AllLights)
                {
                    t.SetActive(true);
                }
                break;
            }
            case false:
            {
                SetOn[0].SetActive(true);
                SetOn[1].SetActive(false);
                EnemyGone.SetActive(true);
                ClerkAppears.SetActive(false);
                keyWillAppear.SetActive(false);
                foreach (var t in AllLights)
                {
                    t.SetActive(false);
                }
                break;
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInRange = true;
            PressE.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isInRange = false;
        PressE.SetActive(false);
    }
}
