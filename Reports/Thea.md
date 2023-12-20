|Description | My Weight |
|----|----|
|Gameplay video | 10 |
|Code video | 0 |
|Good Code  | 25 |
|Bad Code | 25 |
|Development process | 10 |
|Reflection | 30 |

## Good code

### [TriggerGroup.cs](https://github.com/idarlm/imt3603-project/blob/be708eb4057fefc8854ebd3d6d808fdda2d29a94/Assets/PuzzleFiles/TriggerGroup.cs)
The TriggerGroup.cs script is well-structured, making it easy to comprehend and a crucial component of the puzzle system. It enables the connection of multiple puzzles or puzzle elements, ensuring that an event is only triggered when all associated elements have been successfully completed.
At the beginning of the game, the code subscribes all triggers to an event and initializes a list with as many Booleans set to ‘false’ as there are triggers ([Line 14-16]( https://github.com/idarlm/imt3603-project/blob/be708eb4057fefc8854ebd3d6d808fdda2d29a94/Assets/PuzzleFiles/TriggerGroup.cs#L14-L16)). When a trigger is triggered, the code identifies the corresponding trigger and examines the Boolean value in the activated list at the corresponding index ([Line 26]( https://github.com/idarlm/imt3603-project/blob/be708eb4057fefc8854ebd3d6d808fdda2d29a94/Assets/PuzzleFiles/TriggerGroup.cs#L26)). Depending on this status, the value is updated to either true or false. This mechanism ensures the puzzle element's ability to be both activated and deactivated ([Line 29-32]( https://github.com/idarlm/imt3603-project/blob/be708eb4057fefc8854ebd3d6d808fdda2d29a94/Assets/PuzzleFiles/TriggerGroup.cs#L29-L32)). Consequently, it guarantees that the puzzle is only marked as completed when all its components are activated. Notably, the code includes an efficient check for the activation status of all triggers:


```cs
if (activated.All(t => t)) {
            allActivated = true;
        }
```

Instead of having to loop through the entire list we used the "All" method, which makes the code consise and easy to read. It also stops as soon as one of the items is false saving resources.



### [InteractionTextUI](https://github.com/idarlm/imt3603-project/blob/3e0b2834ba212859e982b9740c4ea8fd88c94f45/Assets/Scripts/UI/interactionTextUI.cs)
The InteractionTextUI script is responsible for handling UI text associated with interactable objects. It is attached to the player, with the text designed to follow the camera's movement. This functionality is implemented within the LateUpdate function, ensuring that the text adapts to the player's position after all updates have occurred. Note that a null check on the mainCamera should have been implemented before setting the rotation and adjusting the position of the UI text in the function below.
```cs
 private void LateUpdate()
    {
        // Get the rotation of the main camera
        var rotation = mainCamera.transform.rotation;

        // Make the UI element look at the camera while maintaining its direction
        transform.LookAt(transform.position + rotation * Vector3.forward,
            rotation * Vector3.up);
    }
 ```
The reference to the text UI is obtained using serialzeField ([Line 7]( https://github.com/idarlm/imt3603-project/blob/3e0b2834ba212859e982b9740c4ea8fd88c94f45/Assets/Scripts/UI/interactionTextUI.cs#L7)), and then assigned to a static variable named "instance” ([Line 8]( https://github.com/idarlm/imt3603-project/blob/3e0b2834ba212859e982b9740c4ea8fd88c94f45/Assets/Scripts/UI/interactionTextUI.cs#L8)). This script also includes two static functions, namely ShowUI ([Line 37-45]( https://github.com/idarlm/imt3603-project/blob/3e0b2834ba212859e982b9740c4ea8fd88c94f45/Assets/Scripts/UI/interactionTextUI.cs#L37-L45)) and CloseUI ([Line 51-50]( https://github.com/idarlm/imt3603-project/blob/3e0b2834ba212859e982b9740c4ea8fd88c94f45/Assets/Scripts/UI/interactionTextUI.cs#L51-L60)), which are called to display and hide the UI text. Declaring these functions as static simplifies their invocation from other scripts, facilitating control over the visibility of the text. Both functions ensure that the instance variable is not null before attempting to update the text, preventing potential errors when the no text UI exists.

Although I consider the overall code good, there are areas that could be refined for further improvement. Specifically, the ShowUI function could benefit from a parameter, allowing dynamic text assignment based on the calling context, with a default string provided in case none is specified. Currently, the text is hardcoded, a practice I consider suboptimal. Additionally, instead of simply toggling between hardcoded text and an empty string, it may be more resource-efficient to activate and deactivate the text as needed. Furthermore, while it may not directly impact code quality, utilizing a UI image instead of plain text could possibly enhance the game by improving the readability of the displayed text ingame.


  
### [Loader.cs](https://github.com/idarlm/imt3603-project/blob/be708eb4057fefc8854ebd3d6d808fdda2d29a94/Assets/Scripts/UI/Loader.cs)
The Loader script is another example of code I consider good, specifically designed for loading scenes and implementing a loading screen during the process. A part of the script I consider particularly good is the Enum containing all the scenes. Using an Enum simplifies code updates and maintenance, while also mitigating the risk of errors attributable to spelling mistakes. Loading scenes becomes a straightforward task by utilizing the Enum and appending ".ToString()" (Line [27]( https://github.com/idarlm/imt3603-project/blob/be708eb4057fefc8854ebd3d6d808fdda2d29a94/Assets/Scripts/UI/Loader.cs#L27) and [35]( https://github.com/idarlm/imt3603-project/blob/be708eb4057fefc8854ebd3d6d808fdda2d29a94/Assets/Scripts/UI/Loader.cs#L35)).
```cs
public enum Scene
    {
        MainMenuScene,
        DemoScene, //The actual gamelevel
        LoadingScene
    }
```
Apart from its readability the Loader script exemplifies good coding practices by incorporating a static class, enhancing the modularity of the code. The LoaderCallback function ([Line 32-36]( https://github.com/idarlm/imt3603-project/blob/be708eb4057fefc8854ebd3d6d808fdda2d29a94/Assets/Scripts/UI/Loader.cs#L32-L36)) in combination with the [LoaderCallback]( https://github.com/idarlm/imt3603-project/blob/be708eb4057fefc8854ebd3d6d808fdda2d29a94/Assets/Scripts/UI/LoaderCallback.cs) script further ensures that a scene does not display before it completes loading. It's also important to note that the code relies on Unity's SceneManager, necessitating the addition of scenes to the build settings for the loading mechanism to function properly.
  
### [MainMenuUI](https://github.com/idarlm/imt3603-project/blob/17779c6f017a7e275f14ff4af043da5eb3ef36d2/Assets/Scripts/UI/MainMenuUI.cs)
The MainMenuUI script is not only clean and easily comprehensible but also highly adaptable for future functionalities. Utilizing SerializeField for the buttons streamlines the connection between the UI buttons and the script, making it effortless to add new buttons through the creation of new SerializeField.
```cs
    [SerializeField] private Button playButton; // Button component to start game
    [SerializeField] private Button quitButton; // Button component to quit game
```
The button logic incorporates onClick listeners with lambda functions, contributing to enhanced readability, ease of updates, and a neat approach when introducing new buttons. The function below also illustrates the simplicity of using the loading system.
```cs
     playButton.onClick.AddListener(() =>
        {
            // Call the Load function from the Loader class to load the game scene
            Loader.Load(Loader.Scene.DemoScene);
        });
```
Placing the onClick listeners inside an Awake function ([Line 9]( https://github.com/idarlm/imt3603-project/blob/c5faf370b70f57b3a581585c0022ef9441037149/Assets/Scripts/UI/MainMenuUI.cs#L9)) ensures their initialization before the game starts, guaranteeing clickable buttons as soon as they load on the screen. While there were discussions about adding a load-from-save function to the game, time constraints prevented its implementation. Nevertheless, introducing a button for this would be straightforward, creating a third button that functions only if there's a save. Additionally, the play button's text could dynamically change to "Restart" instead of "Start," using a similar method as employed in the [interactionTextUI]( https://github.com/idarlm/imt3603-project/blob/c5faf370b70f57b3a581585c0022ef9441037149/Assets/Scripts/UI/interactionTextUI.cs#L43) script. One potential improvement involves the quit button, which currently exits the game abruptly; updating it to include a confirmation dialogue would enhance the user experience.


## Bad code

### [InteractTrigger.cs](https://github.com/idarlm/imt3603-project/blob/be708eb4057fefc8854ebd3d6d808fdda2d29a94/Assets/PuzzleFiles/InteractTrigger.cs)
While the InteractTrigger script is easy to read and the logic is good, there are some aspects that lead me to consider it as bad code.
```cs
 if (Input.GetKeyDown(KeyCode.E) && inRange) {
            FireTriggered(this, EventArgs.Empty);
        }
```
The code checks for player interaction using Input.GetKeyDown, which, while not inherently bad, would be more maintainable and easier to update if incorporated into our dedicated [player input]( https://github.com/idarlm/imt3603-project/blob/be708eb4057fefc8854ebd3d6d808fdda2d29a94/Assets/Scripts/PlayerInput/IPlayerInput.cs) script. That way, changes can be applied uniformly across all scripts.
```cs
if (other.gameObject.name == "Player") {
```
The collision checks in both the OnTriggerEnter and OnTriggerExit functions ([Lines 15-30]( https://github.com/idarlm/imt3603-project/blob/be708eb4057fefc8854ebd3d6d808fdda2d29a94/Assets/PuzzleFiles/InteractTrigger.cs#L15-L30)) presents another concern. Using hardcoded strings is considered bad practice, and using the same string in both functions compounds the issue. Implementing a const would be preferable to be able to easily update the string without introducing errors related to spelling. Additionally, using tags instead of gameObjects names is a more applied practice. However, in the context of potential multiplayer updates, relying on tags could pose a challenge as it wouldn't differentiate which player interacted with the object.

### [KeypadListener.cs](https://github.com/idarlm/imt3603-project/blob/17779c6f017a7e275f14ff4af043da5eb3ef36d2/Assets/PuzzleFiles/KeypadListener.cs) & [VisualListener.cs](https://github.com/idarlm/imt3603-project/blob/17779c6f017a7e275f14ff4af043da5eb3ef36d2/Assets/PuzzleFiles/VisualListener.cs)
I consider the KeypadListener.cs and VisualListener.cs scripts bad code. They are connected to the same puzzle involving a PIN code, utilizing interactable game objects that display symbols. To trigger an event, the correct combination must be input. The puzzle's logic was challenging to implement, leading to complex code. Each interactable object within the puzzle cycles through four game objects, creating resource-intensive operations and introducing unnecessary complexity. With additional time, we would have pursued a more efficient solution utilizing a single game object with dynamic rotation. This approach would have streamlined both scripts and eliminated much of the repeating code.

The KeypadListener code lacks readability and, as previously noted, suffers from significant code duplication. The OnTriggered ([Lines 22-51]( https://github.com/idarlm/imt3603-project/blob/17779c6f017a7e275f14ff4af043da5eb3ef36d2/Assets/PuzzleFiles/KeypadListener.cs#L22-L50)) and OnTriggeredFinished ([Lines 53-73]( https://github.com/idarlm/imt3603-project/blob/17779c6f017a7e275f14ff4af043da5eb3ef36d2/Assets/PuzzleFiles/KeypadListener.cs#L53-L73)) methods are nearly identical. To enhance code clarity and reduce redundancy, I would utilize a double for-loop rather than manually checking each key. Additionally, I would change the if-check ([Line 46-49]( https://github.com/idarlm/imt3603-project/blob/17779c6f017a7e275f14ff4af043da5eb3ef36d2/Assets/PuzzleFiles/KeypadListener.cs#L46-L49)) to an if-check like the one used in the [TriggerGroup]( https://github.com/idarlm/imt3603-project/blob/17779c6f017a7e275f14ff4af043da5eb3ef36d2/Assets/PuzzleFiles/TriggerGroup.cs#L38-L41) script. 

While the implementation of dynamic arrays for both the triggers and correct keys is commendable, its effectiveness is compromised by the fact that the key objects are constrained to three items. The absence of a dynamic array for the key objects restricts flexibility for puzzles featuring a different number of interactable game objects. Nevertheless, the use of SerializeField I consider good code, as it simplifies the process of modifying the correct keys in Unity.
```cs
    public PuzzleTrigger[] triggers; // List of all the triggers
    [SerializeField] GameObject[] correctKeys; 
    private bool key1 = false, key2 = false, key3 = false; 
    private Transform key1Object, key2Object, key3Object; // The object for each symbol
```
The hardcoded for-loop in the KeypadListener script is considered bad practice because it leaves the reader in the dark about why it iterates four times. In this code, the number four signifies the symbols each object can rotate between, and this constant should have been stored in a variable. Doing so would enhance clarity and facilitate easier updates to the code.
```cs
    for (var i = 0; i < 4; i++) {
```
Both the KeyPadListener script and VisualListner suffer from poor variable names and complex code that hinders code comprehension. Variable names like "key2Object" lack descriptive information for the reader. The challenge arose from uncertainty about the terminology for different puzzle components, leading to the adoption of "keys" as a generic term used throughout. This term is particularly confusing due to the actual presence of keys in some puzzles. Enhancing script clarity could possibly be achieved by assigning more accurately descriptive names to variables and introducing helping variables to assist in communicating the functionality of the complex code.
```cs
    //Line 61 in the KeyPadListener Script
    key2Object = triggers[1].transform.GetChild(i);

    //Line 18 in the VisualListener Script
    transform.GetChild(i).gameObject.SetActive(false);
```

## Reflection
At the beginning of this course, game development was completely new to me. The entire process, from understanding the game engine to learning a new programming language, presented a steep learning curve. In our group of four students, two, including myself, had no prior experience, while the other two possessed varying levels of experience. This dynamic introduced challenges, particularly as the experienced members set ambitious goals.

The more experienced members seemed to effortlessly navigate through tasks, leaving those without prior experience, including myself, struggling to catch up. Each time I learned a new concept and wanted to contribute; the task had already been completed. When asking what I could contribute I was often directed to watch tutorials to enhance my understanding, delaying my active involvement. It wasn't until later in the project that the other less experienced student and I were assigned specific responsibilities, which made it feel as though our input was not welcomed.

We dedicated significant effort to the puzzles, as that was our assigned responsibility for the game. Crafting the puzzles demanded numerous revisions and involved a steep learning curve, all while dealing with the inherent challenge posed by the puzzle logic itself. After completing the puzzle logic, I started working on the main menu and loading screen, while the other student continued working on the puzzles by making prefabs, improving the code, and incorporating the puzzles into the game world. After completing the main menu and loading screen, I proceeded to design UI elements for the game, such as a stamina bar and a visibility detection feature.

The difference in experience, coupled with the initial assignment of responsibilities that excluded less-experienced team members, resulted in uneven contributions. This led to varying rates of progress among team members, with some advancing more rapidly than others. Throughout the duration of puzzle and UI element completion, experienced students managed to accomplish a significantly greater amount. This, in part, explains why my involvement in other parts of the game was limited.

Despite the challenges, the learning experience was substantial. Learning Unity from scratch was a daunting task, but I navigated it effectively through a combination of YouTube tutorials and engaging in smaller projects. As my confidence in using Unity grew, I developed practical skills such as creating and utilizing prefabs to streamline the process. I also learned how to use SerializeFields for easy variable adjustments without altering the script, providing a convenient way to test different values during gameplay. Additionally, I learned managing assets, how to add them and use them in a scene, as well as how to attach scripts to gameObjects to give them logic. Furthermore, I became familiar with Unity's integrated UI system, particularly in configuring various elements like buttons, images, and sliders, and manipulating them through code. 

I had also never programmed in C# before, but its similarities to C++ facilitated a relatively smooth transition. It might be possible to enhance the code using C# techniques unfamiliar to me. However, given the number of new concepts to grasp, I directed my learning efforts toward Unity. My coding proficiency in game development expanded significantly. I learned to use SerializeField over public variables and to utilize static to reuse code in different scripts, how to update and move game objects, and using lerp to do it smoothly. I also got familiar with the Unity functions Update, Start, Awake, and LateUpdate and when to use which. Additionally, I got familiar with event handling- learning how to fire and subscribe to events, along with effectively managing the events themselves. Furthermore, I learned to control object activation, facilitating the showing, and hiding of objects in the game world. I also became adept at handling collisions for player-object interactions and preventing unintended player passage through objects.

In summary, despite the challenges faced, I have learned a lot. I have gone from having no prior experience in Unity, never having worked with C#, and never having programmed a game, to acquiring a significant amount of knowledge in all these areas. In addition to everything I learned while developing the game I also learned a lot from the tutorials I followed, that I did not get to utilize in our game. One example is the input system integrated in Unity, which simplifies the process of connecting logic to various buttons on both keyboards and controllers. Despite not achieving a fully realized game, I am very proud of my work and how much I have been able to achieve and look forward to delving further into the world of game development.






