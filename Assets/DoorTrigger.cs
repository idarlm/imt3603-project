using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    bool open = false;

    private void Start()
    {
        open = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Capsule" && open == false)
        {
            Debug.Log("Hit: " + other.transform.name);
            GameObject.Find("Door").transform.position += new Vector3(0, 4, 0);
            this.transform.position += new Vector3(0, -1, 0);

            open = true;
        }
    }
}
