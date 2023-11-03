using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackDetector : MonoBehaviour
{
    // Start is called before the first frame update

    public Action<float> OnPlayerOverlap; 
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.GameObject().name == "Player")
        {
            OnPlayerOverlap?.Invoke(0f);
        }
        
    }
}
