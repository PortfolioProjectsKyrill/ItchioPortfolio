using System;
using UnityEngine;

public class FindTextBox : MonoBehaviour
{
    private static FindTextBox _instance;

    public static FindTextBox Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<FindTextBox>(true);

                if (_instance == null)
                {
                    Debug.Log("FindTextBox instance not found. Creating a new one.");
                    GameObject obj = new GameObject("FindTextBox");
                    _instance = obj.AddComponent<FindTextBox>();
                }
                else
                {
                    //Debug.Log("Found FindTextBox instance in the scene.");
                }
            }

            return _instance;
        }
    }

    public GameObject thisGameObject;

    public GameObject GetGameObject()
    {
        thisGameObject = this.gameObject;
        return thisGameObject;
    }
}


