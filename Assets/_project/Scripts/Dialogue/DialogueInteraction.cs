using UnityEngine;
using System.Collections.Generic;

public enum DialogueType
{
    CloseUp, // Dialogue that appears in a close-up view
    PopUp    // Dialogue that appears as a pop-up
}

// Create a new asset menu for easy creation of DialogueInteraction assets
[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue System/Dialogue Interaction")]
public class DialogueInteraction : ScriptableObject
{
    // List of Dialogue lines for this interaction
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();

    // Type of Dialogue for this interaction
    public DialogueType dialogueType;
    
    [Header("Dialogue settings")]
    public bool canPlayerWalk;  // Indicates whether the player can walk during the Dialogue
    public bool autoSkipText;   // Indicates whether the text should be automatically skipped
    [HideInInspector] public bool inThisDialogue;
}
