using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

public class PickupScript : MonoBehaviour
{
    //float distance = 0;
    private GameObject eqSystem;
    private GameObject player;
    private GameObject smartphone;
    public Item item;
    //public GameObject sphere;

    //public bool playerInSightRange;
    void Start()
    {
        eqSystem = GameObject.FindGameObjectWithTag("Inventory");
        player = GameObject.FindGameObjectWithTag("PlayerBody");
        smartphone = GameObject.FindGameObjectWithTag("Smartphone");


        

    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 distanceToPlayer = player.position - transform.position;
        //float distance = Mathf.Sqrt(Mathf.Pow(player.position.x - transform.position.x, 2) + Mathf.Pow(player.position.y - transform.position.y, 2) + Mathf.Pow(player.position.z - transform.position.z, 2));

        /*if (distance < 1.5)
        {
            Player.Pickup();
            Destroy(gameObject);
        }*/
        /*if (distance < 1.5)
        {
            Player.Pickup();
            Destroy(gameObject);
        }*/

    }

    private void OnTriggerEnter(Collider collider)
    {
        if (item == null) return;
        if (collider.gameObject.name != "Player") return;
        if (tag == "PowerBank")
        {
            player.GetComponentInChildren<Flashlight>().ChargeUp(100);
            //smartphone.GetComponentInChildren<TheSmartphone>().UpdateBatteryLevel();
        }
        else if (tag == "Health"){
            player.GetComponentInChildren<EnergyBar>().IncreaseHealth();
           

        }
        else if (tag == "Znajdzka"){
            eqSystem.GetComponent<EqSystem>().PickupItem(item);
        }
        
        Destroy(this.transform.gameObject);
    }

}
