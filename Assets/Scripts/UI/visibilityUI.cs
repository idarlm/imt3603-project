using Illumination;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class VisibilityUI : MonoBehaviour
{
    private PlayerIllumination playerIllumination;
    private float illuminationSum;
    [SerializeField] PlayerIlluminationController playerIlluminationController;
    [SerializeField] Image visibilityImage;
    [SerializeField] Sprite playerVisible;
    [SerializeField] Sprite playerNotVisible;


    private void Start()
    {
        playerIllumination = playerIlluminationController.GetIllumination();
        illuminationSum = (playerIllumination.LeftHandIllumination + playerIllumination.RightHandIllumination +
            playerIllumination.HeadIllumination + playerIllumination.ChestIllumination) * 2;
    }

    private void Update()
    {
        playerIllumination = playerIlluminationController.GetIllumination();
        illuminationSum = (playerIllumination.LeftHandIllumination + playerIllumination.RightHandIllumination +
            playerIllumination.HeadIllumination + playerIllumination.ChestIllumination) * 2;

        visibilityImage.color = visibilityImage.color.WithAlpha(Mathf.Clamp01(illuminationSum));
    }

    public void SetVisibleImage()
    {
        visibilityImage.sprite = playerVisible;
    }

    public void SetNotVisibleImage()
    {
        visibilityImage.sprite = playerNotVisible;
    }
}
