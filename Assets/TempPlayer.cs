using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    void Start()
    {
        cameraTransform.GetComponent<FollowerCamera>();
    }

    public Transform cameraTransform;
    [SerializeField] private float playerSpeed = 5.0f;

    private void Update()
    {
        var forward = transform.position - cameraTransform.position;
        forward.y = 0;
      
        var right = Vector3.Cross(forward, Vector3.down);
        var inputVector = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            inputVector += forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVector -= forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputVector -= right;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector += right;
        }

        var movementVector = inputVector.normalized;

        this.transform.position += playerSpeed * Time.deltaTime * movementVector;
    }
}