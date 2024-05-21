using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    #region instence
    private static TypewriterEffect _instance;

    public static TypewriterEffect Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<TypewriterEffect>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("TypewriterEffect");
                    _instance = obj.AddComponent<TypewriterEffect>();
                }
            }
            return _instance;
        }
    }
    #endregion

    private string _currentFullText;
    private bool _isTyping;
    private TextMeshProUGUI _currentTextMeshPro;
    

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            // Check if the pressed key is not the interact key or movement keys
            if (GameManager.Instance.IsLastPressedKeyAllowed())
            {
                SkipTyping();
            }
        }

    }

    public IEnumerator TypeText(DialogueLine dialogue, TextMeshProUGUI textMeshPro)
    {
        SetVars(dialogue, textMeshPro);

        foreach (char c in _currentFullText)
        {

            _currentTextMeshPro.text += c;

            if (_isTyping)
            {
                if (c.ToString() == "." || c.ToString() == "?" || c.ToString() == "!")
                {
                    Debug.Log("detected a '.'");
                    yield return new WaitForSecondsRealtime(dialogue.textSpeed * 10);
                }
                else if (c.ToString() == ",")
                {
                    Debug.Log("detected a ','");
                    yield return new WaitForSecondsRealtime(dialogue.textSpeed * 6f);
                }
                else
                {
                    yield return new WaitForSecondsRealtime(dialogue.textSpeed);
                }
                
            }
        }

        
        ResetTypewriterEffect();
        yield return null;
    }

    private void SkipTyping()
    {
        if (!_isTyping)
            return;
        
        //StopAllCoroutines();
        //_currentTextMeshPro.text = _currentFullText;
        _isTyping = false;
    }

    private void SetVars(DialogueLine dialogue, TextMeshProUGUI textMeshPro)
    {
        _isTyping = true;

        textMeshPro.font = dialogue.lineFont != null ? dialogue.lineFont : TMP_Settings.defaultFontAsset;
        
        _currentFullText = dialogue.dialogueText;
        _currentTextMeshPro = textMeshPro;
        _currentTextMeshPro.text = ""; // Clear the text initially
    }

    private void ResetTypewriterEffect()
    {
        _isTyping = false;
        _currentTextMeshPro = null;
        _currentFullText = null;
    }
    
}
