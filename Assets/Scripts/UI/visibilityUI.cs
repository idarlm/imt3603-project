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

        if(illuminationSum >= 1)
        {
            SetVisibleImage();
            visibilityImage.color = visibilityImage.color.WithAlpha(Mathf.Lerp(0.5f, 1.0f, illuminationSum));
        } else
        {
            SetNotVisibleImage();
            visibilityImage.color = visibilityImage.color.WithAlpha(Mathf.Lerp(1.0f, 0.5f, illuminationSum));
        }
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
