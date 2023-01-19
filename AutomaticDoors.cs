using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticDoors : MonoBehaviour
{

    Animator animator;
    bool doorOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerBody")
        {
            var eq = other.GetComponentInChildren<EqSystem>(true); // jezu zapomniałem o tej metodzie
            if (eq == null) return; // guard clause
            if (eq.GetImportantPoints() == 7) // teraz jak nie mamy siedmiu, to nie wyjdziemy
            {
                doorOpen = true;
                DoorControl("Open");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (doorOpen == false) return;
        doorOpen = false;
        DoorControl("Close");
    }

    void DoorControl(string direction)
    {
        animator.SetTrigger(direction);
    }
    
}
