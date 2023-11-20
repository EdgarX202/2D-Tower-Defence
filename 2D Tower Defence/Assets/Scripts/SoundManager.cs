using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource effectsSource;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider effectsSlider;

    Dictionary<string,AudioClip> audioClips = new Dictionary<string,AudioClip>();

    // Start is called before the first frame update
    void Start()
    {
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio") as AudioClip[];

        // Run through audio clips and add them to dictionary
        foreach (AudioClip clip in clips)
        {
            audioClips.Add(clip.name, clip);
        }

        LoadVolume();

        musicSlider.onValueChanged.AddListener(delegate { UpdateVolume(); });
        effectsSlider.onValueChanged.AddListener(delegate { UpdateVolume(); });
    }

    public void PlaysEffects(string name)
    {
        effectsSource.PlayOneShot(audioClips[name]);
    }

    public void UpdateVolume()
    {
        // Update the volume when slider are adjusted
        musicSource.volume = musicSlider.value;
        effectsSource.volume = effectsSlider.value;

        // Save volume settings
        PlayerPrefs.SetFloat("Effects", effectsSlider.value);
        PlayerPrefs.SetFloat("Music", musicSlider.value);
    }

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
