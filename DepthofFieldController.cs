using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DepthofFieldController : MonoBehaviour
{
    Ray raycast;
    RaycastHit hit;
    //bool isHit;
    float hitDistance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        raycast = new Ray(transform.position, transform.forward * 100);
        //isHit = false;
        if(Physics.Raycast(raycast, out hit, 100f))
        {
            //isHit = true;
            hitDistance = Vector3.Distance(transform.position, hit.point);

        }
        else
        {
            if (hitDistance < 100f)
            {
                hitDistance++;
            }
        }
        SetFocus();
    }
    void SetFocus()
    {
       
    }
}
