using System;
using System.Collections;
using System.Collections.Generic;
using PlayerMovement;
using Unity.VisualScripting;
using UnityEngine;

public class AttackDetector : MonoBehaviour
{
    public Action<PlayerMovementSystem> OnPlayerOverlap; 
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.GameObject().name == "Player")
        {
            var player = other.GameObject().GetComponent<PlayerMovementSystem>();
            OnPlayerOverlap?.Invoke(player);
        }
        
    }
}
