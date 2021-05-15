using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private const string PlayerUiTag = "PlayerUI";
    private const string CanvasDialogTag = "CanvasDialogue";
    private const string NameTag = "NpcName";
    private const string DialogTag = "Dialogue";
    [SerializeField] private Animator _dialogueBox;
    [SerializeField] private Animator _npc;
    private TextMeshProUGUI _nameText;
    private TextMeshProUGUI _dialogueText;
    private Canvas _canvasDialogue;
    private Canvas _playerUI;
    private Queue<string> _sentences;

    private void Start()
    {
        _sentences = new Queue<string>();
        _playerUI = GameObject.FindGameObjectWithTag(PlayerUiTag).GetComponent<Canvas>();
        _nameText = GameObject.FindGameObjectWithTag(NameTag).GetComponent<TextMeshProUGUI>();
        _dialogueText = GameObject.FindGameObjectWithTag(DialogTag).GetComponent<TextMeshProUGUI>();
        _canvasDialogue = GameObject.FindGameObjectWithTag(CanvasDialogTag).GetComponent<Canvas>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        _dialogueBox.SetBool("IsOpen", true);
        _npc.SetBool("IsEnter", true);
        _playerUI.enabled = false;
        _nameText.text = dialogue.name;
        _sentences.Clear();

        foreach (var sentence in dialogue.sentences)
            _sentences.Enqueue(sentence);

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (_sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        var sentence = _sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    private void EndDialogue()
    {
        _dialogueBox.SetBool("IsOpen", false);
        _npc.SetBool("IsEnter", false);
        _canvasDialogue.enabled = false;
        _playerUI.enabled = true;
    }

    private IEnumerator TypeSentence(string sentence)
    {
        _dialogueText.text = "";
        foreach (var letter in sentence)
        {
            _dialogueText.text += letter.ToString();
            yield return null;
        }
    }
}