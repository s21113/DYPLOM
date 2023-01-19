using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

public class PickupScript : MonoBehaviour
{
    //private Rigidbody rb;
    //private BoxCollider coll;
    //public Transform player;
    //float distance = 0;
    private GameObject eqSystem;
    private GameObject player;
    public Item item;


    //public GameObject sphere;
    //public Color newcolor;
    //private Renderer renderer;

    //public bool playerInSightRange;
    //public LayerMask whatIsPlayer;
    void Start()
    {
        eqSystem = GameObject.Find("EqSystem");
        player = GameObject.Find("Player");

        //renderer.material.shader = Shader.Find("HDRP/Lit");

        //gameObject.GetComponent<Renderer>().material.color = Color.red;

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

        //playerInSightRange = Physics.CheckSphere(transform.position, 5, whatIsPlayer);
        //if (distance < 5)
        //{
        //   // Debug.Log(Time.deltaTime);

        //    renderer.material.SetColor("_BaseColor", Color.red);
        //    //renderer.sharedMaterial.SetColor("_Color", Color.red);
        //}
        //else {
        //    renderer.material.SetColor("_BaseColor", Color.yellow);
        //}

    }

    private void OnTriggerEnter(Collider collider)
    {
        if (item == null) return;
        if (collider.gameObject.name == "Player" && tag == "Znajdzka")
        {
            eqSystem.GetComponent<EqSystem>().PickupItem(item);
            Destroy(gameObject);
        }
        if (item.tag == "Powerbank")
        {
            player.GetComponentInChildren<Flashlight>().ChargeUp(100);
        }
        
    }

}
