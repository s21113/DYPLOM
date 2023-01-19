using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    Animator animator;
    float velocityZ = 0.0f;
    float velocityX = 0.0f;
    public float acceleration = 2.0f;
    public float deceleration = 2.0f;
    public float maxWalkVel = 0.5f;
    public float maxRunVel = 2.0f;
    private bool isCrouching = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool forwardPressed = Input.GetKey("w");
        bool backwardPressed = Input.GetKey("s");
        bool leftPressed = Input.GetKey("a");
        bool rightPressed = Input.GetKey("d");
        bool runPressed = Input.GetKey("left shift");
        bool jumpPressed = Input.GetKey("space");
        
        float currentMaxVel = runPressed ? maxRunVel : maxWalkVel;

        if(Input.GetKeyDown("left ctrl"))
        {
            isCrouching = false;//!isCrouching;
        }
        //jesli wcisniete w zwieksz velocityZ
        if (forwardPressed && velocityZ < currentMaxVel)
        {
            velocityZ += Time.deltaTime * acceleration;
        }
        //jesli wcisniete s zmniejsz velocityZ
        if (backwardPressed && velocityZ > -currentMaxVel)
        {
            velocityZ -= Time.deltaTime * acceleration;
        }
        
        //jesli wcisniete a zmniejsz velocityX
        if (leftPressed && velocityX > -currentMaxVel)
        {
            velocityX -= Time.deltaTime * acceleration;
        }
        //jesli wcisniete d zwieksz velocityX
        if (rightPressed && velocityX < currentMaxVel)
        {
            velocityX += Time.deltaTime * acceleration;
        }
        //zmniejsz velocityZ
        if(!forwardPressed && velocityZ > 0.0f)
        {
            velocityZ -= Time.deltaTime * deceleration;
        }
        //zresetuj velocityZ
        if (!backwardPressed && velocityZ < 0.0f)
        {
            velocityZ += Time.deltaTime * deceleration;
        }
        if (!forwardPressed && !backwardPressed && velocityZ != 0.0f && (velocityZ > -0.02f && velocityZ < 0.02f))
        {
            velocityZ = 0.0f;
        }
        //zwieksz velocityX jesli a nie wcisniete i velocityX < 0
        if (!leftPressed && velocityX < 0.0f)
        {
            velocityX += Time.deltaTime * deceleration;
        }
        //zmiejsz elocityX jesli d nie wcisniete i velocityX > 0
        if (!rightPressed && velocityX > 0.0f)
        {
            velocityX -= Time.deltaTime * deceleration;
        }
        //
        if(!leftPressed && !rightPressed && velocityX !=0.0f && (velocityX > -0.02f && velocityX < 0.02f))
        {
            velocityX = 0.0f;
        }
        //do przodu bieganie
        if(forwardPressed && runPressed && velocityZ > currentMaxVel)
        {
            velocityZ = currentMaxVel;
            
        }
        else if(forwardPressed && velocityZ > currentMaxVel)
        {
            velocityZ -= Time.deltaTime * deceleration;
            
            if(velocityZ > currentMaxVel && velocityZ < (currentMaxVel + 0.05f))
            {
                velocityZ = currentMaxVel;
            }
        }
        else if(forwardPressed && velocityZ < currentMaxVel && velocityZ > (currentMaxVel - 0.05f))
        {
            velocityZ = currentMaxVel;
        }
        //do tylu bieganie
        if (backwardPressed && runPressed && velocityZ < -currentMaxVel)
        {
            velocityZ = -currentMaxVel;
            
        }
        else if (backwardPressed && velocityZ < -currentMaxVel)
        {
            velocityZ += Time.deltaTime * deceleration;
            
            if (velocityZ < -currentMaxVel && velocityZ > (-currentMaxVel + 0.05f))
            {
                velocityZ = -currentMaxVel;
            }
        }
        else if (backwardPressed && velocityZ > -currentMaxVel && velocityZ < (-currentMaxVel - 0.05f))
        {
            velocityZ = -currentMaxVel;
        }
        //w lewo bieganie
        if (leftPressed && runPressed && velocityX < -currentMaxVel)
        {
            velocityX = -currentMaxVel;
            
        }
        else if (leftPressed && velocityX < -currentMaxVel)
        {
            velocityX += Time.deltaTime * deceleration;

            if (velocityX < -currentMaxVel && velocityX > (-currentMaxVel + 0.05f))
            {
                velocityX = -currentMaxVel;
            }
        }
        else if (leftPressed && velocityX > -currentMaxVel && velocityX < (-currentMaxVel - 0.05f))
        {
            velocityX = -currentMaxVel;
        }
        //w prawo bieganie
        if (rightPressed && runPressed && velocityX > currentMaxVel)
        {
            velocityX = currentMaxVel;
            
        }
        else if (rightPressed && velocityX > currentMaxVel)
        {
            velocityX -= Time.deltaTime * deceleration;
            
            if (velocityX > currentMaxVel && velocityX < (currentMaxVel + 0.05f))
            {
                velocityX = currentMaxVel;
            }
        }
        else if (rightPressed && velocityX < currentMaxVel && velocityX > (currentMaxVel - 0.05f))
        {
            velocityX = currentMaxVel;
        }

        animator.SetFloat("velocityZ", velocityZ);
        animator.SetFloat("velocityX", velocityX);
        animator.SetBool("isCrouching", isCrouching);
        animator.SetBool("isJumping", jumpPressed);

    }
}
