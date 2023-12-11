using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionTextUI : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField] private TextMeshProUGUI interactText;

    
    private void Start()
    {
        mainCamera = Camera.main;
        interactText.text = " ";
    }


    private void LateUpdate()
    {
        var rotation = mainCamera.transform.rotation;
        transform.LookAt(transform.position + rotation * Vector3.forward,
            rotation * Vector3.up);
    }


    public void ShowUI()
    {
        interactText.text = "Press [E] to interact";
    }

    public void CloseUI()
    {
        interactText.text = " ";
    }
}
