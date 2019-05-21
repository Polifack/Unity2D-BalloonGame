using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager singleton;
    public Audio[] audios;

    private void Awake()
    {
        if (singleton != null)
        {
            Destroy(gameObject);
        }
        else
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        foreach (Audio audio in audios)
        {
            audio.source = gameObject.AddComponent<AudioSource>();
            audio.source.clip = audio.clip;
            audio.source.loop = audio.loop;
        }
    }
    public void Play(string soundName)
    {
        Audio audio = Array.Find(audios, item => item.nameAudio == soundName);
        if (audio == null)
        {
            Debug.LogWarning(" * Error: " + soundName+" audio not found");
            return;
        }

        audio.source.volume = audio.source.volume * (1f + UnityEngine.Random.Range(-audio.volumeDelta / 2f, audio.volumeDelta / 2f));
        audio.source.pitch = audio.source.pitch * (1f + UnityEngine.Random.Range(-audio.volumeDelta / 2f, audio.volumeDelta / 2f));

        audio.source.Play();
    }
    public void Stop(string soundName)
    {
        Audio audio = Array.Find(audios, item => item.nameAudio == soundName);
        if (audio == null)
        {
            Debug.LogWarning(" * Error: " + soundName + " audio not found");
            return;
        }

        if (!audio.source.isPlaying)
        {
            Debug.LogWarning(" * Error: " + soundName + " is not playing");
            return;
        }

        audio.source.Stop();
    }

    public void StopAll()
    {
        foreach( Audio a in audios)
        {
            a.source.Stop();
        }
    }
}
