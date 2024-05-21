using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using static SoundManager;
using static UnityEngine.Rendering.PostProcessing.HistogramMonitor;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Inventory")]
    public AudioClip inventoryOpen;
    public AudioClip pageFlip;
    [SerializeField] private GameObject Inventory;

    [Header("Player")]
    public AudioClip steps;
    public AudioClip fishCaught;
    [SerializeField] private GameObject Player;

    [Header("Background Music")]
    [SerializeField] private AudioClip[] backgroundSongs;
    [SerializeField] private AudioSource _camera;
    [SerializeField] private int songIndex;

    [SerializeField] private float trackTimer;
    [SerializeField] private bool changed;

    [Header("Shop")]
    public AudioClip buyAndSell;
    [SerializeField] private GameObject Shop;

    [Header("MixerGroups")]
    [SerializeField] private AudioMixerGroup SoundFX;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        if (!_camera)
            return;

        if (_camera.isPlaying)
        {
            trackTimer += Time.deltaTime;
        }

        if (!_camera.isPlaying || trackTimer >= _camera.clip.length)
        {
            if (!changed)
            {
                changed = true;
                ChangeSong();
            }
        }
    }

    public void PlayInventoryClip(AudioClip sound)
    {
        StartCoroutine(InventoryClip(sound));
    }

    private IEnumerator InventoryClip(AudioClip sound)
    {
        AudioSource source = Inventory.AddComponent<AudioSource>();
        source.clip = sound;
        source.outputAudioMixerGroup = SoundFX;
        float t = source.clip.length;
        source.Play();
        yield return new WaitForSeconds(t);
        Destroy(source);
    }

    public void PlayPlayerClip(AudioClip sound)
    {
        StartCoroutine(PlayerClip(sound));
    }

    private IEnumerator PlayerClip(AudioClip sound)
    {
        AudioSource source = Player.AddComponent<AudioSource>();
        source.clip = sound;
        source.outputAudioMixerGroup = SoundFX;
        float t = source.clip.length;
        source.Play();
        yield return new WaitForSeconds(t);
        Destroy(source);
    }

    public void PlayBGClip()
    {
        if (songIndex > backgroundSongs.Length - 1)
        {
            songIndex = 0;
            _camera.clip = backgroundSongs[songIndex];
        }
        else
        {
            songIndex++;
            _camera.clip = backgroundSongs[songIndex - 1];
        }
        _camera.Play();
    }

    public void PlayShopClip(AudioClip sound)
    {
        StartCoroutine(ShopClip(sound));
    }

    private IEnumerator ShopClip(AudioClip sound)
    {
        AudioSource source = Shop.AddComponent<AudioSource>();
        source.clip = sound;
        source.outputAudioMixerGroup = SoundFX;
        float t = source.clip.length;
        source.Play();
        yield return new WaitForSeconds(t);
        Destroy(source);
    }

    private void ChangeSong()
    {
        PlayBGClip();
        trackTimer = 0;
        changed = false;
    }
}
