using UnityEngine.SceneManagement;

public static class Loader
{
    // Enum representing different scenes in the game
    public enum Scene
    {
        MainMenuScene,
        DemoScene, //The actual gamelevel
        LoadingScene
    }


    // Private static variable to store the target scene to load
    private static Scene targetScene;


    /*
     *  Function to initiate the loading of a specified scene
     */
    public static void Load(Scene targetScene)
    {
        // Set the target scene
        Loader.targetScene = targetScene;

        // Load the loading scene
        SceneManager.LoadScene(Scene.LoadingScene.ToString());

    }
    
    // Callback function called when the loading scene has finished loading
    public static void LoaderCallback()
    {
        // Load the target scene
        SceneManager.LoadScene(targetScene.ToString());
    }
}
