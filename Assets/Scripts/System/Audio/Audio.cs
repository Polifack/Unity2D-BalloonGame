using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAudio", menuName = "System/Audio", order = 1)]
public class Audio : ScriptableObject
{
    public string nameAudio;
    public AudioSource source;
    public AudioClip clip;
    public bool loop;
    public float volume;
    public float volumeDelta;
    public float pitch;
    public float pitchDelta;
}
