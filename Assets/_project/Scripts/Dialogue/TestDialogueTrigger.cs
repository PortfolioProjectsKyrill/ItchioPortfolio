using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDialogueTrigger : MonoBehaviour
{
    [Header("references")] public DialogueInteraction dialogue;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            DialogueTrigger.TriggerDialogue(dialogue);
        }
    }
}
