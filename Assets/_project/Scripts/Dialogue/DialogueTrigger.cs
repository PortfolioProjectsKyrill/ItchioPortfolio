using UnityEngine;

public static class DialogueTrigger
{
    public static void TriggerDialogue(DialogueInteraction dialogue)
    {
        if (DialogueManager.Instance.CurrentCoroutine == null)
        {
            DialogueManager.Instance.StartDialogue(dialogue);
        }
    }
}