using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;


public class DialogueSpriteManager : MonoBehaviour
{
    [System.Serializable]
    class DialogueSprite
    {
        public Speaker speaker;
        public string nameOfSprite;
        public Sprite sprite;
    }
    
    private static DialogueSpriteManager _instance;
    public Dictionary<string, Sprite> SpriteDictionary = new Dictionary<string, Sprite>();
    
    [SerializeField]
    private List<DialogueSprite> dialogueSpritesList = new List<DialogueSprite>();

    public static DialogueSpriteManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DialogueSpriteManager>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("DialogueSpriteManager");
                    _instance = singletonObject.AddComponent<DialogueSpriteManager>();
                }
            }
            return _instance;
        }
    }

    private void InitializeDialogueSprites(List<DialogueSprite> dialogueSpriteList)
    {
        foreach (DialogueSprite dialogueSprite in dialogueSpriteList)
        {
            string key = GenerateKey(dialogueSprite.speaker.ToString(), dialogueSprite.nameOfSprite);

            if (!SpriteDictionary.ContainsKey(key))
            {
                SpriteDictionary.Add(key, dialogueSprite.sprite);
                //Debug.Log($"Added {dialogueSprite.sprite} on key:{key} to {SpriteDictionary}");
            }
            else
            {
                Debug.LogWarning("Key already exists in the dictionary: " + key);
            }
        }
    }

    private void Start()
    {
        InitializeDialogueSprites(dialogueSpritesList);
    }

    public Sprite GetDialogueSprite(string speaker, string nameOfSprite)
    {
        string key = GenerateKey(speaker, nameOfSprite);

        Debug.Log($"trying to find {key}");
        
        if (SpriteDictionary.ContainsKey(key))
        {
            return SpriteDictionary[key];
        }
        else
        {
            string defaultKey = GenerateKey(speaker, "Default");
            if (SpriteDictionary.ContainsKey(defaultKey))
            {
                return SpriteDictionary[defaultKey];
            }
            else
            {
                Debug.LogWarning("Key not found in the dictionary: " + key);
                return null;
            }
        }
    }

    public Sprite GetDialogueSprite(DialogueLine dialogueLine)
    {
        return GetDialogueSprite(dialogueLine.speaker.ToString(), dialogueLine.nameOfSprite);
    }

    private string GenerateKey(string speaker, string nameOfSprite)
    {
        return speaker + "_" + nameOfSprite;
    }
}
