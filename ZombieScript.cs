using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieScript : MonoBehaviour
{
    private Renderer rnd;
    private Animator zbAnim;
    public AudioSource audioSource;
    public AudioClip clip;
    private float volume = 1f;
    private float lifeTime = 2f;

    private Transform straszak;
    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        //straszak = GameObject.FindGameObjectWithTag("Jumpscare").transform;
        player = GameObject.FindGameObjectWithTag("PlayerBody").transform;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerBody")
        {
            audioSource.PlayOneShot(clip, volume);
            Destroy(gameObject, lifeTime);
        }
    }
}
