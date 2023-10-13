using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterTrigger : PuzzleTrigger
{
    public int threshold = 10;
    private int counter;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            counter++;

            if (counter == threshold)
            {
                FireTriggered(this, System.EventArgs.Empty);
                Debug.Log("CounterTrigger fired");
            }
        }
    }
}
