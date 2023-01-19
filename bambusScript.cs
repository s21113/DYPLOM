using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bambusScript : MonoBehaviour
{
    //[SerializeField] private BoxCollider colliderOne;
   // [SerializeField] private BoxCollider colliderTwo;
   // [SerializeField] private CapsuleCollider capsule;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 15);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
        
    }
}
