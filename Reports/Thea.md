|Description | My Weight |
|----|----|
|Gameplay video | 10 |
|Code video | 0 |
|Good Code  | 25 |
|Bad Code | 25 |
|Development process | 10 |
|Reflection | 30 |

## Good code

### [TriggerGroup.cs]()
I consider this code good as it is clean easy to read and is an important part of the puzzles. This code makes it possible to connect multiple puzzles or parts of puzzles together and only fire an event when all of them have been completed. At the tsart of the game it subsribes all the triggers to an event and adds as many false as there are triggers to a list. If a trigger is triggered it will find the right trigger and check if it is activated or not, so that it the puzzle only gets completed if all the parts are activated. One part of the code that I consider particulary good is the check if all the triggers are activated. 

```cs
if (activated.All(t => t)) {
            allActivated = true;
        }
```

Instead of having to loop through the entire list we used the "All" method, which makes the code consise and easy to read. It also stops as soon as one of the items is false saving resources.



### [InteractionTextUI]()
For the UI when it comes to the text connected to interactable obejcts it is attached to the player. The UI text is coded so that it will follow the camera. This is done inside an LateUpdate function so that it happens after all the updates, which means the text will follow the player after it has been updated. The camera is not being checked if its null before setting the rotation.
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
The text UI reference is fetched by using serialzeField, (link til linje 7), and then there is a static variable called instance (link til linje 8) that will be set to the Txt UI. This code also has to sttic functions to show and close the text. Using static is something I consider good code. This way one can call these functions from other scripts to decide when you want to show the text.
Both the ShowUI and CloseUI functions(link til funksjonane) makes sure that the instance is set before trying to update the text, so that if there is no instance ther will not be any errors. 
The ShowUI function could easily be improved by giving it a paramteter so that the text could be set to whatever best fits based on where the function is called, and no string given having a default string. As of now the text is hardcoded which I do not consider good code. Another thing that could possible improve the code is using an UI image instead of just text so that the text would be easier to read. And instead of just changing the text betweent he hardcoded text to a empty string, acticvating and disabling the text instead to save resources. 

  
### [Loader.cs]()
In the loader script used for loading scenes I have created and Enum containing all the scenes. This I consider good code as it is easy to update and maintain  the code as well as making it less likely for an error to happen based on a spelling error. When loading a scene you can just use the enum and add ".ToString()"(link til loadscene linje).
```cs
public enum Scene
    {
        MainMenuScene,
        DemoScene, //The actual gamelevel
        LoadingScene
    }
```
In addition to being very readable and clean the Loader script has a static class which makes the code modular which is also considered good code.
The code is however dependent on Unitys scenemanger so one have to make sure that the scenes are added to the build settings for the loading to work.
  
### [MainMenuUI]()
The main menu code is maybe the cleanest code I have written in this project. It is very clean and readbale, as well as it is very easy to update if new functionalities were to be added.
For the buttons I used serialzeField, which makes it easy to connect the UI buttons to the script and easy to make a new serialzeField and button.
```cs
    [SerializeField] private Button playButton; // Button component to start game
    [SerializeField] private Button quitButton; // Button component to quit game
```
For the buttons logic they have onclick listeners, with lambda functions which makes them easy to read, easy to update and easy to add new buttons with it getting messy. 
lambda in the click listeners, easy to read. Using the enum to easily load the right startscene
```cs
     playButton.onClick.AddListener(() =>
        {
            // Call the Load function from the Loader class to load the game scene
            Loader.Load(Loader.Scene.DemoScene);
        });
```
The onclick listners are inside an Awake function (link til linje 9), so that they get initialized before the game starts, this will ensure that the buttons are clickable as soon as they have loaded on the screen.
We talked about adding a load from save function to our game but did not have the time. Adding a button for this owuld be really easy as we could easily have a third button which would only work if there was a save, and could change the play buttons text to be restart instead of start using the same method as we did in the interaction text script(link til der ej sette teksta til interact).
One thing that could be imrpoved is the quit button. As of now it just quits the game with no warning could update to have a confirmation.


## Bad code

### [InteractTrigger.cs]()
Although the interactTrigge script is easy to read it does have some issues that made me consider it as bad code. 
```cs
 if (Input.GetKeyDown(KeyCode.E) && inRange) {
            FireTriggered(this, EventArgs.Empty);
        }
```
When checking if the player is interacting we are using Input.GetKeyDown, which isn't necessarily bad code in it self, but considering that we have a whole script dedicated to playerinput, it should have been incorporated there and then used here, so that it would be more maintainable and easier to update across all the scripts.

The other issue that makes me consider this code as bad is our collision check.
```cs
if (other.gameObject.name == "Player") {
```
First of all its bad practice using hardcoded strings, in addition to using the same string in both the OnTriggerEnter and OnTriggerExit functions(link til linje 17 og 26). Here we should have used a const instead so that we could easily update it without having any errors related to spelling errors. It would also be more correct to use tags. Another problem with this check is if we were to update the game for multiplayer. Using a tag could work, but then we wouldnt know which player interacted with the object.

