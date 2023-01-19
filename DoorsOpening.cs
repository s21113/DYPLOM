using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorsOpening : MonoBehaviour
{
    public Transform playerCamera;
    public float maxDistance = 100;
    private bool opened = false;
    private Animator anim;
    public AudioSource doorSound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Pressed();
            //Debug.Log("E pressed");
        }
    }

    void Pressed() 
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit doorHit;
        if(Physics.Raycast(ray, out doorHit, maxDistance))
        {
            
            if(doorHit.collider.tag == "Door")
            {
               // Debug.Log("Trafiono drzwi");
                doorSound.Play();
                anim = doorHit.collider.GetComponentInParent<Animator>();
                if(opened)
                opened = false;
                else
                opened = true;
                anim.SetBool("Opened", opened);
            }
            else
            {
                Debug.DrawLine(ray.origin, ray.origin+ray.direction *100, Color.green);
            }
        }
    }
}
