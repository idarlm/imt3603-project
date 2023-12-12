using Illumination;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class VisibilityUI : MonoBehaviour
{
    private float staminaTimer; //test variable
    private float illuminationSum; //variable to sum all the illuminated bodyparts
    private PlayerIllumination playerIllumination; //variable to use the reference to the players illumination
    [SerializeField] PlayerIlluminationController playerIlluminationController; //The reference to the players illumination
    [SerializeField] Image visibilityImage; //visibility UI image
    [SerializeField] Slider staminaBar; //staminabar UI element


    private void Start()
    {
        staminaTimer = 1f; //test variable

        //Getting the player illumination at the start of the game
        playerIllumination = playerIlluminationController.GetIllumination();

        //Calculating the sum of the different illuminated bodyparts
        illuminationSum = (playerIllumination.LeftHandIllumination + playerIllumination.RightHandIllumination +
            playerIllumination.HeadIllumination + playerIllumination.ChestIllumination) / 0.5f;
    }

    private void Update()
    {
        staminaTimer -= 0.001f; //test variable

        //Getting the players illumination
        playerIllumination = playerIlluminationController.GetIllumination();

        //Calculating the sum of the different illuminated bodyparts
        illuminationSum = (playerIllumination.LeftHandIllumination + playerIllumination.RightHandIllumination +
            playerIllumination.HeadIllumination + playerIllumination.ChestIllumination) / 0.5f;

        //Changing the alpha of the image based on how illuminated the player is
        visibilityImage.color = visibilityImage.color.WithAlpha(illuminationSum);

        //Lerping the stamina bar based on the stamina timer
        staminaBar.value = Mathf.Lerp(0f, 1f, staminaTimer);
    }
}
