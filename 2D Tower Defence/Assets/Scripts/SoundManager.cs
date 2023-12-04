using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SoundManager : Singleton<SoundManager>
{
    // Serialised Fields
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource effectsSource;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider effectsSlider;

    Dictionary<string,AudioClip> audioClips = new Dictionary<string,AudioClip>();

    // Start is called before the first frame update
    void Start()
    {
        // Load audio clips from the folder
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio") as AudioClip[];

        // Run through audio clips and add them to dictionary
        foreach (AudioClip clip in clips)
        {
            audioClips.Add(clip.name, clip);
        }

        LoadVolume();

        // Adjust volume sliders
        musicSlider.onValueChanged.AddListener(delegate { UpdateVolume(); });
        effectsSlider.onValueChanged.AddListener(delegate { UpdateVolume(); });
    }

    // Plays a sound effect once
    public void PlaysEffects(string name)
    {
        effectsSource.PlayOneShot(audioClips[name]);
    }

    // Adjust and save volume settings in PlayerPrefs
    public void UpdateVolume()
    {
        // Update the volume when slider are adjusted
        musicSource.volume = musicSlider.value;
        effectsSource.volume = effectsSlider.value;

        // Save volume settings
        PlayerPrefs.SetFloat("Effects", effectsSlider.value);
        PlayerPrefs.SetFloat("Music", musicSlider.value);
    }

    // Load volume settings when the game is started next time
    public void LoadVolume()
    {
        // Store volume
        effectsSource.volume = PlayerPrefs.GetFloat("Effects", 0.5f);
        musicSource.volume = PlayerPrefs.GetFloat("Music", 0.5f);

        // Update value
        effectsSlider.value = effectsSource.volume;
        musicSlider.value = musicSlider.value;
    }
    
}
