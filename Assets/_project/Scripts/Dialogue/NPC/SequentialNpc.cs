using UnityEngine;
using UnityEngine.Events;

public class SequentialNpc : BaseNpc
{
    [SerializeField] private DialogueInteraction[] sequentialNpc;
    protected override void Update()
    {
        if (!CanStartDialogue())
            return;

        if (TimesInteracted == 0)
        {
            DialogueTrigger.TriggerDialogue(firstDialogue);

            print("first interaction?");

            regionEvent.Invoke();
            quest.Invoke();
        }
        else if (TimesInteracted <= sequentialNpc.Length)
        { 
            // Adjust the index to match the array starting at 0 and interaction starting at 1.
            int interactionIndex = TimesInteracted - 1;

            // Perform actions based on TimesInteracted and sequentialNpc
            DialogueTrigger.TriggerDialogue(sequentialNpc[interactionIndex]);
            print("first interaction?");
        }
        else if (TimesInteracted > sequentialNpc.Length)
        {
            print("second interaction?");

            // Trigger the dialogue associated with the last item in the array
            int lastItemIndex = sequentialNpc.Length - 1;
            DialogueTrigger.TriggerDialogue(sequentialNpc[lastItemIndex]);
        }

        TimesInteracted++; 
    }
}
