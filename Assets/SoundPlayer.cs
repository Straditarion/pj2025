using System;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public static SoundPlayer Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Play(AudioClip clip, float volume)
    {
        GetComponent<AudioSource>().PlayOneShot(clip, volume);
    }
}
