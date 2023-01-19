using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private bool canUse = true;
    public float speed = 14f;
    public float amount = 0.5f;
    private float defaultYPosi = 0;
    private float timer;
    private Camera playerCamera;
    private CharacterController controller;

    // Start is called before the first frame update
    void Awake()
    {
        playerCamera = GetComponentInChildren<Camera>();
        controller = GetComponent<CharacterController>();
        defaultYPosi = playerCamera.transform.localPosition.y;

    }

    // Update is called once per frame
    void Update()
    {
        if (canUse)
        {
            Handle();
        }
    }

    private void Handle()
    {
        if (!controller.isGrounded) return;

        //if(Mathf.Abs())
    }
}
