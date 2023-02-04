using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reppelantscript : MonoBehaviour
{

    GameObject zombie;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 4);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Object that collided with me: " + other.gameObject.name);
        if (other.gameObject.name == "parasite_l_starkie")
        {
            zombie = other.gameObject;
        }
        
    }

    private void OnDestroy()
    {
        if(zombie != null){
            zombie.GetComponent<ZombieWalk>().setIsInRangeFalse();
        }
       
    }
}
