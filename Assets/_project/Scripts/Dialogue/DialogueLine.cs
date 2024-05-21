using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public enum Speaker
{
    Player,
    KnappeMan,
    Bip,
    Chamos,
    Plimby,
    Fishlegs
    
}

[CreateAssetMenu(fileName = "NewDialogueLine", menuName = "Dialogue System/Dialogue Line")]
public class DialogueLine : ScriptableObject
{

    [Header("LineSettings"), Space] 
    public bool useBreaks = true;
    public bool checkPlaceholder;
    public float textSpeed = 0.02f;
    [Space]
    public TMP_FontAsset lineFont;
    
    [Header("DialogueText"), Space] [Tooltip("The name of the person's dialog")]
    public Speaker speaker = Speaker.Player;

    public string nameOfSprite;
    
    [Space]
    [FormerlySerializedAs("dialogueText")] [TextArea(3, 10), Tooltip("in here write what the dialog line should be saying")]
    public string originalDialogueText;

    [HideInInspector] public string dialogueText;




}