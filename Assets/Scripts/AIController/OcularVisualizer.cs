using System.Collections;
using System.Collections.Generic;
using AIController;
using Unity.VisualScripting;
using UnityEngine;

public class OcularVisualizer : MonoBehaviour
{
    private Vector3 axis;
    public float FOV;

    void Start()
    {
        var forward = transform.forward;
        axis = new Vector3(forward.x, forward.y, forward.z);
    }
    void Update()
    {
        for (int j = 0; j < 36; j++)
        {
            for (int i = 0; i < 36; i++)
            {
                axis = Quaternion.AngleAxis(i*10, 
                    Quaternion.AngleAxis(j * 10, transform.forward) * transform.up
                    ) * transform.forward;
                Debug.DrawLine(
                    transform.position, 
                    transform.position + axis * 5,
                    Color.red* OcularSimulator.AttenuateByFOV(
                        transform, 
                        axis, 
                        FOV, 
                        1)
                );
            }
        }
        
    }
}

