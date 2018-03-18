using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource Soundfx;

    public AudioSource Background;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        DontDestroyOnLoad(this);
    }

    public void PlaySFX(AudioClip clip)
    {
        Soundfx.clip = clip;
        Soundfx.loop = false;
        Soundfx.Play();
    }
}
