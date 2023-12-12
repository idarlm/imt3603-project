using Illumination;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisibilityUI : MonoBehaviour
{
    // private bool visible = true;
    private Vector4 playerIllumination;
    private Transform leftHand;
    [SerializeField] Image visibilityImage;
    [SerializeField] Sprite playerVisible;
    [SerializeField] Sprite playerNotVisible;


    private void Start()
    {
       // playerIllumination;
        //leftHand = PlayerIlluminationController.leftHand;
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
