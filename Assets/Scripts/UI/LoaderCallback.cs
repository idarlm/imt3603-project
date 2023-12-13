using UnityEngine;

public class LoaderCallback : MonoBehaviour
{
    // Bool variable to track whether the Update method is called for the first time
    private bool isFirstUpdate = true;

    private void Update()
    {
        // Check if this is the first time Update is called
        if (isFirstUpdate)
        {
            // Set isFirstUpdate to false to indicate that Update has been called
            isFirstUpdate = false;

            // Call the LoaderCallback function from the Loader class
            Loader.LoaderCallback();
        }
    }

}
