using Illumination;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class VisibilityUI : MonoBehaviour
{
    private static float stamina; // Variable representing how much stamina the player has
    private static float _previousIllumination; // Variable to store previous illumination
    private float illuminationSum; // Variable to sum the illumination of different body parts
    private PlayerIllumination playerIllumination; // Reference to the player's illumination data
    [SerializeField] PlayerIlluminationController playerIlluminationController; // Reference to the player's illumination controller
    [SerializeField] Image visibilityImage; // UI image representing visibility
    [SerializeField] Slider staminaBar; // UI slider representing stamina bar


    private void Update()
    {

        // Get the player's illumination data during each frame
        playerIllumination = playerIlluminationController.GetIllumination();
        

        // Recalculate the sum of illumination from different body parts
        illuminationSum = (playerIllumination.LeftHandIllumination + playerIllumination.RightHandIllumination +
            playerIllumination.HeadIllumination + playerIllumination.ChestIllumination) / 0.5f;

        // Lerping the illuminationSum, using deltaTime for smooth transition
        illuminationSum = Mathf.Lerp(_previousIllumination, illuminationSum, Time.deltaTime * 5f);

        // Adjust the alpha of the UI image based on the player's illumination level
        visibilityImage.color = visibilityImage.color.WithAlpha(illuminationSum);

        // Lerping the stamina bar based on the stamina timer
        staminaBar.value = Mathf.Lerp(0f, 1f, stamina);

        // Updating previous illumination
        _previousIllumination = illuminationSum;
    }

    /*
     *  Function for updating the staminaTimer
     */
    public static void StaminaTimer(float stamina)
    {
        VisibilityUI.stamina = stamina;
    }
}
