using System;
using UnityEngine;

public class SimpleTrigger : PuzzleTrigger
{

    [SerializeField] PlayerPuzzle player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player") //other.gameObject == player)
        {
            FireTriggered(this, EventArgs.Empty);
            Debug.Log("SimpleTrigger fired");
        } 
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player") { 
            FireTriggeredFinished(this, EventArgs.Empty);
            Debug.Log("SimpleTrigger done firing");
        }
    }

}
