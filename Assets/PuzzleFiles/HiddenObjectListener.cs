using System;
using UnityEngine;

public class HiddenObjectListener : PuzzleTrigger
{
    public PuzzleTrigger trigger; 
    private Vector3 startPos;
    private bool isMoved = false;
    private bool isFetched = false;
    [SerializeField] Vector3 offset; // How much the object should be moved
    [SerializeField] GameObject key; // Key object to unlock a door

    private void Start()
    {
        // Setting the items start position
        startPos = transform.position;

        // Subscribing to the event
        trigger.Triggered += moveObject;
    }

    private void Update()
    {
        
        if (isMoved) // Checking if the object is moved
        {

            // Lerping its position to move it
            transform.position = Vector3.Lerp(transform.position, startPos + offset, Time.deltaTime);
        }

        if (isFetched) // Checking if the key object is picked up
        {
            key.SetActive(false); // Disabling the object
            FireTriggered(this, EventArgs.Empty); // Firing an event to indicate that the player has the key
        }


    }

    /*
     *  Function that updates the moved and fetched bools based on interaction.
     */
    void moveObject(object obj, EventArgs args)
    {

        if (!isMoved) // Checking if it has not already been moved
        {
            isMoved = true;
        }
        else
        {
            isFetched = true;
        }

    }

}
