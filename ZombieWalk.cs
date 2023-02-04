using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ZombieWalk : MonoBehaviour
{
    private Rigidbody rb;
    public GameObject WP1;
    public GameObject WP2;
    private GameObject player;
    bool WP1cmp = true;
    bool WP2cmp = false;
    private bool isInRange;
    Animator animator;
    NavMeshAgent agencik;
    private int hpPoints = 1;
    private bool freeze = false;
    private bool goingToPlayer = false;
    private bool spinning = false;
    private float speed;
    // private Collider coll1;
    // private Collider coll2;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        animator.SetBool("isInRange", isInRange);
        player = GameObject.FindGameObjectWithTag("PlayerBody");
        agencik = GetComponent<NavMeshAgent>();

        // GameObject x1 = GameObject.FindGameObjectWithTag("WP1");
        // GameObject x2 = GameObject.FindGameObjectWithTag("WP2");
        // coll1 = x1.GetComponent<Collider>();
        // coll2 = x2.GetComponent<Collider>();

        // coll2.enabled = false;
        speed = UnityEngine.Random.Range(2.5f, 6.5f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 myVector = new Vector3(0.0f, 0.0f, 5.0f);
        rb.velocity = myVector * 125.0f;

        //  Debug.Log(coll2);
    }
   

    private void FixedUpdate()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        Vector3 targetPostition = new Vector3(player.transform.position.x,
                                      transform.position.y,
                                      player.transform.position.z);

        if (distance <= 10f && Math.Abs(transform.position.y - player.transform.position.y) <= 1.5f)
        {
            goingToPlayer = true;
            agencik.speed = 8f;
            agencik.SetDestination(targetPostition);
            //transform.position = Vector3.MoveTowards(transform.position, targetPostition, speed * 2 * Time.deltaTime);
            //transform.LookAt(targetPostition);
        }
        else
        {
            goingToPlayer = false;
            agencik.speed = speed;
        }

        if (distance < 2.8f && !freeze)
        {
            isInRange = true;
            animator.SetBool("isInRange", isInRange);
            goingToPlayer = false;

            if(!spinning){
                StartCoroutine(Hit());
            }
            
        }
        else if (!freeze)
        {
            isInRange = false;
            animator.SetBool("isInRange", isInRange);
            if (WP1cmp && !goingToPlayer)
            {
                agencik.SetDestination(WP1.transform.position);
                //transform.position = Vector3.MoveTowards(transform.position, WP1.transform.position, speed * Time.deltaTime);
                transform.LookAt(WP1.transform);
            }
            else if (WP2cmp && !goingToPlayer)
            {
                agencik.SetDestination(WP2.transform.position);
                //transform.position = Vector3.MoveTowards(transform.position, WP2.transform.position, speed * Time.deltaTime);
                transform.LookAt(WP2.transform);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject == WP1 && !WP2cmp)
        {
            // coll2.enabled = true;

            WP1cmp = !WP1cmp;
            WP2cmp = !WP2cmp;
            transform.Rotate(new Vector3(0, 90, 0), Space.World);
            // coll1.enabled = false;
        }
        if (other.gameObject == WP2 && !WP1cmp)
        {
            // coll1.enabled = true;
            
            WP2cmp = !WP2cmp;
            WP1cmp = !WP1cmp;
            transform.Rotate(new Vector3(0, -90, 0), Space.World);
            // coll2.enabled = false;
            
        }
        
        
        if (other.gameObject.CompareTag("Bambus"))
        {
            hpPoints -= 1;
            if (hpPoints <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void setIsInRangeFalse()
    {
        freeze = false;
        isInRange = false;
        animator.SetBool("isInRange", isInRange);
    }

    private void OnTriggerStay(Collider other)
    {
        // if(other.gameObject.tag != "Untagged" && other.gameObject.tag != "Jumpscare" && other.gameObject.tag == "reppelant" && other.gameObject.tag != "PlayerBody" ){
        //     Debug.Log(other.gameObject.tag);
        // }
        
        if (other.gameObject.CompareTag("reppelant"))
        {
            //Debug.Log("444444444");
           
            freeze = true;
            agencik.isStopped = true;
            isInRange = true;
            animator.SetBool("isInRange", isInRange);

        }else if (spinning) {

            //Debug.Log("5555555");
            
            freeze = true;
            agencik.isStopped = true;
            isInRange = true;
            animator.SetBool("isInRange", isInRange);

        }else{
            
            freeze = false;
            agencik.isStopped = false;
            isInRange = false;
            animator.SetBool("isInRange", isInRange);
        }
    }

    IEnumerator Hit (){
        spinning = true;
        //Debug.Log("JEB");

        //Debug.Log("STOP");
        for (int i=0; i<300; i++){
            transform.Rotate(new Vector3(0, 8, 0), Space.World);
            yield return new WaitForSeconds(0.01f);
        }
      

        //Debug.Log("RETURN");

        spinning = false;
        StopCoroutine(Hit());

    }



}
