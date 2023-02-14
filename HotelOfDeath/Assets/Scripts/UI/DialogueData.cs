using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class DialogueData : ScriptableObject
{
    [System.Serializable]
    public struct DialogueLine
    {
        public string characterName;
        [TextArea(3, 10)] 
        public string text;
    }
    
    public DialogueLine[] dialogues;
}