### [KeypadListener.cs]() & [VisualListener.cs]()
These scripts I consider bad code. They are connected to the same puzzle, which is the puzzle with three symbols that need to match for an event to fire. The logic of this puzzle was hard to code which is reflected in the code. We coded it so that each interactable object in the puzzle loops between four game objects. This is not the best solution as it takes up more resources and the code gets more confusing. If we had time we would have changed it to use one gameobject and then change that objects rotation. This would have improved both the scripts, as we would not need all the repeating code.

The code in KeypadListener is not that easy to read and has a lot of repeating code. OnTriggered and OnTriggeredFinished is almost identical(link) in addition to the code inside the for loop in these functions.  Fix it by double for loop, instead of checking manually for each key. If check to check if all true similar to the one in triggergroup(link til triggergourp).


Dynamic triggers and correct key array, but hardcoded three keys.  This is not flexible if we wanted to have puzzles with less or more symbols than three. It is also pointless for the arrays to be dynamic if they are only gonna contain three items anyway. Something that is good however is the use of serializefield so we can easily change correct keys and triggers in Unity.
```cs
    public PuzzleTrigger[] triggers; // List of all the triggers
    [SerializeField] GameObject[] correctKeys; 
    private bool key1 = false, key2 = false, key3 = false; 
    private Transform key1Object, key2Object, key3Object; // The object for each symbol
```

Hardcoded for loop is considered bad practice. Why is it four? Four is the number of symbols each object has to rotate between. This should have been put in a variable so it would be clear what it was, as well as to make it easier to update.
```cs
    for (var i = 0; i < 4; i++) {
```

Bad variablenames and confusing code that does not make it easier. key2Object does not give a lot of information to the reader. We had trouble comming up with good variable names as we were not quite sure what the different parts of such a puzzle is called, so we ended on keys and used this everywhere. Later we realized it was a bad name since we actually have keys in some puzzles. The bad variable name combined with the confusing code that comes after makes it hard to undrstand what exactly is going on.
```cs
    key2Object = triggers[1].transform.GetChild(i);
```
This was one of the biggest issues in the VisualListener script as well.
```cs
     transform.GetChild(i).gameObject.SetActive(false);
```
The code above is repeated all the time but its confusing what this actually is. 



## Reflection
At the beginning of this course, game development was completely new to me. The entire process, from understanding the game engine to learning a new programming language, presented a steep learning curve. In our group of four students, two, including myself, had no prior experience, while the other two possessed varying levels of experience. This dynamic introduced challenges, particularly as the experienced members set ambitious goals.

The more experienced members seemed to effortlessly navigate through tasks, leaving those without prior experience, including myself, struggling to catch up. Each time I learned a new concept and wanted to contribute; the task had already been completed. When asking what I could contribute with I was often directed to watch tutorials to enhance my understanding, delaying my active involvement. It wasn't until later in the project that the other less experienced student and I were assigned specific responsibilities, which made it feel as though our input was not welcomed. 

**Our responsibility was the puzzles for the game. We dedicated significant effort to the puzzles. It required multiple rewrites and a considerable learning curve.  In addition to the logic of the puzzles itself being a challenge.** After completing the puzzle logic, I started working on the main menu and loading screen, while the other student continued working on the puzzles by making prefabs, improving the code, and incorporating the puzzles into the game world.** When the main menu and loading screen were finished, I made UI elements which included a stamina bar and a visibility detection.**

**The difference in experience and the fact that us less experienced did not get assigned any responsibilities in the beginning led to uneven contributions within the team, with some members progressing faster than others. In the time it took to complete the puzzles and UI elements the experienced students had gotten a lot more done.** This, in part, explains why my involvement in other parts of the game was limited. 

Despite the challenges, the learning experience was substantial. Learning Unity from scratch was a daunting task, but I navigated it effectively through a combination of YouTube tutorials and engaging in smaller projects. As my confidence in using Unity grew, I developed practical skills such as creating and utilizing prefabs to streamline the process. I also learned how to use SerializeFields for easy variable adjustments without altering the script, providing a convenient way to test different values during gameplay. **Other things I learned includes managing assets, how to add them and use them in a scene, as well as how to attach scripts to objects to give them logic. I also got familiar with the integrated UI system in Unity, particularly when it comes to setting up different UI elements such as buttons, images and sliders and manipulating these using code.**

I had also never programmed in C# before, but its similarities to C++ facilitated a relatively smooth transition. **There are probably code however that could be improved using C# code I am not familiar with, but with all the new things to learn I focused my learning on Unity.**  My coding proficiency in game development expanded significantly.  I learned to use SerializeField over public variables and to utilize static for code reuse in different scripts. **How to update and move game objects, using lerp to do it smoothly. I also got familiar with the unity functions update, start, and lateupdate and when to use which.  Additionally, I got very familiar with event handling, firing events, subscribing to events. Managing object activation, handling collisions for player-object interactions, and preventing unwanted player passage through objects added a layer of complexity to my learning journey.**

**In summary, despite the challenges faced, I have learned a lot. I have gone from having no experience in Unity, never having touched C#, and never having programmed a game, to have gained a lot of knowledge within all of these. Despite not achieving a fully realized game, there is a sense of pride in the progress made. I learned a lot about game development from the tutorials that I did not get to utilize in our game. One of these being the input system that is integrated in unity, which makes it easy to connect logic to different buttons, both on keyboards and controllers. I am very proud of my work and how much I have been able to do and look forward to delving further into the world of game development.**



