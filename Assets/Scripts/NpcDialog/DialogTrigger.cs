using UnityEngine;

namespace NpcDialog
{
    public class DialogTrigger : MonoBehaviour
    {
        public Dialog dialog;

        private void Start()
        {
            TriggerDialog();
        }

        private void TriggerDialog()
        {
            FindObjectOfType<DialogManager>().StartDialog(dialog);
        }
    }
}