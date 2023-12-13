using TMPro;
using UnityEngine;

public class InteractionTextUI : MonoBehaviour
{
    private Camera mainCamera; // Main camera in the scene
    [SerializeField] private TextMeshProUGUI interactText; // TextMeshProUGUI component for interaction text
    private static TextMeshProUGUI instance; // Static instance of the interactText component


    private void Start()
    {
        // Get the main camera in the scene
        mainCamera = Camera.main;

        // Assign the interactText component to the static instance
        instance = interactText;

        // Set the initial text to an empty string
        interactText.text = " ";
    }


    private void LateUpdate()
    {
        // Get the rotation of the main camera
        var rotation = mainCamera.transform.rotation;

        // Make the UI element look at the camera while maintaining its direction
        transform.LookAt(transform.position + rotation * Vector3.forward,
            rotation * Vector3.up);
    }

    /*
     *  Function to display the interaction UI text
     */
    public static void ShowUI()
    {
        // Check if the instance is not null
        if (instance != null)
        {
            // Update the text to the interaction prompt
            instance.text = "Press [E] to interact";
        }
    }


    /*
     *  Function to close the interaction UI text
     */
    public static void CloseUI()
    {
        // Check if the instance is not null
        if (instance != null)
        {
            // Set the text to an empty string to hide the interaction prompt
            instance.text = " ";
        }
            
    }
}
