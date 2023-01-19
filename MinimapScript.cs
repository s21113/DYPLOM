using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapScript : MonoBehaviour
{
    public Transform player;

    void Update()
    {
        var yy = transform.position.y;
        Vector3 newPosition = new Vector3
        {
            x = player.position.x,
            y = yy,
            z = player.position.z
        };
        transform.position = newPosition;

        //transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
    }
}
