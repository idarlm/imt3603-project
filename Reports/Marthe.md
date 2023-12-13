|Description | min | def | max |
|----|----|----|----|
|Gameplay video | 5 | 10 | 20 |
|Code video | 0 | 10 | 15 |
|Good Code  | 10 | 20 | 30 |
|Bad Code | 10 | 20 | 30 |
|Development process | 10 | 20 | 30 |
|Reflection | 10 | 20 | 30 |

|Description | My Weight |
|----|----|
|Gameplay video | 10 |
|Code video | 0 |
|Good Code  | 20 |
|Bad Code | 20 |
|Development process | 20 |
|Reflection | 30 |

## Video
A **video** showing off the **code** that is tightinly integrated with the game engine that is difficult to see from the text of the programming.
I have decided not to do a video, because i feel there is no specific Unity game engine functionalities we have used that is not somewhat visible through the code files.

## Good code
A link to, and discussion of, code you consider **good**.
[Animation code](https://github.com/idarlm/imt3603-project/blob/e66721fe202b8c21dbed1a644c7376566f558405/Assets/PuzzleFiles/Animations/AnimateOnEvent.cs#L7-L34)
I have implemented animation code to use on doors, walls, etc. to rotate and move. The code is implemented in a way to easily make new animations, for example shrink something. All that is needed is a new code that inherit the AnimationOnEvent class, and shrink the targeted object by changing its scale using something like written below.
```
"transform.localScale += new Vector3(x, y, z);".
```

The AnimateOnEvent class is an abstrach class, this means that the class is restricted and you can't create objects with it. To access it, it must be inherited. It's functionallity is to activate and deactivate animations through child classes. This is done with the "isActive" bool when triggers start and finishes. The two classes that inherit from AnimateOnEvent are RotateOnEvent and MoveOnEvent. "Link to rotate and move code".
- Trigger group

- Linking objects to puzzle files
- Puzzle integration
- Prefabs made from the other prefabs, easy to put inside the world


## Bad code
A link to, and discussion of, code you consider **bad**.
- Keypad puzzle
  - Was originally created to to switch between different objects
    - Many gameobjects needed with this method, what would happend if we wanted a longer passcode and more symbols to choose between?
    - Hard to keep track of many gameobjects, and what objesct is the right one
  - Was later decided to use a cube that could rotate on interaction
  - Decided not to change the code because of time, and rather write about it in the report
  - To improve it: Use rotation animation, and set a "right rotation" field and check if it matches. Would only need as many cubes as symbols we want in the passcode, a lot less resources.

## Reflection
A personal **reflection** about the key concepts you learnt during the course.
- Started without any knowledge<br>
I started this course with no knowledge about game programming. I also had no previous experience with Unity or other game engines, so it took some time to learn how to use it and get into the logic of game programming.

- Some people in the group had experience with Unity<br>
In our group we had two people with no previous knowledge, including me, and two who already had some expirience with Unity and game programming. Because of this, we had set some high expectations for our game, but understood that some of us had to do more work than others because of our mixed knowledge. In the time we had to work with the game, I used a lot of time in the begining to try and understand what Unity was and how to use it.
Consepts I learned about Unity was for example its itegrated input system for movement and interactions. However, this was not one of my taskt in the project. I also learned making game objects, serializable fields that's visible in Unity, making prefabs and adding materials to gameobjects. I would say my knowledge about Unity at the end of the projects is very limited still, because I used my time on what I specifically needed to know to do my own parts of the game. 

After learning about Unity I had to start participating in the actual coding of our game, and I felt it was very hard to know where to begin. Me and the other group member with no previous knowledge was in charge of implementing the puzzles of our game. We both came up with puzzle-ideas that we thought were doable for us, and tried watching videos to know where to start. However, others in the group wanted us to implement the puzzle code in a way that was not easy to learn with simple videos and with the short amout of time we had. Because of this, we got a little help starting out, and some ideas from the other members on how to implement the puzzles.
When implementing the puzzles, I got more and more familiar with the game logic and Unity. I found it hard in the begining to juggle all the different gameobjects, and understand what was missing from serializable fields and scripts, but I got a hang of it in the end. I saw much improvement in my debugging skills and understanding errors.

Throughout the process of implementing the puzzles, we tried communicating with the other group members to get their input. We often got feedback that stated our work was fine, and to continue to implement the other puzzles we had planned to do. However, after we had implemented all puzzles, we were told to do it in another and better way. Myself and my puzzle-companion were originally happy and proud of everything we had managed to do, and it would have worked fine even though it was not the best code. While my puzzle-companion started on another task, I had to revise much of the code. I rewrote about half of the code we had, this was a tiresom job, especially because it could have been done from the start, if we just had gotten better feedback. In this process I learned more about combining scripts and inheriting, and wrote new generic code to use for more than just the puzzles. The animations code I wrote about earlier is an example of this. Because I basically wrote the puzzles code two times, I got much more practice with the logic, and connecting objects with scripts and triggers. Every puzzle was also made in their own scene in Unity, so I learned how to use them aswell. 




Throughout the whole project I have learned coding C#.



- Too high ambitions

