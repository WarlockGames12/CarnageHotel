using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler
{
    [Header("Button Hover Audio:")] 
    [SerializeField] private AudioSource buttonHover;

    public void OnPointerEnter(PointerEventData ped)
    {
        buttonHover.Play();
    }
}

