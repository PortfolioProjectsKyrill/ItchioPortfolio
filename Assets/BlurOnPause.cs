using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class BlurOnPause : MonoBehaviour
{
    private PostProcessVolume _postProcessVolume;

    // Reference to the DepthOfField component
    private DepthOfField _depthOfField;

    [SerializeField] private bool _inMenu;

    private void Start()
    {
        // Get the PostProcessVolume component attached to the camera
        _postProcessVolume = GetComponent<PostProcessVolume>();

        if (_postProcessVolume == null)
        {
            Debug.LogError("PostProcessVolume component not found on the camera.");
        }
        else
        {
            // Get or add the DepthOfField component to the volume
            if (!_postProcessVolume.profile.TryGetSettings(out _depthOfField))
            {
                _depthOfField = _postProcessVolume.profile.AddSettings<DepthOfField>();
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Toggle the Depth of Field effect
            _inMenu = _inMenu == true ? _inMenu = false : _inMenu = true;
            SetDepthOfFieldEnabled(_inMenu);
        }
    }

    // Enable or disable Depth of Field
    public void SetDepthOfFieldEnabled(bool isEnabled)
    {
        if (_depthOfField == null)
            return;

        _depthOfField.active = isEnabled;
        _inMenu = isEnabled;
    }
}