using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeWalk : MonoBehaviour
{
    private bool enable = true;
    public float aplituda = 0.015f;
    public float czestotliwosc = 10.0f;
    public new Transform camera;
    public Transform cameraHolder;
    private float toggleSpeed=5.0f;
    private Vector3 startPos;
    private CharacterController controller;
    //private bool isGrounded;

    // Start is called before the first frame update
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        startPos = camera.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!enable) return;

        CheckMotion();
        
        camera.LookAt(FocusTarget());
    }

    private Vector3 FocusTarget()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + cameraHolder.localPosition.y, transform.position.z);
        pos += cameraHolder.forward * 15.0f;
        return pos;
    }

    private void PlayMotion(Vector3 motion)
    {
        //Debug.Log("odtwarzanie chodzenia");
        camera.localPosition += motion;
    }
    private void CheckMotion()
    {
        //Debug.Log("sprawdzanie czy sie porusza");

        float speed = controller.velocity.magnitude;

        ResetPosition();

        if (speed < toggleSpeed) return;
        if (controller.isGrounded == false)
        {
            //Debug.Log("nie jest grounded");
            return;
        }

        PlayMotion(FootStepMotion());
    }

    private Vector3 FootStepMotion()
    {
        //Debug.Log("poruszanie kamerą");

        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * czestotliwosc) * aplituda;
        pos.x += Mathf.Cos(Time.time * czestotliwosc/2) * aplituda*2;

        return pos;
    }

    private void ResetPosition()
    {
        if (camera.localPosition == startPos) return;
        camera.localPosition = Vector3.Lerp(camera.localPosition, startPos, 1 * Time.deltaTime);
    }

}
