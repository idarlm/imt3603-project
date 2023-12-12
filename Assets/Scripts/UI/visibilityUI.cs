using Illumination;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class VisibilityUI : MonoBehaviour
{
    private float illuminationSum;
    private PlayerIllumination playerIllumination;
    [SerializeField] PlayerIlluminationController playerIlluminationController;
    [SerializeField] Image visibilityImage;


    private void Start()
    {
        playerIllumination = playerIlluminationController.GetIllumination();
        illuminationSum = (playerIllumination.LeftHandIllumination + playerIllumination.RightHandIllumination +
            playerIllumination.HeadIllumination + playerIllumination.ChestIllumination) / 0.5f;
    }

    private void Update()
    {
        playerIllumination = playerIlluminationController.GetIllumination();
        illuminationSum = (playerIllumination.LeftHandIllumination + playerIllumination.RightHandIllumination +
            playerIllumination.HeadIllumination + playerIllumination.ChestIllumination) / 0.5f;

       visibilityImage.color = visibilityImage.color.WithAlpha(illuminationSum);

    }
}
