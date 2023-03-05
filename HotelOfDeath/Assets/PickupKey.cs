using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PickupKey : MonoBehaviour
{

    [SerializeField] private GameObject pressE;
    [SerializeField] private AudioSource[] getKey;
    public static bool willGetKey;
    public static bool hasKey;

    private void Update()
    {
        switch (hasKey)
        {
            case true:
                pressE.SetActive(false);
                var randomSound = Random.Range(0, getKey.Length);
                getKey[randomSound].Play();
                gameObject.SetActive(false);
                break;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            willGetKey = true;
            pressE.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        willGetKey = false;
        pressE.SetActive(false);
    }
}
