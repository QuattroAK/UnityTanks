using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundController : MonoBehaviour
{
    private static SoundController instance;

    [Header("Audio components")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioItem> audioItems;

    public static SoundController Instance
    {
        get
        {
            return instance;
        }
    }

    public void Init()
    {
        instance = this;
        Instance.PlayAudio(TypeAudio.BackgroundMusic);
    }

    public void PlayAudio(TypeAudio typeAudio)
    {
        foreach (var audioItem in audioItems)
        {
            if (audioItem.TypeAudio == typeAudio)
            {
                audioSource.clip = audioItem.AudioClip;
            }
        }

        audioSource.Play();
    }
}

public enum TypeAudio
{
    BackgroundMusic,
}

[Serializable]
public class AudioItem
{
    public TypeAudio TypeAudio;
    public AudioClip AudioClip;
}

