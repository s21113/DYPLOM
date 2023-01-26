using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameControlUp : MonoBehaviour
{
    public GameObject frameDoorsUp;
    public Animator frameDoorsAnimUp;
    public BoxCollider coll;
    bool player, elevator;
    //private bool up, down;
    // Start is called before the first frame update
    void Start()
    {
        player = false;
        elevator = false;
        frameDoorsAnimUp.SetBool("Opened", false);
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
            player = true;
            frameDoorsAnimUp.SetBool("Opened", true);

        }
        /*if (other.gameObject.tag == "Elevator")
        {
            print("Kolizja z  windą");
            elevator = true;
            //frameDoorsAnimUp.SetBool("Opened", true);
        }
        Debug.Log("Kolizja gracz: " + player + " Winda: " + elevator);
        if(elevator==true && player == true)
        {
            frameDoorsAnimUp.SetBool("Opened", true);
            
        }*/



    }
    private void OnTriggerExit(Collider other)
    {
        frameDoorsAnimUp.SetBool("Opened", false);
        print("Brak kolizji z graczem");
        player = false;
        elevator = false;
    }
    void DoorControl(string direction)
    {
        frameDoorsAnimUp.SetTrigger(direction);
    }
}

