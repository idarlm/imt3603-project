using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton; // Button component to start game
    [SerializeField] private Button quitButton; // Button component to quit game

    private void Awake()
    {
        // Add a listener to the play button that triggers when the button is clicked
        playButton.onClick.AddListener(() =>
        {
            // Call the Load function from the Loader class to load the game scene
            Loader.Load(Loader.Scene.DemoScene);
        });

        // Add a listener to the quit button that triggers when the button is clicked
        quitButton.onClick.AddListener(() =>
        {
            // Quit the application
            Application.Quit();
        });
    }


}
