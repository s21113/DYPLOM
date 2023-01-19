using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CichoXD : MonoBehaviour
{
    [SerializeField] private GameObject onigiriPrefab;
    int uses = 3;

    public void DispenseOnigiri()
    {
        Instantiate(onigiriPrefab, transform.position, transform.rotation);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            if (Input.GetKeyDown("e") && uses > 0)
            {
                uses -= 1;
                DispenseOnigiri();
            }

        }
    }

}
