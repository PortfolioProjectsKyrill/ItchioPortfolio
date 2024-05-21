using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ReceiveInteraction : MonoBehaviour
{
    public bool hasInteraction;

    private BoxCollider _boxColl;

    private void Awake()
    {
        // Initialize the BoxCollider
        _boxColl = GetComponent<BoxCollider>();

        // Set BoxCollider properties
        _boxColl.isTrigger = true;
        _boxColl.center = new Vector3(0, 0, 0.75f);
        _boxColl.size = new Vector3(1.4f, 1.55f, 1);
    }

    private void OnTriggerStay(Collider other)
    {
        // Check if the collider belongs to the player
        PlayerStateMachine player = other.GetComponent<PlayerStateMachine>();
        if (player != null)
        {
            CheckForInteractionInput();
        }
    }

    private void CheckForInteractionInput()
    {
        // Check for interaction input using the GameManager
        hasInteraction = Input.GetKeyDown(GameManager.Instance.GetKey("Interact"));
    }

    public bool CheckForInteraction()
    {
        return hasInteraction;
    }
}