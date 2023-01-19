using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorDoorsScript : MonoBehaviour
{
    Animator animator;
    bool doorOpen;
    

    // Start is called before the first frame update
    void Start()
    {
        doorOpen = false;
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "DownCollider")
        {
            doorOpen = true;
            DoorControl("Open");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (doorOpen)
        {
            doorOpen = false;
            DoorControl("Close");
        }
    }

    void DoorControl(string direction)
    {
        animator.SetTrigger(direction);

    }
}
