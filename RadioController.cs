using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RadioController : MonoBehaviour
{
    //public static RadioController instance;
    public AudioClip[] audioClips;
    private AudioSource source;

    IEnumerator ChangeSong()
    {
        while (source.isPlaying)
        {
            int index = (int)Random.Range(0, audioClips.Length);
            source.clip = audioClips[index];
            source.Play();
            yield return new WaitForSecondsRealtime(45);
        }
    }

    void Start()
    {
        source = GetComponent<AudioSource>();
        audioClips = Resources.LoadAll<AudioClip>("Sounds/RadioMusic");
        StartCoroutine(ChangeSong());
    }
}
