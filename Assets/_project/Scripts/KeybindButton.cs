using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeybindButton : MonoBehaviour
{
    [SerializeField, TextArea()] private string description = "make sure the Button name is an keybind";
    public string buttonName;

    private void Start()
    {
        buttonName = gameObject.name;
    }
    
    public void OnClick()
    {
        GameManager.Instance.StartKeybindInput(buttonName);
    }  
}