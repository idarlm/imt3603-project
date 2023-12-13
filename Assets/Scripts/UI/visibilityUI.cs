using Illumination;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class VisibilityUI : MonoBehaviour
{
    private float staminaTimer; // Test variable representing a timer for stamina (temporary)
    private float illuminationSum; // Variable to sum the illumination of different body parts
    private PlayerIllumination playerIllumination; // Reference to the player's illumination data
    [SerializeField] PlayerIlluminationController playerIlluminationController; // Reference to the player's illumination controller
    [SerializeField] Image visibilityImage; // UI image representing visibility
    [SerializeField] Slider staminaBar; // UI slider representing stamina bar


    private void Start()
    {
        staminaTimer = 1f; // Test variable initialization, 1 for full bar

        /*
        // Get the player's illumination at the start of the game
        playerIllumination = playerIlluminationController.GetIllumination();

        // Calculating the sum of the different illuminated bodyparts
        illuminationSum = (playerIllumination.LeftHandIllumination + playerIllumination.RightHandIllumination +
            playerIllumination.HeadIllumination + playerIllumination.ChestIllumination) / 0.5f;
        */
    }

    private void Update()
    {
        staminaTimer -= 0.001f; // Test variable update to drain stamina bar

        // Get the player's illumination data during each frame
        playerIllumination = playerIlluminationController.GetIllumination();

        // Recalculate the sum of illumination from different body parts
        illuminationSum = (playerIllumination.LeftHandIllumination + playerIllumination.RightHandIllumination +
            playerIllumination.HeadIllumination + playerIllumination.ChestIllumination) / 0.5f;

        // Adjust the alpha of the UI image based on the player's illumination level
        visibilityImage.color = visibilityImage.color.WithAlpha(illuminationSum);

        // Lerping the stamina bar based on the stamina timer
        staminaBar.value = Mathf.Lerp(0f, 1f, staminaTimer);
    }
}
