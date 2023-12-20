# Desired weighing

|Description | min | def | max |
|----|----|----|----|
|Gameplay video | 5 | 10 | 20 |
|Code video | 0 | 10 | 15 |
|Good Code  | 10 | 20 | 30 |
|Bad Code | 10 | 20 | 30 |
|Development process | 10 | 20 | 30 |
|Reflection | 10 | 20 | 30 |

# Good code

**Player movement system**

I decided from the beginning that I was going to use the state pattern, since I wanted to keep the logic for each state seperated.
Without this seperation the code would get very messy with lots of if checks for many different edge cases which would make the code hard to read and maintain.

The code is divided into 6 state behaviour classes: 
- FallingState
- GroundedState
    - WalkingState
    - CrouchingState
    - SprintingState
- PushingObjectState

These 6 classes are encapsulated by the PlayerMovementSystem class, which is passed along as context to the active state object. Most of the state data is hoisted in this class.
Initially the state management logic (state switching etc.) was part of the PlayerMovementSystem class, but it was later refactored into it's own class, which André then expanded upon when developing the AI system.

I have chosen to implement the Walking, Crouching and Sprinting states as substates of Grounded. This lets me keep a lot of the keep most of the general logic in the Grounded state and only extend or override the behaviour where necessary in the substates.
One of the biggest challenges was deciding how to share information between states. A prime example of this was when I was working on the Crouching state. Initially i had thought to just have a crouching state  and needed to keep track of whether the player had actually uncrouched or not.
In the end I kept the logic for crouching/uncrouching separate from the state machine itself and used an event to determine when to change state. I still feel like I could somehow do this in a cleaner way, but it worked and that was the most important part.
Overall, it feels like using a state machine was a good choice and I would probably use a similar approach if I were to do it again. However the last couple of weeks i have also been wondering if maybe a stack based approach would be more suited.

Another challenge somewhat separate from the code was making the movement feel satisfying. I still don't think I have succeeded in this, but it is far better now than at the beginning. The limited animations might also play a part in this. Overall, if I had a little more time to do a final refactoring to get rid of some repetitive code across the state classes and do some tweaks here and there I would be happy with it.

# Bad code

**Snapshot system**

The world snapshot/saving system was not something we had given much thought until about midway through the semester.
Because of that we had not given much thought to how our data was structured. In my opinion it would be best to have a data layer that deals with all instancing of characters and stateful world elements, like puzzles.
Having that in place would make it much simpler to aggregate all the data for a data set (such as the set of all characters), as well as reloading/generating a scene from a data set.

When I started working on the snapshot system I decided that it would be best to just make a simple system that any MonoBehaviour could opt into by implementing an interface. 
The system relies on the unity SendMessage feature that lets us invoke methods on other GameObjects. I then made an interface that specifies the implementation of these methods.
I have chosen to create two separate interfaces for writing data to a snapshot and reading data from one. The idea behind that was to make the implementation of these methods as straight forward as possible for the rest of my group, if they were
to make use of it. However this was downprioritized due to lack of time, and the system is largely unused, despite being functional. This approach of using interfaces to segregate the functionality based on what action is being performed is 
one of the parts that I like the most about this code.

My biggest gripes with the system comes down to a lack of clarity in what actually happens, as well as some implementation details which quite messy. A snapshot is made by calling a static method of the SnapshotManager class. This class will then 
get a list of all non-static GameObjects in the scene and send the MakeSnapshot message to all of them. As previously mentioned, I would much rather have a layer in between that deals with the collection and loading of data. This layer could then instantiate objects and set their state during creation. It would also allow more flexibility in how data is collected. For instance, collecting all the AI state might require a more sophisticated approach than saving a collection of physics objects. Having this layer in between would also make it easier to provide different data structures tailored to the type of objects we are saving, providing more effiecient storage. 

Another flaw in the current system is the lack of persistent and unique identifiers for each object. This complicates the data loading phase. As an example, say we had a bunch of houses that each have a door that we can open and close. If we save the state of each door, how do we know which data entry belongs to which door? Unity documentation specifically states that the UUID that is used internally are not persistent so we can't use that. The solution that i came up with was to just generate a fully qualified name for each component based on the hierarchy of GameObjects it belongs to. The problem with this is that the names can get very long, which is not ideal. An even bigger issue however is that changing the scene hierarchy could unexpectedly lead to loss of saved data, which is really bad and not sustainable in the long run.


# Reflection
