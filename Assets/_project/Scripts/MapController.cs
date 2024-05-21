using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    public static MapController instance;
    public List<GameObject> spriteObjects;

    public List<Zone> zones;
    public List<TextMeshPro> zoneTexts;

    public Color textColor;
    public TMP_FontAsset font;

    public GameObject camTexture;

    public Vector3 textRotation;

    public Vector2 textBoxSize;
    public float fontSize;

    private void Awake()
    {
        instance = this;
        spriteObjects = new List<GameObject>();
    }

    public void TurnOnMap()
    {
        camTexture.SetActive(true);
        for (int i = 0; i < spriteObjects.Count; i++)
        {
            spriteObjects[i].SetActive(true);
        }
    }

    public void TurnOffMap()
    {
        camTexture.SetActive(false);
        for (int i = 0; i < spriteObjects.Count; i++)
        {
            spriteObjects[i].SetActive(false);
        }
    }

    public void ZoneAccessShowing()
    {
        for (int i = 0;i < zones.Count; i++) 
        {
            if (zones[i].unlocked)
            {
                zoneTexts[i].text = "Area Unlocked";
            }
            else if (!zones[i].unlocked)
            {
                zoneTexts[i].text = "Area Locked";
            }
        }
    }
}
