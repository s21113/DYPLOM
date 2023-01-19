using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorCollisionController : MonoBehaviour
{
    public GameObject elevator, elevatorDoors, frameDoorsUp, frameDoorsDown, upCollider, downCollider;
    public Animator frameDoorsAnimDown, frameDoorsAnimUp;
    public BoxCollider coll;

    //private bool up, down;
    // Start is called before the first frame update
    void Start()
    {
        // up = true;
        // down = false;
        elevator = GetComponent<GameObject>();       

    }

    // Update is called once per frame
    void Update()
    {
        /*if (coll.gameObject.tag == "UpCollider" || coll.gameObject.tag=="DownCollider")
        {
            DoorControl("Open");
        }
        else
        {
            DoorControl("Close");
        }
        */
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "UpCollider" || other.gameObject.tag == "DownCollider")
        {
            //print("No kolizja na gorze albo na dole");
            DoorControl("Open");
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        DoorControl("Close");
        //print("Closing");
    }
    void DoorControl(string direction)
    {
        frameDoorsAnimUp.SetTrigger(direction);
        frameDoorsAnimDown.SetTrigger(direction);
    }
}
