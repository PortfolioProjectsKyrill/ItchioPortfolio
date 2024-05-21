using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindImage : MonoBehaviour
{
    private static FindImage _instance;

    public static FindImage Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<FindImage>(true);
            }

            if (_instance== null)
            {
                Debug.Log("FindSprite instance not found. Creating a new one.");
                GameObject obj = new GameObject("FindSprite");
                _instance = obj.AddComponent<FindImage>();
            }
            else
            {
                Debug.Log("Found FindSprite instance in the scene.");
            }

            return _instance;
        }
    }

    public Image thisImage;

    public Image GetImage()
    {
        Image image = GetComponent<Image>();
        thisImage = image;
        return image;
    }
}

