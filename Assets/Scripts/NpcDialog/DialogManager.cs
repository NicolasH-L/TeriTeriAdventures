using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace NpcDialog
{
    public class DialogManager : MonoBehaviour
    {
        private const string PlayerUiTag = "PlayerUI";
        private const string CanvasDialogTag = "CanvasDialog";
        private const string NameTag = "NpcName";
        private const string DialogTag = "Dialog";
        [SerializeField] private Animator dialogBox;
        [SerializeField] private Animator npc;
        private TextMeshProUGUI _nameText;
        private TextMeshProUGUI _dialogText;
        private Canvas _canvasDialog;
        private Canvas _playerUI;
        private Queue<string> _sentences;

        //Suggestions made by Rider.
        private static readonly int IsOpen = Animator.StringToHash("IsOpen");
        private static readonly int IsEnter = Animator.StringToHash("IsEnter");

        private void Start()
        {
            _sentences = new Queue<string>();
            // Enable Line for Testing only.
            // if (GameObject.FindGameObjectWithTag(PlayerUiTag) != null)
            _playerUI = GameObject.FindGameObjectWithTag(PlayerUiTag).GetComponent<Canvas>();
            _nameText = GameObject.FindGameObjectWithTag(NameTag).GetComponent<TextMeshProUGUI>();
            _dialogText = GameObject.FindGameObjectWithTag(DialogTag).GetComponent<TextMeshProUGUI>();
            _canvasDialog = GameObject.FindGameObjectWithTag(CanvasDialogTag).GetComponent<Canvas>();
        }

        public void StartDialog(Dialog dialog)
        {
            dialogBox.SetBool(IsOpen, true);
            npc.SetBool(IsEnter, true);
            // Enable Line for Testing only.
            // if (_playerUI != null)
            _playerUI.enabled = false;
            _nameText.text = dialog.name;
            _sentences.Clear();

            foreach (var sentence in dialog.sentences)
                _sentences.Enqueue(sentence);

            DisplayNextSentence();
        }

        public void DisplayNextSentence()
        {
            if (_sentences.Count == 0)
            {
                EndDialog();
                return;
            }

            var sentence = _sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
        }

        private void EndDialog()
        {
            dialogBox.SetBool(IsOpen, false);
            npc.SetBool(IsEnter, false);
            _canvasDialog.enabled = false;
            // Enable Line for Testing only.
            // if (_playerUI != null)
            _playerUI.enabled = true;
        }

        private IEnumerator TypeSentence(string sentence)
        {
            _dialogText.text = "";
            foreach (var letter in sentence)
            {
                _dialogText.text += letter.ToString();
                yield return null;
            }
        }
    }
}