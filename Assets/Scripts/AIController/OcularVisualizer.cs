using System.Collections;
using System.Collections.Generic;
using AIController;
using Unity.VisualScripting;
using UnityEngine;

public class OcularVisualizer : MonoBehaviour
{
    private Vector3 horizontalDirection;
    private Vector3 verticalDirection;
    public float FOV;

    void Start()
    {
        var forward = transform.forward;
        horizontalDirection = new Vector3(forward.x, forward.y, forward.z);
    }
    void Update()
    {
        for (int i = 0; i < 360; i++)
        {
            horizontalDirection = Quaternion.AngleAxis(i, Vector3.up) * transform.forward;
            verticalDirection = Quaternion.AngleAxis(i, Vector3.right) * transform.forward;
            Debug.DrawLine(
                transform.position, 
                transform.position + horizontalDirection * 5,
                Color.red* OcularSimulator.AttenuateByFOV(
                    transform.forward, 
                    horizontalDirection, 
                    FOV, 
                    1)
            );
            Debug.DrawLine(
                transform.position, 
                transform.position + verticalDirection * 5,
                Color.red* OcularSimulator.AttenuateByFOV(
                    transform.forward, 
                    verticalDirection, 
                    FOV, 
                    1)
            );
        }
    }
}

