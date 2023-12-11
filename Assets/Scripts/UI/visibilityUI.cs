using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisibilityUI : MonoBehaviour
{
    private bool visible = true;
    [SerializeField] Image visibilityImage;
    [SerializeField] Sprite playerVisible;
    [SerializeField] Sprite playerNotVisible;


    private void Start()
    {
        ChangeImage();
    }

    
    private void Update()
    {
        ChangeImage();
    }

    private void ChangeImage()
    {
        if (visible)
        {
            visibilityImage.sprite = playerVisible;
        }
        else
        {
            visibilityImage.sprite = playerNotVisible;
        }
    }
}
