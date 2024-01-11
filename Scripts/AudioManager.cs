using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    private static AudioManager instance;

    void Awake()
    {
        if (instance == null){
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds){
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.spatialBlend = s.spatialBlend ;
            s.source.loop = s.loop;
        }
    }

    /*void Start()
    {
        Play("Theme");
    }*/

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null){
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        if (s.source == null){
            Debug.LogWarning("Sound source for: " + name + " not found!");
            return;
        }

        // Set pitch to a random value between 0.8 and 1.2.
        // You can change these values to get the range you want.
        s.source.pitch = UnityEngine.Random.Range(0.7f, 1.3f);

        s.source.Play();
    }

    public float GetSoundLength(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return 0;
        }else {
            return s.clip.length;
        }
    }
}
