# Desired Weighing

| Description         | min  | def  | max  |
| ------------------- | ---- | ---- | ---- |
| Gameplay video      | 5    | 16   | 20   |
| Code video          | 0    | 0    | 15   |
| Good Code           | 10   | 27   | 30   |
| Bad Code            | 10   | 27   | 30   |
| Development process | 10   | 10   | 30   |
| Reflection          | 10   | 20   | 30   |





# Overview

Throughout the project my primary task has been to develop  the enemy rats  that hunt for our mouse character. At the core of this is the AIStateMachine class, building on a template made by Idar. Below is a diagram that shows the relationship between that class and other important components of the system I've built up throughout the project. 

- ![gampeprog](images/gampeprog.png)


_An overview of the most important classes and how they relate to the AI system_

# Good Code

## AI Settings

In contrast to the bad practice of using magic numbers, as mentioned in the Bad Code section, the AI system gradually moved towards having parameters exposed as fields tweakable in the Unity UI, and eventually towards using a single object to broadcast these values to the various enemies on startup of a scene. 

[AISettingsController](https://github.com/idarlm/imt3603-project/blob/be708eb4057fefc8854ebd3d6d808fdda2d29a94/Assets/Scripts/AIController/Settings/AISettingsController.cs#L1) works in tandem with the [AISettingsManager ](https://github.com/idarlm/imt3603-project/blob/main/Assets/Scripts/AIController/Settings/AISettingsManager.cs) singleton to pass values to the AI on the map. The purpose of using  a singleton as a communications channel is avoiding the need to reference the AISettingsController when placing an AI in the map. While singletons can feel questionable, I often found myself reaching for the pattern when I needed to communicate between  objects without having to explicitly pass them a reference, which was  more or less a must for objects such as light sources and AI that needed to be able to know about something without incurring the work load of manually setting up a reference. 

### Illumination System

An important component in the game was the ability to tell if the player was lit up, with reasonable accuracy. For that purpose I set up a system where  each light source was equipped with an [IlluminationSource](https://github.com/idarlm/imt3603-project/blob/main/Assets/Scripts/Illumination/IlluminationSource.cs) script. On Start, these register with the [IlluminationManager](https://github.com/idarlm/imt3603-project/blob/main/Assets/Scripts/Illumination/IlluminationManager.cs) singleton, allowing lighting to be calculated and added  to the total player illumination on demand. 

Rats  check if they are within range of the player, if they are, they call `GetIllumination` on the singleton, which then invokes the  light calculation on all light sources that registered with it on start. The range of the light  source component is used to determine  if any lighting should be calculated at all, and if it is, the intensity is used  attenuated with`AttenuateSquare` based  on distance.

The illumination is calculated once per square. If an AI has already queried the lighting manager, it will reuse the already calculated values to serve any other calls for that frame. An improvement would be to move this process to the physics update, but it hasn't been problematic performance wise. 

## State Machine

Following a state machine pattern allowed complex chains of logic to be separated into different AIStates. While there are some issues  with how I've used states mentioned under bad code, I believe that it's still a positive. An example of a state  where it's easy to track what the AI does is [InspectLocation](https://github.com/idarlm/imt3603-project/blob/main/Assets/Scripts/AIController/PatrolBehaviour/InspectLocation.cs). The Update method contains little logic, making it fairly simple to read and rework. Trying to work all the logic of the various states into a single class would be messy, hard to maintain and develop, and likely an awful nested mess of if else statements.

## Gizmos

This is more about the usability of scripts in the context of the Unity engine more so than how the code itself is written, so I question whether it fits in here, but I'll include it regardless as it was an important factor in making some of the scripts be practical in use. See the gameplay video at around the 20:30 mark for a visual demonstration.

The system used for AI pathing, the destination nodes to be specific, uses gizmos to indicate which node is next and which is the previous in the chain, making it easy to tell at a glance if you've set the wrong reference. Additionally, setting a node's next or previous to itself triggers a visual warning in the UI and a popup error message.This is done in the [OnDrawGirzmos()](https://github.com/idarlm/imt3603-project/blob/55b39a74f5a0339254330a6949c7e4bb34e56896/Assets/Scripts/Pathing/Waypoint.cs#L108) method. 

The same has been done with the rat, letting you see what their detection range is, their FOV where the player can be seen, target waypoint  and cage, etc. 

## StateFactory

To instantiate the various states I utilize [StateFactory](https://github.com/idarlm/imt3603-project/blob/55b39a74f5a0339254330a6949c7e4bb34e56896/Assets/Scripts/AIController/StateFactory.cs#L11). The current implementation takes an enum and returns an AIState subclass based on the provided enum. The benefit of this is that it's easy to swap out implementations if you want to write variations on the same behaviour. Rather than having to go through code and replace `new SomeState()` in all places it is used, I instead only have to swap out the appropriate line in the switch statement. A slight sin against this is instances where I instantiate a "temporary" idle where I need to know the type to use a method it has. Instead a temporary idle should have just been a distinct state that utilized some parameter on the AIContext object to determine how long to idle.

# Bad Code

## General

### Magic Numbers

A frequent sin  throughout the project has been the dreaded magic number, where during testing a new feature, plain numbers have been put in conditions and similar, where they then often remained. While it might make something work then and there, the need to change these values often  arrive sooner rather than later, and it was frequently hard to track down where exactly these values were set and used. The obvious solution is to utilize serialized fields exposed in the Unity editor, allowing easy and rapid tweaking during development, which is what I moved towards more and more.

## AI System

### Communication with other objects

While I think generally the state machine pattern works well by reducing various if statements, the system could benefit from being better structured. [AIState]() is the class defining the common behavior of the states the AI is in. As it is now, it is hard to track where various values gets uploaded to accompanying systems. The  Enter and Exit methods on this base class should probably have handled more of this logic, something which instead has repeated itself across the implementations in the subclasses, a ripe source for bugs. [AIStateMachine](https://github.com/idarlm/imt3603-project/blob/main/Assets/Scripts/AIController/AIStateMachine.cs) interacts with quite a  few systems through its AIState object. When this becomes hard to track, debugging the enemies becomes quite challenging, having to look through X amount of state subclasses, track how they transition and checking whether the correct operation is being done for each class. Moving more  logic into methods on the base class could allow logic to be easily located, and then just have the subclasses invoke different logic based on the needs of that state. Regardless of the solution, the system has grown big and hard to track.

### State granularity

Some `Update` implementations grew to be too messy during development, with several nested if statements. A solution to this, which was partially applied, would be to split AIStates up into smaller ones, though it's a balance between having to maintain too many small classes versus having to maintain messy `Update`logic like in [IdleState](https://github.com/idarlm/imt3603-project/blob/c238026ec51e6ca1c365c1ca43056a302bc8ffba/Assets/Scripts/AIController/IdleBehaviour/IdleState.cs#L52)

### State  Transitions

Currently it's a bit hard to track what states can lead to what other states, as state transitions happens in various if else statements in the Update method. A cleaner approach could be to do something like in Unity's Animator system, where transitions are caused by a set  of conditions that can be defined somewhere, preferrably in the Enter method, which then can be checked at the end of each Update call with the body above ideally only containing updates of appropriate values. Below is some pseudo code.

```c#
void SetTransitionCondition(enum type, string name, (type)=>bool condition, AIState targetState) {
	// register some transition
}

void Enter() {
	SetTransitionCondition(Float, "Lost Sight", (x)=>{x < 5.f}, AIStateLabel.Idle);
    SetTransitionCondition(Float, "Seen Player", (x)=>{x > 10.f}, AIStateLabel.Chase);
}

void Update(Context context) {
    // update context values
    
    CheckTransitionConditions(
    	// list of key value pairs
    );
}
```

It would have to be more robust than this to factor in multiple conditions being met for a transition, but it would be a start in cleaning the code up as transitions always be defined in one place. 

## FX System

While the FX systems  works OK as is, when I wrote it a lot of repeating  patterns turned up  for  performing some  transition  over time, such as Increasing the volume of a song to fade it  in, or fade the screen to black. An improvement would be to write a generic system able to  que up and progress these transitions per frame, and then let the implementation of a base class determine what's actually done each frame. Since the sound system was added the last few days, this was not done, but it'll something I'll be considering if I  keep the project going.

[AudioFade](https://github.com/idarlm/imt3603-project/blob/be708eb4057fefc8854ebd3d6d808fdda2d29a94/Assets/Scripts/FX/Audio/AudioController.cs#L11C29-L11C29) looks suspiciously similar to [Effect](https://github.com/idarlm/imt3603-project/blob/c238026ec51e6ca1c365c1ca43056a302bc8ffba/Assets/Scripts/FX/Visual/Effects/Effect.cs#L9). A  suitable solution could be something like:

```c#
public abstract class TransitionEffect<T>
{
    protected float TransitionTime = 0.5f;
    protected float ElapsedTime = 0f;

    public void Progress(float deltaTime)
    {
        ElapsedTime += deltaTime;
        ApplyEffect(volume);
    }

    public bool IsDone()
    {
        return ElapsedTime >= TransitionTime;
    }

    protected abstract void ApplyEffect(T argument);
}
```

Which is just a slight alteration of the original Effect abstract class and replacing the ApplyEffect parameter with a generic one.



## AIInteractionoFXManager

While not the worst, it does have a slight smell to it. A good object oriented principle from SOLID is the Separation of Concern. [AIInteractionFXManager](https://github.com/idarlm/imt3603-project/blob/c238026ec51e6ca1c365c1ca43056a302bc8ffba/Assets/Scripts/FX/AIInteractionFXManager.cs#L17C33-L17C33) is responsible for queuing up FX in response to AI interaction, but it is also used by the rats to get information about the players capture state. A cleaner approach would be to let the state of enemy-player interaction be handled by one class, then have the FX system communicate with it and cue FX in response to state change. As can be seen by the diagram, AIInteractionFXManager also depends on the classes that interact with Unity's audio and volume components and control these. This is  the only singleton where the dependencies are not all in the direction of the singleton. I consider it cleaner to have the singleton have some data and methods to do operations on these, functioning as a communications channel. The current solution just feels less clean.

# Reflection

## Development  Process

Fairly early on in the project it became pretty apparent that the group had a divide down the middle in terms of ambition and motivation. I think it's fair to say that one half got demotivated by the speed at which the other half worked as they expressed feeling left behind due to being less proficient, and the other half got demotivated by the friction this created. While I won't speculate on reasons or the why of the way things turned out too much, I will reflect briefly on  what I could have done better personally. 

While frequent no-shows and messages on discords at midnight prior to the set work-day is frustrating, I should still show the emotional maturity to not let frustrations impact the degree to which I utilize issues and other forms of git/jira related communications. Idar and myself were often physically present alone during some periods, during which we synchronized work through simple discussion. We didn't see this like a large problem as there was little overlap in the code we wrote, but I suspect not using issue tracking just increased the problems we had with group cohesion. 

Being the individual that shaped the direction the most in terms of art and goals, I should also have taken up the responsibility of guiding those less able to a greater extent. My impression was that we had a relatively flat structure where we were expected to take initiative and work dynamically, but I think there was a growing expectation that we assign others  tasks. At the same time, when we did try to provide things to do, a lack of a common programming lingo was often challenging. It's hard for me to know what people are capable of doing, or what they would need, or want, guidance on.



## Game Programming

While I had prior experience with 3D modeling and scene creation, I had no direct experience with any game engine in terms of coding, so the course provided an interesting opportunity for learning something closely related to a subject I was already interested in. 

While I have used  singletons before, I've not used them like I've done in this project where a lot of scripts had a need to be able to be placed on an object and not be manually set up to reduce work loads. While singletons can be a bit messy in that the top level visibility of its use in code can be subtle, I found it a very useful tool as a means of communicating between game objects. 

A regrettable point in our project was the fact that we never had a game that had enough mechanics in place until very late in the process, which meant there was little to playtest. I noticed the need for playtesting quite a bit when recording the playthrough as, while I had created a world that looked palatable, the interaction between player, enemy and the world did not add up to  a pleasant gaming experience. 

I could also have used less time on 3D modeling, asset hunting and world creation, though at the same time I don't think me contributing even more code would have benefited the group dynamic much. The fact that I was able to combine my existing passion for 3D content  and programming was what kept me going throughout the project, in addition to the interesting way one has to think when programming for a game engine. Quite different from what I'm used to from other courses. 

The interaction between my  AI Script and the animations I chose for my model and how they synergized  was very interesting. Since the look direction of the rat plays a large part in how they detect the player, I added in behavior such as playing an animation where the rat looks behind itself if the player is stood  closely behind the rat, or had the Animator play more frantic animations where the rat looks around it in a searching fashion when it has caught a glimpse of the player or "heard" something above a certain threshold. While there are plenty of quirks and bugs in the animations, I am still pleased with how 'lifelike' they turned out despite the relative simplicity of the underlying code. 















