using System;
using UnityEngine;

[Serializable]
public class SoundClip
{
    [SerializeField] AudioClip clip;
    [SerializeField] float volume;

    public AudioClip Clip => clip;
    public float Volume => volume;
}
