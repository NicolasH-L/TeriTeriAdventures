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
    private const string DialogueBoxTag = "DialogueBox";
    private const string FuHuaTag = "FuHua";
    private const string TransitionTag = "Transition";
    private TextMeshProUGUI _dialogueBox;
    private Image _fuHua;
    private Image _transition;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    private void Start()
    {
        
        // _dialogueBox = GameObject.FindGameObjectWithTag(DialogueBoxTag).GetComponent<TextMeshProUGUI>();
        _fuHua = GameObject.FindGameObjectWithTag(FuHuaTag).GetComponent<Image>();
        Debug.Log(GameObject.FindGameObjectWithTag(FuHuaTag));
        if (GameObject.FindGameObjectWithTag(TransitionTag) != null)
            _transition = GameObject.FindGameObjectWithTag(TransitionTag).GetComponent<Image>();
        _playerUI = GameObject.FindGameObjectWithTag(PlayerUiTag).GetComponent<Canvas>();
        _nameText = GameObject.FindGameObjectWithTag(NameTag).GetComponent<TextMeshProUGUI>();
        _dialogueText = GameObject.FindGameObjectWithTag(DialogTag).GetComponent<TextMeshProUGUI>();
        _canvasDialogue = GameObject.FindGameObjectWithTag(CanvasDialogTag).GetComponent<Canvas>();
        // _dialogueBox.enabled = false;
        _fuHua.enabled = false;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        _playerUI.enabled = false;
        _nameText.text = dialogue.name;
        Debug.Log(_nameText);

        //Value exists but the object is undefined?
        Debug.Log(dialogue.name);

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
            sentences.Enqueue(sentence);

        DisplayNextSentence();
    }

    public void DisplayGame()
    {
        if (_transition != null)
            Destroy(_transition);
        Destroy(gameObject);
        _dialogueBox.enabled = true;
        // _fuHua.SetActive(true);
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
        _playerUI.enabled = true;
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