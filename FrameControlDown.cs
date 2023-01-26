using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameControlDown : MonoBehaviour
{
    public GameObject frameDoorsDown;
    public Animator frameDoorsAnimDown;
    public BoxCollider coll;

    //private bool up, down;
    // Start is called before the first frame update
    void Start()
    {

        frameDoorsAnimDown.SetBool("Opened", false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "PlayerBody")
        {
            print("Kolizja z graczem");
            frameDoorsAnimDown.SetBool("Opened", true);
        }


    }
    private void OnTriggerExit(Collider other)
    {
        frameDoorsAnimDown.SetBool("Opened", false);
        print("Brak kolizji z graczem");
    }
    
}