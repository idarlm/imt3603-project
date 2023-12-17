|Description | My Weight |
|----|----|
|Gameplay video | 10 |
|Code video | 0 |
|Good Code  | 20 |
|Bad Code | 30 |
|Development process | 15 |
|Reflection | 25 |

## Video
I have decided not to do a video, because i feel there is no specific Unity game engine functionalities we have used that is not somewhat visible through the code files.

## Good code
I have implemented animation code to use on doors, walls, etc. to rotate and move. The code is implemented in a way that make it easier make new animations and have them activated on a fireTrigger. For example, to shrink something, all that is needed is a new code that inherit the AnimationOnEvent class, and shrink the targeted object by changing its scale using something like written below.
```cs
transform.localScale += new Vector3(x, y, z);
```
You would the connect the shrink script to the object you want to shrink, and in our case, we have an interaction script that fires a trigger when pressing E, that could be connected to a parent object.

The AnimateOnEvent class is an abstract class, this means that the class is restricted and you can't create objects with it. To access it, it must be inherited. It's functionallity is to activate and deactivate animations through child classes. This is done with the "isActive" bool when triggers starts and finishes, as you can see in [this code](https://github.com/idarlm/imt3603-project/blob/e66721fe202b8c21dbed1a644c7376566f558405/Assets/PuzzleFiles/Animations/AnimateOnEvent.cs#L7-L34). In the update function, we continuously call the Animate function, where the function body is difined differently in each of the animation scripts. The two classes that inherit from AnimateOnEvent are RotateOnEvent and MoveOnEvent. The AnimationOnEvent class contains shared and reusable code that other scripts can inherit, instead of repeating code in all the scripts.

MoveOnEvent is one of the classes that inherit the AnimateOnEvent class. It is a simple script that moves the object based on the input in a serializable field. A serializable field is a field that shows in the game engine, and takes input from there. This script originally listened to the "isActive" when "TriggerDeactivated" ran, but I removed the fireTriggerFinished in another script, because i changed my mind on an objects behaviour. So right now, the "TriggerDeactivated" funtion in AnimateOnEvent is not used, but it is integrated for easy use if we were to continue developing our game. You can see that the body of the Animate function in MoveOnEvent contains actions for when "isActive" is true, and also if it's not because of the previous functionality I had on the object [in this code](https://github.com/idarlm/imt3603-project/blob/b30399eefaa60e6946dadef5f05c2a58dbb5360e/Assets/PuzzleFiles/Animations/MoveOnEvent.cs#L5-L21).

Overall, the purpose of the AnimateOnEvent class and the scripts that inherit that class, is that the code is reusable, instead of repeating code in each animation script to trigger it. The thought is as mentioned earlier, that you only need to decide what animation you want, and write that code in the "Animate" function body. In addition, the anitmation scripts are also coded for easy alteration in movement. The rotation in RotateOnEvent are for example decided based on axis and angle from serializable fields, instead of hardcoded to only a doors expected rotation behavior.

- Trigger group
Another smart script that me and my puzzle-partner in the group integrated were TriggerGroup. This script listenes for an event from several triggers, before it is allowed to fire an event itself. 
- Linking objects to puzzle files
- What is good code? Reusable (rotate on event + animate on event), Understandable.
- Puzzle integration
- Prefabs made from the other prefabs, easy to put inside the world


## Bad code
- Keypad puzzle
  - Was originally created to to switch between four different objects.
    - Many gameobjects needed with this method, what would happend if we wanted a longer passcode and more symbols to choose between?
    - Hard to keep track of many gameobjects, and what object is the right one.
  - Was later decided to use a cube that could rotate on interaction to display up to six different symbols.
  - Decided not to change the code because of lack of time, and rather write about it in the report.
  - To improve it: Use already existing rotation animation script, and set a "right rotation" field and check if it matches. Would only need as many cubes as symbols we want in the passcode, a lot less resources.
 
Some of the code that me and another group member wrote that I think was bad was the code for the Keypad puzzle, where you need to interact with a cube to set in the right number sequence to open the castle door. Not only is the code badly implemented, but the puzzle idea as well. 
We originally planned a puzzle that had three spots for one out of four different symbols. I admit we didn't think all to well through the implementation of this, and realised all too late that we could have done it much easier and better. 
The objects hierarchy in Unity are set up like this:<br>
![image](https://github.com/idarlm/imt3603-project/assets/127052202/acd0e5e2-261c-4a12-ba21-9772a21e1fb2)

The Keys object has the KeypadListener script attached to it, Key1 has the InteractTrigger script, and Key1Visual has the VisualListener script attached. Inside Key1Visual are four objects of the same number-cube, just rotated to display either 1, 2, 3 or 4. Key2 and Key3 are just as Key1. Already we can tell that there are a lot of redundant game objects that consume unnecessary resources.
As mentioned the keypad puzzle was created for switching between four different objects within the game environment. Our approach involved creating numerous game objects to represent each possible sumbol/number, resulting in a complex system that posed challanges for scalability and maintenance. However, we made all of the code for the puzzle to work, and felt we had too little time left to change it. 

I will try to explain our code solution for that specific puzzle, and talk about desired changes of we were to revide the code.
The InteractTrigger script is a simple script that fires a trigger when the player are in range of the objects box collider and presses E. The script VisualListener are connected to each Key*N*Visual, and it is the script that switches the visual of the symbols.
In [this](https://github.com/idarlm/imt3603-project/blob/10034c7e6441a1469f4d9b398fc671bf1193c672/Assets/PuzzleFiles/VisualListener.cs#L11-L21) code we start by subscribing to the ChangeVisual method that listens to the triggered event in InteractTrigger. We then deactivate three of the four children (cubes with number 1-4) so that the number 1 cube is the only one active and visible. 
The ChangeVisual method is where we change what cube is active and visible. In [this](https://github.com/idarlm/imt3603-project/blob/bbcf8f6c311bbd8daff77b89a38ce6f6aa021984/Assets/PuzzleFiles/VisualListener.cs#L25-L29) for-loop we go through all the four children of Key*N*Visual, and get the active-state of the "i" number child. Furthermore, in [this](https://github.com/idarlm/imt3603-project/blob/bbcf8f6c311bbd8daff77b89a38ce6f6aa021984/Assets/PuzzleFiles/VisualListener.cs#L31-L33) if-check if the child is the active one, we set it to not active. Then, if the child we are currently on is not the last of the four children, the next child is set to active. If it is the last one, we reset the index "i" and set the first child to active, as you can see in [this](https://github.com/idarlm/imt3603-project/blob/bbcf8f6c311bbd8daff77b89a38ce6f6aa021984/Assets/PuzzleFiles/VisualListener.cs#L34-L40) code. After the change in symbol is made, we then check if the new active child matches with the game object we have set as the correct key in a serializable field in Unity that belongs to the script. In our game puzzle, we have set the right symbols as 3 for Key1Visual, 1 for Key2Visual and 4 for Key4Visual. If the new active child and the "correctKey" matches, we fire an event. If not, we stop the trigger. [Here](https://github.com/idarlm/imt3603-project/blob/bbcf8f6c311bbd8daff77b89a38ce6f6aa021984/Assets/PuzzleFiles/VisualListener.cs#L43-L48) you can see the code for that. 

Now that I have explained the workings of changing between the four objects in every one of the three number slots (key slots), I move on to the KeypadListener script. This script listens to the events fired from each Key*N*Visual, and subscribes to two methods; OnTriggered and OnTriggeredFinished. I think that this code is hard to understand if you haven't written it yourself, and even then it is not easy to understand what is going on in the code.
We can start by analyzing the variables used in this script. [Here](https://github.com/idarlm/imt3603-project/blob/fe4cade14c31fe7b1d77d414cfa4af357fc67ce0/Assets/PuzzleFiles/KeypadListener.cs#L7-L10) you can see that we've made a list of all the triggers, this contains each of the Key*N*Visual. We also have a serialized field of an array of game objects, where we put in the same correct keys (symbols/numbers) as we put in the VisualListener script for each Key*N*Visual. These need to be put in the correct order as well. Already there are a lot of game objects to keep track of in this code, so that the KeypadListener and VisualListener scripts can work together. The next variables holds values for each key, or pin, we have in our passcode. "key*N*" tell us if the keys are the correct key (symbol/number) at the moment, and the other holds the child of Key*N*Visual.

Furthermore, in the code's start function we go through each of the triggers and subscribe to the class's two methods in a foreach loop. The OnTriggered method happens when the Key*N*Visual and its correct key matches and fires an event that triggers the method. [Here](https://github.com/idarlm/imt3603-project/blob/04c9317aaf36403b996f5f35a7d71fc38e49f4a3/Assets/PuzzleFiles/KeypadListener.cs#L24-L43) we go through all of the four different children of Key*N*Visual, and for each trigger in the array, which would also be key visual 1, 2 and 3, fetch their child number "i". We then check if that child is active (if its the one that is currently visible) and if its name is the same as the correspondent correct key's name. If so, we set its mathcing key*N* variable to true. After the for loop, we do an [if check](https://github.com/idarlm/imt3603-project/blob/04c9317aaf36403b996f5f35a7d71fc38e49f4a3/Assets/PuzzleFiles/KeypadListener.cs#L45-L49) on all the key*N* variables, if all of them are true, that means the right passcode combination has been set, and we then fire an event that the castle doors listenes to open up.




- only one cube for each symbol we want in the passcode, and rotate it on interaction
- shared serialized filed to store the correct sequenze of rotation that is the correct one
- Logic would be much of the same, just try to make the two script work better together, and reduce amount of repeating code.


## Reflection
- Started without any knowledge<br>
I started this course with no knowledge about game programming. I also had no previous experience with Unity or other game engines, so it took some time to learn how to use it and get into the logic of game programming.

- Some people in the group had experience with Unity<br>

In our group we had two people with no previous knowledge, including me, and two who already had some expirience with Unity and game programming. Because of this, we had set some high expectations for our game, but understood that some of us had to do more work than others because of our mixed knowledge. In the time we had to work with the game, I used a lot of time in the begining to try and understand what Unity was and how to use it.
Consepts I learned about Unity was for example its itegrated input system for movement and interactions. However, this was not one of my taskt in the project. I also learned making game objects, serializable fields that's visible in Unity, making prefabs and adding materials to gameobjects. I would say my knowledge about Unity at the end of the projects is very limited still, because I used my time on what I specifically needed to know to do my own parts of the game. 

After learning about Unity I had to start participating in the actual coding of our game, and I felt it was very hard to know where to begin. Me and the other group member with no previous knowledge was in charge of implementing the puzzles of our game. We both came up with puzzle-ideas that we thought were doable for us, and tried watching videos to know where to start. However, others in the group wanted us to implement the puzzle code in a way that was not easy to learn with simple videos and with the short amout of time we had. Because of this, we got a little help starting out, and some ideas from the other members on how to implement the puzzles.
When implementing the puzzles, I got more and more familiar with the game logic and Unity. I found it hard in the begining to juggle all the different gameobjects, and understand what was missing from serializable fields and scripts, but I got a hang of it in the end. I saw much improvement in my debugging skills and understanding errors.

Throughout the process of implementing the puzzles, we tried communicating with the other group members to get their input. We often got feedback that stated our work was fine, and to continue to implement the other puzzles we had planned to do. However, after we had implemented all puzzles, we were told to do it in another and better way. Myself and my puzzle-companion were originally happy and proud of everything we had managed to do, and it would have worked fine even though it was not the best code. While my puzzle-companion started on another task, I had to revise much of the code. I rewrote about half of the code we had and added all animations scripts and particle and light scripts. This was a tiresom job, especially because it could have been done from the start, if we just had gotten better feedback. In this process I learned more about combining scripts and inheriting, and wrote new generic code to use for more than just the puzzles. The animations code I wrote about earlier is an example of this. Because I basically wrote the puzzles code two times, I got much more practice with the logic, and connecting objects with scripts and triggers. Every puzzle was also made in their own scene in Unity, so I learned how to use them aswell. 




Throughout the whole project I have learned coding C#.



- Too high ambitions because of 

