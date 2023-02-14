using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue Settings")] 
    [SerializeField] private DialogueData dialogue;
    [SerializeField] private Text dialogueText;
    [SerializeField] private AudioClip typeSound;

    public bool isActive = false;
    public bool _isTyping = false;
    
    private int _dialogueIndex = 0;
    private float _typingSpeed = 0.05f;
    private string _currentText = "";
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void StartDialogue()
    {
        _isTyping = true;
        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        if (_isTyping) yield break;
        _isTyping = true;
        foreach (var letter in dialogue.dialogues[_dialogueIndex].text.ToCharArray())
        {
            _currentText += letter;
            dialogueText.text = _currentText;
            _audioSource.PlayOneShot(typeSound);
            yield return new WaitForSeconds(_typingSpeed);
        }
        _isTyping = false;
    }

    public void AdvanceDialogue()
    {
        if (_dialogueIndex >= dialogue.dialogues.Length)
        {
            EndDialogue();
            return;
        }
    
        _dialogueIndex++;
        _currentText = "";
        _isTyping = false;
        StartCoroutine(TypeText());
    }

    private void EndDialogue()
    {
        _dialogueIndex = 0;
        dialogueText.text = "";
        _isTyping = false;
        isActive = false;
    }
}
