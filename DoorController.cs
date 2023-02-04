using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    Animator animator;
    bool doorOpen, near;
    // Start is called before the first frame update
    void Start()
    {
        doorOpen = false;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        DoorControl();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerBody")
        {
            near = true;
            print("Czy blisko:" + near);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "PlayerBody")
        {
            
            near = false;
            print("Czy blisko:"+near);
        }
    }
    void DoorControl(){
        if(near && Input.GetKeyDown(KeyCode.E)){
            doorOpen=!doorOpen;
            animator.SetBool("Opened", doorOpen);
        }
        //else
        //print("Not near door");
    }
}
