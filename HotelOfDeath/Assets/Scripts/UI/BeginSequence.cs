using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginSequence : MonoBehaviour
{

    [SerializeField] private DialogueManager _dialogue;

    // Start is called before the first frame update
    private void Update()
    {
        _dialogue.isActive = true;
    }
}
