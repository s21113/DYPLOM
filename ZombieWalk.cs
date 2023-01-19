using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieWalk : MonoBehaviour
{
    private Rigidbody rb;
    [Range(0, 8f)] [SerializeField] private float speed = 3f;
    private Vector3 m_Velocity = Vector3.zero;
    public GameObject WP1;
    public GameObject WP2;
    private GameObject player;
    bool WP1cmp = true;
    bool WP2cmp = false;
    private bool isInRange;
    Vector3 vecBgn;
    Animator animator;
    private int hpPoints;
    private bool freze;
    private bool inTrigger;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        hpPoints = 4;
        freze = false;
        isInRange = false;
        animator.SetBool("isInRange", isInRange);
        player = GameObject.FindGameObjectWithTag("PlayerBody");
        inTrigger = false;


    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 targetVelocity = new Vector3 (10 * 10f, rb.velocity.x, rb.velocity.x);
        //rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
        Vector3 myVector = new Vector3(0.0f, 0.0f, 5.0f);
        rb.velocity = myVector * 111.0f;

        //Debug.Log(rb.velocity);
        //rb.velocity = transform.forward * 5f;
        lookat();
    }

    private void lookat()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        Vector3 targetPostition = new Vector3(player.transform.position.x,
                                      transform.position.y,
                                      player.transform.position.z);
        Vector3 moveAround1 = new Vector3(WP1.transform.position.x, WP1.transform.position.y, WP1.transform.position.z);
        Vector3 moveAround2 = new Vector3(WP2.transform.position.x, WP2.transform.position.y, WP2.transform.position.z);

        //bool done = false;

        if (distance < 3 && !freze)
        {
            isInRange = true;
            animator.SetBool("isInRange", isInRange);
            //transform.LookAt(targetPostition);
        }
        else if (!freze)
        {

            isInRange = false;
            animator.SetBool("isInRange", isInRange);
            if (WP1cmp)
            {
                transform.position = Vector3.MoveTowards(transform.position, moveAround1, speed * Time.deltaTime);

            }
            else if (WP2cmp)
            {
                transform.position = Vector3.MoveTowards(transform.position, moveAround2, speed * Time.deltaTime);
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "WP1")
        {
            WP1cmp = !WP1cmp;
            WP2cmp = !WP2cmp;
            transform.Rotate(new Vector3(0, 180, 0), Space.World);
        }
        if (other.gameObject.name == "WP2")
        {
            WP2cmp = !WP2cmp;
            WP1cmp = !WP1cmp;
            transform.Rotate(new Vector3(0, 180, 0), Space.World);
        }
        if (other.gameObject.tag == "Bambus")
        {
            hpPoints = hpPoints - 1;
            if (hpPoints < 0)
            {
                Destroy(gameObject);
            }

        }
        
    }

    public void setIsInRangeFalse(){
        freze = false;
        isInRange = false;
        animator.SetBool("isInRange", isInRange);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "reppelant")
        {
            inTrigger = true;
            freze = true;
            isInRange = true;
            animator.SetBool("isInRange", isInRange);
        }
        

    }
}
