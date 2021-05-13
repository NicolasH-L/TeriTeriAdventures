using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private const string PlayerUiTag = "PlayerUI";
    private const string CanvasDialogTag = "CanvasDialogue";
    private const string NameTag = "NpcName";
    private const string DialogTag = "Dialogue";
    private TextMeshProUGUI _nameText;
    private TextMeshProUGUI _dialogueText;
    private Canvas _canvasDialogue;
    private Canvas _playerUI;
    private Queue<string> sentences = new Queue<string>();

    // Start is called before the first frame update
    private void Start()
    {
        // playerUI = GameObject.FindGameObjectWithTag(PlayerUiTag).GetComponent<Canvas>();
        _nameText = GameObject.FindGameObjectWithTag(NameTag).GetComponent<TextMeshProUGUI>();
        _dialogueText = GameObject.FindGameObjectWithTag(DialogTag).GetComponent<TextMeshProUGUI>();
        _canvasDialogue = GameObject.FindGameObjectWithTag(CanvasDialogTag).GetComponent<Canvas>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        // playerUI.enabled = false;
        _nameText.text = dialogue.name;
        Debug.Log(_nameText);

        //Value exists but the object is undefined?
        Debug.Log(dialogue.name);

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
            sentences.Enqueue(sentence);

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    private void EndDialogue()
    {
        _canvasDialogue.enabled = false;
        // playerUI.enabled = true;
    }

    private IEnumerator TypeSentence(string sentence)
    {
        _dialogueText.text = "";
        foreach (char letter in sentence)
        {
            _dialogueText.text += letter;
            yield return null;
        }
    }
}