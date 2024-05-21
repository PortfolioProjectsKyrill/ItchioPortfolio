using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class BaseNpc : MonoBehaviour
{
    protected int TimesInteracted;
    protected bool HasInteracted;
    public DialogueInteraction firstDialogue;
    private bool _foundPlayer;
    public UnityEvent regionEvent;

    public UnityEvent quest;
    protected virtual void Update()
    {
        if (!CanStartDialogue())
            return;
        
        DialogueTrigger.TriggerDialogue(firstDialogue);
        if (TimesInteracted < 1)
        {
            quest.Invoke();
            regionEvent.Invoke();
        }
        TimesInteracted++;
        HasInteracted = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering GameObject has the PlayerStateMachine component
        PlayerStateMachine playerStateMachine = other.gameObject.GetComponent<PlayerStateMachine>();
        if (playerStateMachine != null)
        {
            _foundPlayer = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        // Reset _foundPlayer when the player leaves the trigger zone
        PlayerStateMachine playerStateMachine = other.gameObject.GetComponent<PlayerStateMachine>();
        if (playerStateMachine != null)
        {
            _foundPlayer = false;
        }
    }

    protected bool CanStartDialogue()
    {
        return _foundPlayer && Input.GetKeyDown(GameManager.Instance.GetKey("Interact"));
    }
}
