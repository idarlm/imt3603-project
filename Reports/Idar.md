# Desired weighing

|Description | min | def | max | **my value** |
|----|----|----|----|----|
|Gameplay video | 5 | 10 | 20 | **10** |
|Code video | 0 | 10 | 15 | **0** |
|Good Code  | 10 | 20 | 30 | **25** |
|Bad Code | 10 | 20 | 30 | **25** |
|Development process | 10 | 20 | 30 | **10** |
|Reflection | 10 | 20 | 30 | **30** |

# Good code

**Player movement system**

I decided from the beginning that I was going to use the state pattern, since I wanted to keep the logic for each state seperated.
Without this seperation the code would get very messy with lots of if checks for many different edge cases which would make the code hard to read and maintain.

The code is divided into 6 behaviour classes: 
- FallingState
- GroundedState (abstract)
    - WalkingState
    - CrouchingState
    - SprintingState
- PushingObjectState

![Screenshot 2023-12-20 at 17 02 20](https://github.com/idarlm/imt3603-project/assets/101576034/2a8de82e-3137-4079-94c6-2544838c7592)

These 6 classes are encapsulated by the PlayerMovementSystem class, which is passed along as context to the active state object. Most of the state data is hoisted in this class.
Initially the state management logic (state switching etc.) was part of the PlayerMovementSystem class, but it was later refactored into it's own class, which André then expanded upon when developing the AI system.

I have chosen to implement the Walking, Crouching and Sprinting states as substates of Grounded. This lets me keep a lot of the keep most of the general logic in the Grounded state and only extend or override the behaviour where necessary in the substates.
One of the biggest challenges was deciding how to share information between states. A prime example of this was when I was working on the Crouching state. Initially i had thought to just have a crouching state  and needed to keep track of whether the player had actually uncrouched or not.
In the end I kept the logic for crouching/uncrouching separate from the state machine itself and used an event to determine when to change state. I still feel like I could somehow do this in a cleaner way, but it worked and that was the most important part.
Overall, it feels like using a state machine was a good choice and I would probably use a similar approach if I were to do it again. However the last couple of weeks i have also been wondering if maybe a stack based approach would be more suited.

When developing the movement system I put a lot of effort into making an API for it that the other members could utilize. This meant that I needed to think about what would be useful to expose and how to make it intuitive. These details were also something we briefly discussed whenever the need would occur. As a part of this effort I made sure to provide some documentation for each public facing property and method that other group members might have use for. The goal for this was to let them have helpful information pop up in their code editors when working interfacing with this class. I also added tooltips in the unity editor for properties that I thought could be a bit vague to work with.

Another challenge somewhat separate from the code was making the movement feel satisfying. I still don't think I have succeeded in this, but it is far better now than at the beginning. The limited animations might also play a part in how this is percieved. Overall, if I had a little more time to do a final refactoring to get rid of some repetitive code across the behaviour classes and do some tweaks here and there I would be happy with it.

# Bad code

**Snapshot system**

The world snapshot/saving system was not something we had given much thought until about midway through the semester.
Because of that we had not given much thought to how our data was structured. In my opinion it would be best to have a data layer that deals with all instancing of characters and stateful world elements, like puzzles.
Having that in place would make it much simpler to aggregate all the data for a data set (such as the set of all characters), as well as reloading/generating a scene from a data set.

When I started working on the snapshot system I decided that it would be best to just make a simple system that any MonoBehaviour could opt into by implementing an interface. The system relies on the unity SendMessage feature that lets us invoke methods on other GameObjects. I then made an interface that specifies the implementation of these methods. I have chosen to create two separate interfaces for writing data to a snapshot and reading data from one. The idea behind that was to make the implementation of these methods as straight forward as possible for the rest of my group, if they were to make use of it. However this was downprioritized due to lack of time, and the system is largely unused, despite being functional. This approach of using interfaces to segregate the functionality based on what action is being performed is one of the parts that I like the most about this code.

My biggest gripes with the system comes down to a lack of clarity in what actually happens, as well as some implementation details which are quite messy. A snapshot is made by calling a static method of the SnapshotManager class. This class will then get a list of all non-static GameObjects in the scene and send the MakeSnapshot message to all of them. As previously mentioned, I would much rather have a layer in between that deals with the collection and loading of data. This layer could then instantiate objects and set their state during creation. It would also allow more flexibility in how data is collected. For instance, collecting all the AI state might require a more sophisticated approach than saving a collection of physics objects. Having this layer in between would also make it easier to provide different data structures tailored to the type of objects we are saving, providing more effiecient storage. The problem with this approach for us was that it would require quite a lot of work to retroactively add this to our systems, which we did not have time for.

Another flaw in the current system is the lack of persistent unique identifiers for each object. This complicates the data loading phase. As an example, say we had a bunch of houses that each have a door that we can open and close. If we save the state of each door, how do we know which data entry belongs to which door? Unity documentation specifically states that the UUIDs used internally are not persistent and should not be used for this. The solution that i came up with was to just generate a fully qualified name for each component based on the hierarchy of GameObjects it belongs to. The problem with this is that the names can get very long, which is not ideal. An even bigger issue however is that changing the scene hierarchy could unexpectedly lead to loss of saved data, which is really bad and not sustainable in the long run.



# Reflection

Going into this project I had some prior experience with Unity. This definitely helped as I feel quite confident in navigating the documentation whenever I need to read up on something. I also know quite familiar with the c# language. All of this meant that the coding aspect of this project felt fairly comfortable for me. This is however the first time I have worked collaboratively on a unity project, and I have gotten a few experiences to carry forward regarding the importance of proper planning and information pooling. Now that the project is complete i feel like i could have done a bit more to guide the less experienced group members. During the project we mostly worked on our own corners of the project, doing little to stick our hands into other peoples code. While I think it's important to respect the autonomy of each group member, it is also important to make sure we are all on the same page, and that is something I should improve on.

Overall, our group did very little documentation outside of the code. 

Regarding my own work, I wish I could have gotten a little more progress done before the end of the project. This semester has been especially stressful with many seperate group projects/assignments across different courses. On top of that I had to take an additional course, adding more straws to the camel's back (I am the camel in this instance). Luckily my back didn't break. My sanity, on the other hand, is completely gone (as evidenced by the three sentences you've just read). I had really wanted to add a database/networking component by now, but the last few weeks of the semester were far too busy. That said, if I had managed my time better I am sure I would have made it. This is also something I could improve on. That said, I still think the project ended up fairly decent, especially with the time allotted.
