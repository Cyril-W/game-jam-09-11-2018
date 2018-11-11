using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAudioEvent : MonoBehaviour {

    [SerializeField] AudioSource audioSource;

    public void Play(AudioClip clip)
    {
        if (clip == null && audioSource.clip == null)
        {
            Debug.LogError("Trying to play no clip");
            return;
        }

        audioSource.PlayOneShot(clip == null ? audioSource.clip : clip);
    }
}
