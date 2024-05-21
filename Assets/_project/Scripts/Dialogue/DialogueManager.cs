using System;
using System.Collections;
using System.ComponentModel.Design;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

// Manages the display of dialogues in the game
public class DialogueManager : MonoBehaviour
{
    #region instance
    // Singleton instance for the DialogueManager
    private static DialogueManager _instance;

    public static DialogueManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DialogueManager>();

                if (_instance == null)
                {
                    // Create a new GameObject with DialogueManager if not found in the scene
                    GameObject obj = new GameObject("DialogueManager");
                    _instance = obj.AddComponent<DialogueManager>();
                }
            }
            return _instance;
        }
    }
    #endregion
    
    [Header("References")]
    private PlayerStateMachine _playerStateMachine; // Reference to the PlayerStateMachine
    public GameObject dialogueBoxNewCanvasPrefab; // Prefab for the firstDialogue box
    public Coroutine CurrentCoroutine; // Current coroutine for handling dialogues

    //private GameObject dialogueBoxParent;

    private void Awake()
    {
        // Set up the Singleton pattern
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        _playerStateMachine = FindObjectOfType<PlayerStateMachine>();
    }

    // Method to start a firstDialogue based on the DialogueInteraction provided
    public void StartDialogue(DialogueInteraction dialogue)
    {
        DialogueUtilities.InDialogue = true;
        if (!dialogue.canPlayerWalk)
        {
            _playerStateMachine.DisablePlayerMovement(false);
        }

        switch (dialogue.dialogueType)
        {
            case DialogueType.CloseUp:
                CloseUpDialogue(dialogue);
                break;

            case DialogueType.PopUp:
                CurrentCoroutine ??= StartCoroutine(PopUpDialogue(dialogue));
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    // Method to handle CloseUp firstDialogue type
    private void CloseUpDialogue(DialogueInteraction dialogue)
    {
        //TODO: work on this once you got time.
    }

    // Coroutine to handle PopUp firstDialogue type
    private IEnumerator PopUpDialogue(DialogueInteraction dialogue)
    {
        GameObject popupUIElement = DialogueUtilities.FindUIElement(dialogue.dialogueType);

        if (popupUIElement == null)
            yield break;

        // Activate the PopUp UI element
        popupUIElement.SetActive(true);

        TextMeshProUGUI dialogueText = DialogueUtilities.FindTextMeshProUGUI(dialogue.dialogueType);

        if (dialogueText == null)
        {
            // Log an error if TextMeshProUGUI is not found
            Debug.LogError("TextMeshProUGUI not found for firstDialogue type: " + dialogue.dialogueType);
            popupUIElement.SetActive(false);
            yield break;
        }


        // Iterate through each firstDialogue line and display using TypewriterEffect
        for (int i = 0; i < dialogue.dialogueLines.Count; i++)
        {
            DialogueUtilities.ChangeDialogueSprite(dialogue.dialogueLines[i]);
            DialogueUtilities.CheckDialogueText(dialogue.dialogueLines[i]);

            yield return StartCoroutine(TypewriterEffect.Instance.TypeText(dialogue.dialogueLines[i], dialogueText));

            if (dialogue.autoSkipText)
            {
                yield return null;
            }
            else
            {
                yield return new WaitUntil(() => Input.anyKeyDown && GameManager.Instance.IsLastPressedKeyAllowed());
            }
            
        }

        // Deactivate the PopUp UI element after the firstDialogue is complete
        if (!dialogue.canPlayerWalk)
        {
            _playerStateMachine.EnablePlayerMovement();
        }

        dialogue.inThisDialogue = false;
        DialogueUtilities.InDialogue = false;
        
        popupUIElement.SetActive(false);
        CurrentCoroutine = null;
    }
    
}
