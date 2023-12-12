using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionTextUI : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField] private TextMeshProUGUI interactText;
    private static TextMeshProUGUI instance;

    
    private void Start()
    {
        mainCamera = Camera.main;
        instance = interactText;
        interactText.text = " ";
    }


    private void LateUpdate()
    {
        var rotation = mainCamera.transform.rotation;
        transform.LookAt(transform.position + rotation * Vector3.forward,
            rotation * Vector3.up);
    }


    public static void ShowUI()
    {
        if (instance != null)
            instance.text = "Press [E] to interact";
    }

    public static void CloseUI()
    {
        if (instance != null)
            instance.text = " ";
    }
}
