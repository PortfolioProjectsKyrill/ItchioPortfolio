using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Ensures that there is only one _instance of GameManager
    public static GameManager Instance;

    private KeyCode _lastPressedKey = KeyCode.None;

    [Header("settings"), Space(8)]
    [Header("Player"), Space]
    public string playerName = "player";
    [Space]
    public float xSensitivity;
    public float ySensitivity;
    
    public KeyCode forwardKey = KeyCode.W;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode backKey = KeyCode.S;
    public KeyCode rightKey = KeyCode.D;
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode mapKey = KeyCode.M;
    [Space]
    public KeyCode interactKey = KeyCode.E;
    public KeyCode questKey = KeyCode.Q;

    [Header("fishing"),Space]
    public KeyCode catchFishKey = KeyCode.Mouse1;

    //Keybinds
    public Dictionary<string, KeyCode> KeyBinds;

    public DeathScreen deathScreen;
    public Coroutine respawnCoroutine;
    public GameObject questWindow;

    [SerializeField] private bool mapState;
    private string _awaitingKeybindButton;

    public int landingPlatformActDist;

    public bool IsLastPressedKeyAllowed()
    {
        return _lastPressedKey != GetKey("Forward") &&
               _lastPressedKey != GetKey("Left") &&
               _lastPressedKey != GetKey("Back") &&
               _lastPressedKey != GetKey("Right") &&
               _lastPressedKey != GetKey("Interact") &&
               _lastPressedKey != GetKey("Jump") &&
               _lastPressedKey != GetKey("CatchFish");
    }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        
        
        KeyBinds = new Dictionary<string, KeyCode>();
        Time.timeScale = 1.0f;

        DefaultKeybinds();
    }

    private void GetDeathsScreenAndQuestWindow()
    {
        deathScreen = FindObjectOfType<DeathScreen>();
        questWindow = deathScreen.quests;
    }

    private void Update()
    {
        if ((SceneManager.GetActiveScene().buildIndex == 3 || SceneManager.GetActiveScene().buildIndex == 2) && deathScreen == null)
        {
            GetDeathsScreenAndQuestWindow();
        }

        if (Input.anyKeyDown)
        {
            // Check all possible keys manually
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    _lastPressedKey = keyCode;
                    break;
                }
            }
        }

        ShowMap();
        if (Input.GetKeyDown(questKey))
        {
            QuestButton(ref questWindow);
        }

        
        if (!string.IsNullOrEmpty(_awaitingKeybindButton) && Input.anyKeyDown)
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (!Input.GetKeyDown(keyCode))
                    continue;
                
                
                BindKey(_awaitingKeybindButton, keyCode);
                _awaitingKeybindButton = null;
                break;
            }
        }


    }
    
    public void StartKeybindInput(string key)
    {
        _awaitingKeybindButton = key;
        KeybindManager.Instance.UpdateKeyText(key, KeyCode.None);
    }

    public void StartEndScreenFadeIn(string text = null)
    {
        respawnCoroutine ??= StartCoroutine(deathScreen.FadeInScreen(text));
    }

    public void LoadScene(int scene)
    {
        MenuManager.instance.LoadScene(scene);
    }

    public void ApplySettings(int[] ints)
    {
        QualitySettings.SetQualityLevel(ints[0]);
        QualitySettings.antiAliasing = ints[1];
        QualitySettings.vSyncCount = ints[2];
        QualitySettings.masterTextureLimit = ints[3];
        QualitySettings.shadowResolution = ints[4] switch
        {
            0 => ShadowResolution.Low,
            1 => ShadowResolution.Medium,
            2 => ShadowResolution.High,
            3 => ShadowResolution.VeryHigh,
            _ => ShadowResolution.VeryHigh,
        };
    }

    public void QuestButton(ref GameObject questTextParent)
    {
        if (questTextParent.active)
        {
            questTextParent.SetActive(false);
        }
        else
        {
            questTextParent.SetActive(true);
        }
    }

    private void ShowMap()
    {
        if (Input.GetKeyDown(mapKey))
        {
            if (!mapState)
            {
                MapController.instance.TurnOnMap();
                mapState = true;
                MapController.instance.ZoneAccessShowing();
            }
            else
            {
                MapController.instance.TurnOffMap();
                mapState = false;
            }
        }
    }

    public void BindKey(string key, KeyCode keyBind)
    {
        Dictionary<string, KeyCode> currentDictionary = KeyBinds;

        if (!currentDictionary.ContainsKey(key))
        {
            currentDictionary.Add(key,keyBind);
            Debug.Log($"added {key} with value: {keyBind} to the dictionary");
        }
        else if (currentDictionary.ContainsValue(keyBind))
        {
            RebindKey(key, keyBind);
        }
        
        currentDictionary[key] = keyBind;
        KeybindManager.Instance.UpdateKeyText(key, keyBind);
    }

    private void RebindKey(string key, KeyCode keyBind)
    {
        Dictionary<string, KeyCode> currentDictionary = KeyBinds;
        
        string myKey = currentDictionary.FirstOrDefault(x => x.Value == keyBind).Key;

        currentDictionary[myKey] = KeyCode.None;
        //foreach (var keys in currentDictionary)     
        //{
        //    Debug.Log($"key: {keys.Key} with value: {keys.Value} is in the dict");
        //}
        KeybindManager.Instance.UpdateKeyText(key,KeyCode.None);
    }
    


    public void DefaultKeybinds()
    {
        if (KeybindManager.Instance != null)
        {
            // Bind keys for different actions
            BindKey("Forward", KeyCode.W);
            BindKey("Left", KeyCode.A);
            BindKey("Back", KeyCode.S);
            BindKey("Right", KeyCode.D);
            BindKey("Jump", KeyCode.Space);
            BindKey("Interact", KeyCode.E);
            BindKey("CatchFish", KeyCode.Mouse1);
            BindKey("Menu", KeyCode.Escape);
            BindKey("Map", KeyCode.M);
            BindKey("Quest", KeyCode.Q);
        }
    }

    public KeyCode GetKey(string key)
    {
        Dictionary<string, KeyCode> currentDictionary = KeyBinds;

        if (currentDictionary.ContainsKey(key))
        {
            return currentDictionary[key];
        }
        else
        {
            return KeyCode.Joystick2Button6;
        }
    }
}