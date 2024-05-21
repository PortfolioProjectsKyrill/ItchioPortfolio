using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class KeybindManager : MonoBehaviour
{

    private static KeybindManager _instance;

    public static KeybindManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<KeybindManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("KeybindManagerSingleton");
                    _instance = singletonObject.AddComponent<KeybindManager>();
                }
            }
            return _instance;
        }
    }

    [SerializeField] private GameObject[] keybindButtons;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }

        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        GameManager.Instance.DefaultKeybinds();
        

        KeybindReference[] keybindReferences = FindObjectsOfType<KeybindReference>(true);
        keybindButtons = keybindReferences.Select(k => k.gameObject).ToArray();
        UpdateKeybindTexts();
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        // Clear the existing array
        Array.Clear(keybindButtons, 0, keybindButtons.Length);

        // Find UI elements in the new scene and update keybind texts
        KeybindReference[] keybindReferences = FindObjectsOfType<KeybindReference>(true);
        keybindButtons = keybindReferences.Select(k => k.gameObject).ToArray();
        UpdateKeybindTexts();
    }

    
    public void UpdateKeyText(string key, KeyCode code)
    {
        GameObject keybindButton = Array.Find(keybindButtons, x => x.name == key);
        if (keybindButton != null)
        {
            TextMeshProUGUI tmp = keybindButton.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = code.ToString();
        }
        else
        {
            Debug.LogWarning("Keybind button not found: " + key);
        }
    }
    
    
    private void UpdateKeybindTexts()
    {
        GameManager gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene!");
            return;
        }

        Dictionary<string, KeyCode> keybinds = gameManager.KeyBinds;
        if (keybinds == null)
        {
            Debug.LogError("Keybinds dictionary is null!");
            return;
        }

        foreach (GameObject keybindButton in keybindButtons)
        {
            string key = keybindButton.name;
            if (keybinds.TryGetValue(key, out KeyCode code))
            {
                UpdateKeyText(key, code);
            }
            else
            {
                Debug.LogError("Key not found in the Keybinds dictionary: " + key);
            }
        }
    }
}
