using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewElevatorController : MonoBehaviour
{
    public Transform playerCamera;
    public float maxDistance = 100f;
    private bool up = false, down = false;
    public Animator elevatorAnim;
    public Animator elevatorDoorsAnim, elevatorFrameUpAnim, elevatorFrameDownAnim;
    public AudioSource buttonSound;
    public AudioSource elevatorSound;
    private float timer = 3.5f;

    private GameObject elevator, elevatorFrameUp, elevatorFrameDown;
    private GameObject buttonUp;
    private GameObject buttonDown;
    private GameObject elevatorCallUp, elevatorCallDown, upCollider, downCollider;

    // Start is called before the first frame update
    void Start()
    {
        elevator = GameObject.FindWithTag("Elevator").GetComponentInChildren<GameObject>();
        elevatorFrameUp = GameObject.FindWithTag("ElFrameUp").GetComponentInChildren<GameObject>();
        elevatorFrameDown = GameObject.FindWithTag("ElFrameDown").GetComponentInChildren<GameObject>();
        //elevatorAnim = elevator.GetComponent<Animator>();

        //elevatorAnim = elevator.GetComponent<Animator>(); 
        buttonUp = GameObject.FindWithTag("ElevatorUp").GetComponentInChildren<GameObject>();
        buttonDown = GameObject.FindWithTag("ElevatorDown").GetComponentInChildren<GameObject>();
        elevatorCallUp = GameObject.FindWithTag("ElFrameUpKey").GetComponentInChildren<GameObject>();
        elevatorCallDown = GameObject.FindWithTag("ElFrameDownKey").GetComponentInChildren<GameObject>();

        //upCollider = GameObject.FindWithTag("UpCollider").GetComponentInChildren<GameObject>();
        //downCollider = GameObject.FindWithTag("DownCollider").GetComponentInChildren<GameObject>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Pressed();
            
            
        }
        timer -= Time.deltaTime;
        //print("Czasownik: "+timer);
        if (timer <= 0.0f)
        {
            up = false;
            down = false;
            elevatorSound.Stop();
            elevatorDoorsAnim.SetBool("Opened", true);
        }
        if(timer > 0.0f)
        {
            elevatorDoorsAnim.SetBool("Opened", false);
        }

    }

    void Pressed()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit buttonHit;
        if (Physics.Raycast(ray, out buttonHit, maxDistance))
        {
            timer = 4.5f;

            if (buttonHit.collider.tag == "ElevatorUp")
            {
                // Debug.Log("Trafiono przyciskUp");
                buttonSound.Play();

                if (timer > 0.0f)
                {
                    elevatorSound.Play();
                    print("Playing elevator sound");
                    //elevatorFrameDownAnim.SetBool("Opened", false);
                    
                }
                if (timer <= 0.0f)
                {
                    elevatorSound.Stop();
                    //elevatorFrameUpAnim.SetBool("Opened", true);
                    
                }
               
                //elevatorAnim = buttonHit.collider.GetComponentInParent<Animator>();
                if (up)
                    up = false;
                else
                    up = true;
                elevatorAnim.SetBool("Up", up);
                elevatorAnim.SetBool("Down", false);

            }
            if (buttonHit.collider.tag == "ElevatorDown")
            {
                // Debug.Log("Trafiono przyciskDown");
                buttonSound.Play();

                if (timer > 0.0f)
                {
                    elevatorSound.Play();
                    print("Playing elevator sound");
                }
                if (timer <= 0.0f)
                {
                    elevatorSound.Stop();
                }
                //elevatorAnim = buttonHit.collider.GetComponentInParent<Animator>();
                if (down)
                    down = false;
                else
                    down = true;
                elevatorAnim.SetBool("Down", down);
                elevatorAnim.SetBool("Up", false);

            }
            if (buttonHit.collider.tag == "ElFrameUpKey")
            {
                // Debug.Log("Trafiono przyciskUp na obudowie");
                buttonSound.Play();

                if (timer > 0.0f)
                {
                    elevatorSound.Play();
                    print("Playing elevator sound");
                }
                if (timer <= 0.0f)
                {
                    elevatorSound.Stop();
                }

                //elevatorAnim = buttonHit.collider.GetComponentInParent<Animator>();
                if (up)
                    up = false;
                else
                    up = true;
                elevatorAnim.SetBool("Up", up);
                elevatorAnim.SetBool("Down", false);

            }
            if (buttonHit.collider.tag == "ElFrameDownKey")
            {
                // Debug.Log("Trafiono przyciskDown na obudowie");
                buttonSound.Play();

                if (timer > 0.0f)
                {
                    elevatorSound.Play();
                    print("Playing elevator sound");
                }
                if (timer <= 0.0f)
                {
                    elevatorSound.Stop();
                }
                //elevatorAnim = buttonHit.collider.GetComponentInParent<Animator>();
                if (down)
                    down = false;
                else
                    down = true;
                elevatorAnim.SetBool("Down", down);
                elevatorAnim.SetBool("Up", false);

            }


            //Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.green);

        }
    }
    
}
