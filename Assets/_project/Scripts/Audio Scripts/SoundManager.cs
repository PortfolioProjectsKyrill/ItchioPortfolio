using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    [System.Serializable]
    public class Sound
    {
        public string name;      // Name of the sound
        public AudioClip clip;   // AudioClip associated with the sound
    }

    [Header("audio")]
    [Tooltip("Add music audio into this list")]
    public List<Sound> music = new List<Sound>();
    [Tooltip("Add sfx audio into this list")]
    public List<Sound> sfx = new List<Sound>();
    [Tooltip("Add ambience audio into this list")]
    public List<Sound> ambience = new List<Sound>();
    [Tooltip("Add UI audio into this list")]
    public List<Sound> UI = new List<Sound>();

    private Dictionary<string, AudioSource> _audioSources = new Dictionary<string, AudioSource>();

    // Singleton instance
    private static SoundManager instance;

    // Awake is called before the first frame update
    private void Awake()
    {
        // Ensure only one instance of SoundManager exists
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            // Destroy duplicate instances
            Destroy(gameObject);
            return;
        }

        // Don't destroy the SoundManager object when loading new scenes
        DontDestroyOnLoad(gameObject);

        // Initialize sounds for different categories
        InitializeSounds(music, "m_");
        InitializeSounds(sfx, "sfx_");
        InitializeSounds(ambience, "amb_");
        InitializeSounds(UI, "ui_");
    }

    // InitializeSounds method sets up AudioSources for a list of sounds with a given prefix
    private void InitializeSounds(List<Sound> soundList, string prefix)
    {
        foreach (Sound sound in soundList)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = sound.clip;
            _audioSources[prefix + sound.name] = source;
        }
    }

    // Instance property to get the singleton instance
    public static SoundManager Instance
    {
        get
        {
            return instance;
        }
    }
    
    /// <summary>
    /// Plays the sound associated with the given audio name if it exists in the SoundManager.
    /// </summary>
    /// <param name="pAudioName">Name of the sound to be played.</param>
    /// <remarks>
    /// If the specified sound is not found, a warning message is logged to the console.
    /// </remarks>
    public void PlaySound(string pAudioName)
    {
        if (_audioSources.ContainsKey(pAudioName))
        {
            _audioSources[pAudioName].Play();
        }
        else
        {
            Debug.LogWarning("Sound " + pAudioName + " not found in the SoundManager.");
        }
    }

    /// <summary>
    /// Stops the playback of the sound with the specified name if it exists in the SoundManager.
    /// </summary>
    /// <param name="pAudioName">Name of the sound to be stopped.</param>
    /// <remarks>
    /// If the specified sound is not found, a warning message is logged to the console.
    /// </remarks>
    public void StopSound(string pAudioName)
    {
        if (_audioSources.ContainsKey(pAudioName))
        {
            _audioSources[pAudioName].Stop();
        }
        else
        {
            Debug.LogWarning("Sound " + pAudioName + " not found in the SoundManager.");
        }
    }

    /// <summary>
    /// Plays a one-shot instance of the sound with the specified name if it exists in the SoundManager.
    /// </summary>
    /// <param name="pAudioName">Name of the sound to be played as a one-shot.</param>
    /// <remarks>
    /// If the specified sound is not found, a warning message is logged to the console.
    /// </remarks>
    public void PlayOneShot(string pAudioName)
    {
        if (_audioSources.ContainsKey(pAudioName))
        {
            _audioSources[pAudioName].PlayOneShot(_audioSources[pAudioName].clip);
        }
        else
        {
            Debug.LogWarning("Sound " + pAudioName + " not found in the SoundManager.");
        }
    }
}