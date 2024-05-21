using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSliders : MonoBehaviour
{
    public Slider MasterSlider;
    public Slider MusicSlider;
    public Slider FXSlider;
    public AudioMixerGroup Master;
    public AudioMixerGroup Music;
    public AudioMixerGroup FX;

    // Add any additional initialization or functionality as needed
    public void SetMaster(float volume)
    {
        Master.audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }
    public void SetMusic(float volume)
    {
        Music.audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }
    public void SetFX(float volume)
    {
        FX.audioMixer.SetFloat("FXVolume", Mathf.Log10(volume) * 20);
    }

    public void OnSliderValueChange()
    {
        SetMaster(MasterSlider.value);
        SetMusic(MusicSlider.value);
        SetFX(FXSlider.value);
    }
}
