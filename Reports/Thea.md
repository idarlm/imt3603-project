|Description | My Weight |
|----|----|
|Gameplay video | 10 |
|Code video | 0 |
|Good Code  | 25 |
|Bad Code | 25 |
|Development process | 10 |
|Reflection | 30 |

## Good code
- TriggerGroup -> all check

```cs
if (activated.All(t => t)) {
            allActivated = true;
        }
```

- interactionTextUI
    - Lateupdate for the UI text
    - teksta følge kamera
    - static
    - Show and Close can be used in diferent parts of code, kunne enkelt vårte oppdatert til å ta inn paramter og såtte custom text på ulike ting.
    - null checks
    - bad: singleton istaden? hardkoda text, ingen error sjekk kamera, image istaden med setActive?
  
- loader
    - Enum og .toString so ingen skrivefeil, lett og legge til flere level/scene
    - clean, lett leselig
    - static class -> modularity
    - bad: dependent på scenemanger i unity so må passe på at scenene ligge der i rett rekkefølge?
  
- mainmenuUI
    - Serializefield, easy to add new buttons
    - lambda in the click listeners, easy to read
    - Using the num to easily load the right startscene
    - clean ,readable, easy to add new buttons or change button behavior.
    - add button for saved games, so if there exists a saved game the button play button
      changes text to restart, and a second button called load from save is avaiable.
    - bad: quit -> close right away no confirm message.
 
- visibilityUI


Både og:
- interacttrigger
    - good: clean, easy to read, using UI system to display/close text, and event system
    - input.keydown instead of the playerinput system
    - checking gameObject name -> what if multiplayer? also not good to hardcode string

## Bad code
   
- keypadlistener
    - repeat code
    - hard to read
    - hardcoded for loop
    - bad variablenames
    - triggers.transform.getchil confusing.
    - dynamic triggers and correct key array, but hardcoded three keys.
    - good: serializefield so we can easily change correct keys in unity, triggers, the logic of all true fire
  
- visualListener
    - Hard to read
    - transform.getchile.gamobject... ka e d for noke? utydelig kode
    - indexen?

## Reflection
At the beginning of this course, game development was completely new to me. The entire process, from understanding the game engine to learning a new programming language, presented a steep learning curve. In our group of four students, two, including myself, had no prior experience, while the other two possessed varying levels of experience. This dynamic introduced challenges, particularly as the experienced members set ambitious goals.

The more experienced members seemed to effortlessly navigate through tasks, leaving those without prior experience, including myself, struggling to catch up. Each time I learned a new concept and wanted to contribute; the task had already been completed. When asking what I could contribute with I was often directed to watch tutorials to enhance my understanding, delaying my active involvement. It wasn't until later in the project that the other less experienced student and I were assigned specific responsibilities, which made it feel as though our input was not welcomed. 

**Our responsibility was the puzzles for the game. We dedicated significant effort to the puzzles. It required multiple rewrites and a considerable learning curve.  In addition to the logic of the puzzles itself being a challenge.** After completing the puzzle logic, I started working on the main menu and loading screen, while the other student continued working on the puzzles by making prefabs, improving the code, and incorporating the puzzles into the game world.** When the main menu and loading screen were finished, I made UI elements which included a stamina bar and a visibility detection.**

**The difference in experience and the fact that us less experienced did not get assigned any responsibilities in the beginning led to uneven contributions within the team, with some members progressing faster than others. In the time it took to complete the puzzles and UI elements the experienced students had gotten a lot more done.** This, in part, explains why my involvement in other parts of the game was limited. 

Despite the challenges, the learning experience was substantial. Learning Unity from scratch was a daunting task, but I navigated it effectively through a combination of YouTube tutorials and engaging in smaller projects. As my confidence in using Unity grew, I developed practical skills such as creating and utilizing prefabs to streamline the process. I also learned how to use SerializeFields for easy variable adjustments without altering the script, providing a convenient way to test different values during gameplay. **Other things I learned includes managing assets, how to add them and use them in a scene, as well as how to attach scripts to objects to give them logic. I also got familiar with the integrated UI system in Unity, particularly when it comes to setting up different UI elements such as buttons, images and sliders and manipulating these using code.**

I had also never programmed in C# before, but its similarities to C++ facilitated a relatively smooth transition. **There are probably code however that could be improved using C# code I am not familiar with, but with all the new things to learn I focused my learning on Unity.**  My coding proficiency in game development expanded significantly.  I learned to use SerializeField over public variables and to utilize static for code reuse in different scripts. **How to update and move game objects, using lerp to do it smoothly. I also got familiar with the unity functions update, start, and lateupdate and when to use which.  Additionally, I got very familiar with event handling, firing events, subscribing to events. Managing object activation, handling collisions for player-object interactions, and preventing unwanted player passage through objects added a layer of complexity to my learning journey.**

**In summary, despite the challenges faced, I have learned a lot. I have gone from having no experience in Unity, never having touched C#, and never having programmed a game, to have gained a lot of knowledge within all of these. Despite not achieving a fully realized game, there is a sense of pride in the progress made. I learned a lot about game development from the tutorials that I did not get to utilize in our game. One of these being the input system that is integrated in unity, which makes it easy to connect logic to different buttons, both on keyboards and controllers. I am very proud of my work and how much I have been able to do and look forward to delving further into the world of game development.**



