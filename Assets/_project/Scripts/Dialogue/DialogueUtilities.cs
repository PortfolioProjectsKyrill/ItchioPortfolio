using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Static class for finding UI elements related to dialogues
public static class DialogueUtilities
{
    private static Dictionary<string, string> _textToReplace = new Dictionary<string, string>();

    public static bool InDialogue;

    // Method to find the appropriate UI element based on the firstDialogue type
    public static GameObject FindUIElement(DialogueType textType)
    {
        GameObject gameObject = null;

        switch (textType)
        {
            case DialogueType.CloseUp:
                break;
            case DialogueType.PopUp:
                gameObject = FindTextBox.Instance.GetGameObject();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(textType), textType, "Didn't find corresponding gameObject");
        }
        
        return gameObject;
    }

    // Method to find TextMeshProUGUI component within the UI element
    public static TextMeshProUGUI FindTextMeshProUGUI(DialogueType textType)
    {
        // Get the target UI element based on the firstDialogue type
        GameObject targetGameObject = FindUIElement(textType);

        if (targetGameObject == null)
        {
            Debug.LogError($"Target GameObject for {textType} not found.");
            return null;
        }

        // Get the TextMeshProUGUI component from the UI element
        TextMeshProUGUI textMeshPro = targetGameObject.gameObject.GetComponentInChildren<PopUpText>().
            gameObject.GetComponent<TextMeshProUGUI>();

        return textMeshPro;
    }
    
    public static void CheckDialogueText(DialogueLine lineToEdit)
    {
        //Debug.Log("Started checking: " + lineToEdit);

        if (!lineToEdit.checkPlaceholder)
        {
            lineToEdit.dialogueText = lineToEdit.originalDialogueText;
            Debug.Log("Nothing to change");
            return;
        }
        Debug.Log("Passed if");

        string originalString = lineToEdit.originalDialogueText;
        var newString = originalString; // Initialize newString with the original string

        Dictionary<string, string> wordsToReplace = new Dictionary<string, string>()
        {
            {"player", GameManager.Instance.playerName},
            {"forwardKey", GameManager.Instance.GetKey("Forward").ToString()},
            {"leftKey", GameManager.Instance.GetKey("Left").ToString()},
            {"backKey", GameManager.Instance.GetKey("Back").ToString()},
            {"rightKey", GameManager.Instance.GetKey("Right").ToString()},
            {"jumpKey", GameManager.Instance.GetKey("Interact").ToString()},
            {"interactKey", GameManager.Instance.GetKey("Jump").ToString()},
            {"catchFishKey", GameManager.Instance.GetKey("CatchFish").ToString()},
        };

        // Iterate over the dictionary and replace words
        foreach (var replacement in wordsToReplace)
        {
            newString = newString.Replace(replacement.Key, replacement.Value);
        }

        lineToEdit.dialogueText = newString;
    }

    public static void ChangeDialogueSprite(DialogueLine spriteOfLineToEdit)
    {
        if (spriteOfLineToEdit.speaker == Speaker.Player)
        {
            FindImage.Instance.gameObject.SetActive(false);
            Debug.Log("De-activate game object");
            return;

        }
        else
        {
            FindImage.Instance.gameObject.SetActive(true);
            Debug.Log("activate game object");
        }
        
        Image imageToChange = FindImage.Instance.GetImage();
        Debug.Log($"Found Image: {imageToChange}");
        
        Sprite sprite = DialogueSpriteManager.Instance.GetDialogueSprite(spriteOfLineToEdit);
        
        if (sprite != null)
            Debug.Log($"Found Sprite: {sprite}");

        imageToChange.sprite = sprite;
        Debug.Log($"Changed sprite: {imageToChange} in Image: {imageToChange}");

    }


}