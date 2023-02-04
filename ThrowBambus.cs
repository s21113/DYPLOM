using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBambus : MonoBehaviour
{

    public GameObject bambus;
    public GameObject reppelant;
    private GameObject player;
    private Rigidbody rb;
    private Transform flashlight;
    private int force;
    private bool canThrow1;
    private bool canThrow2;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerBody");

        flashlight = transform.Find("flashlight");
        //flashlight = GameObject.Find("Sphere").transform;
        //ball = GameObject.Find("Bambus");
        //reppelant = GameObject.Find("OdstraszaczPrefab");
        force = 0;
        canThrow1 = true;
        canThrow2 = true;

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("b") && canThrow1)
        {

            StartCoroutine("charge");

            //Debug.Log(flashlight.transform.rotation);
            //Debug.Log(flashlight.transform.position);
            //newBall.transform.position = new Vector3(flashlight.transform.position.x, flashlight.transform.position.y, flashlight.transform.position.z);
            //newBall.transform.rotation = new Quaternion(flashlight.transform.rotation.x, flashlight.transform.rotation.y, flashlight.transform.rotation.z, flashlight.transform.rotation.w);
            //rb.AddForce(transform.up * 100);

        }
        if (Input.GetKeyUp("b") && canThrow1)
        {
            StopCoroutine("charge");
            GameObject newBall = Instantiate(bambus, flashlight.position, flashlight.rotation);
            rb = newBall.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * force);
            force = 0;
            StartCoroutine("wait1");


        }
        if (Input.GetKeyDown("g") && canThrow2)
        {
            GameObject newreppelent = Instantiate(reppelant, flashlight.position, flashlight.rotation);
            //newreppelent.transform.position = new Vector3(flashlight.transform.position.x, flashlight.transform.position.y, flashlight.transform.position.z);
            newreppelent.transform.Rotate(-90, 0, 0);
            rb = newreppelent.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 300);
            StartCoroutine("wait2");

        }
    }

    IEnumerator charge()
    {
        while (true)
        {
            force = force + 130;
            yield return new WaitForSeconds(.03f);
        }
    }

    IEnumerator wait1()
    {
        canThrow1 = false;
        yield return new WaitForSeconds(30);
        canThrow1 = true;
    }

    IEnumerator wait2()
    {
        canThrow2 = false;
        yield return new WaitForSeconds(45);
        canThrow2 = true;
    }

}
