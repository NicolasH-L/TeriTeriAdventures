﻿using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public Dialog dialog;

    private void Start()
    {
        TriggerDialog();
    }

    public void TriggerDialog()
    {
        FindObjectOfType<DialogManager>().StartDialog(dialog);
    }
}