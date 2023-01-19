using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieScriptCrawl : MonoBehaviour
{
    private Renderer rnd;
    private Animator zbAnim;
    public AudioSource audioSource;
    public AudioClip clip;
    private float volume = 1f;
    private float lifeTime = 1.4f;

    private Transform straszak;
    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        rnd = GetComponent<Renderer>();
        rnd.enabled = false;
        zbAnim = gameObject.GetComponentInParent<Animator>();
        zbAnim.enabled = false;

        //straszak = GameObject.FindGameObjectWithTag("Jumpscare").transform;
        player = GameObject.FindGameObjectWithTag("PlayerBody").transform;

    }

    private void OnTriggerEnter(Collider other)
    {
        Vector3 targetPostition = new Vector3(player.position.x,
                                      transform.position.y,
                                      player.position.z);

        if (other.gameObject.tag == "PlayerBody")
        {

            transform.parent.LookAt(targetPostition);
            audioSource.PlayOneShot(clip, volume);
            rnd.enabled = true;
            zbAnim.enabled = true;
            Destroy(gameObject, lifeTime);
        }
    }
}