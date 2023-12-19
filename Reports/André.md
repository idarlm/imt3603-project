# Desired Weighing

| Description         | min  | def  | max  |
| ------------------- | ---- | ---- | ---- |
| Gameplay video      | 5    | 10   | 20   |
| Code video          | 0    | 10   | 15   |
| Good Code           | 10   | 20   | 30   |
| Bad Code            | 10   | 20   | 30   |
| Development process | 10   | 20   | 30   |
| Reflection          | 10   | 20   | 30   |



# Overview

Throughout the project my primary task has been to develop  the enemy rats  that hunt for our titular mouse. This primarily involved, but was not limited to, the combination of a state machine communicating with an Unity Animator, a graphically programmed state machine handline the animation transitions. 

- Enemies

  - AI State machine
  - Animation
  - Stealth Mechanics
  - Modeling
  - Pathing system
    - In world nodes acting as traversable linked lists
    - Cages with a destination point for  the AI and player on capture

- Camera

- FX and Audio

  - Changes in music based on detection
  - Sound FX when nearly spotted, chase start, chase end, capture
  - Changes in  Visual FX in response to being hunted or captured.

  

# Good Code

### AI  Settings

In contrast to the bad practice of using magic numbers, as mentioned in the Bad Code section, the AI system gradually moved towards having parameters exposed as fields tweakable in the Unity UI, and eventually towards using a single object to broadcast these values to the various enemies on startup of a scene. 

[AISettingsController](https://github.com/idarlm/imt3603-project/blob/be708eb4057fefc8854ebd3d6d808fdda2d29a94/Assets/Scripts/AIController/Settings/AISettingsController.cs#L1) works in tandem with the [AISettingsManager ](https://github.com/idarlm/imt3603-project/blob/main/Assets/Scripts/AIController/Settings/AISettingsManager.cs) singleton to pass values to the AI on the map. The purpose of using  a singleton as a communications channel is avoiding the need to reference the AISettingsController when placing an AI in the map. While singletons can feel questionable, I often found myself reaching for the pattern when I needed to communicate between  objects without having to explicitly pass them a reference, which was  more or less a must for objects such as light sources and AI that needed to be able to know about something without incurring the work load of manually setting up a reference. 

### Illumination System

An important component in the game was the ability to tell if the player was lit up, with reasonable accuracy. For that purpose I set up a system where  each light source was equipped with an [IlluminationSource](https://github.com/idarlm/imt3603-project/blob/main/Assets/Scripts/Illumination/IlluminationSource.cs) script. On Start, these register with the [IlluminationManager](https://github.com/idarlm/imt3603-project/blob/main/Assets/Scripts/Illumination/IlluminationManager.cs) singleton, allowing lighting to be calculated and added  to the total player illumination on demand. 

Rats  check if they are within range of the player, if they are, they call `GetIllumination` on the singleton, which then invokes the  light calculation on all light sources that registered with it on start. The range of the light  source component is used to determine  if any lighting should be calculated at all, and if it is, the intensity is used  attenuated with`AttenuateSquare` based  on distance.

The illumination is calculated once per square. If an AI has already queried the lighting manager, it will reuse the already calculated values to serve any other calls for that frame. An improvement would be to move this process to the physics update, but it hasn't been problematic performance wise. 

## State Machine

## State



# Bad Code

## General

### Magic Numbers

A frequent sin  throughout the project has been the dreaded magic number, where during testing a new feature, plain numbers have been put in conditions and similar, where they then often remained. While it might make something work then and there, the need to change these values often  arrive sooner rather than later, and it was frequently hard to track down where exactly these values were set and used. The obvious solution is to utilize serialized fields exposed in the Unity editor, allowing easy and rapid tweaking during development, which is what I moved towards more and more.



## FX System

While the FX systems  works OK as is, when I wrote it a lot of repeating  patterns turned up  for  performing some  transition  over time, such as Increasing the volume of a song to fade it  in, or fade the screen to black. An improvement would be to write a generic system able to  que up and progress these transitions per frame, and then let the implementation of a base class determine what's actually done each frame. Since the sound system was added the last few days, this was not done, but it'll something I'll be considering if I  keep the project going.

[AudioFade](https://github.com/idarlm/imt3603-project/blob/be708eb4057fefc8854ebd3d6d808fdda2d29a94/Assets/Scripts/FX/Audio/AudioController.cs#L11C29-L11C29) looks suspiciously similar to [Effect](https://github.com/idarlm/imt3603-project/blob/c238026ec51e6ca1c365c1ca43056a302bc8ffba/Assets/Scripts/FX/Visual/Effects/Effect.cs#L9)

## AI System

### Communication with other objects

While I think generally the state machine pattern works well by reducing various if statements, the system could benefit from being better structured. [AIState]() is the class defining the common behavior of the states the AI is in. As it is now, it is hard to track where various values gets uploaded to accompanying systems. The  Enter and Exit methods on this base class should probably have handled more of this logic, something which instead has repeated itself across the implementations in the subclasses, a ripe source for bugs. [AIStateMachine](https://github.com/idarlm/imt3603-project/blob/main/Assets/Scripts/AIController/AIStateMachine.cs) interacts with quite a  few systems through its AIState object. When this becomes hard to track, debugging the enemies becomes quite challenging, having to look through X amount of state subclasses, track how they transition and checking whether the correct operation is being done for each class. Moving more  logic into methods on the base class could allow logic to be easily located, and then just have the subclasses invoke different logic based on the needs of that state. Regardless of the solution, the system has grown big and hard to track.

### State granularity

Some `Update` implementations grew to be too messy during development, with several nested if statements. A solution to this, which was partially applied, would be to split AIStates up into smaller ones, though it's a balance between having to maintain too many small classes versus having to maintain messy `Update`logic like in [IdleState](https://github.com/idarlm/imt3603-project/blob/c238026ec51e6ca1c365c1ca43056a302bc8ffba/Assets/Scripts/AIController/IdleBehaviour/IdleState.cs#L52)



## AIInteractionoFXManager

While not the worst, it does have a slight smell to it. A good object oriented principle from SOLID is the Separation of Concern. [AIInteractionFXManager](https://github.com/idarlm/imt3603-project/blob/c238026ec51e6ca1c365c1ca43056a302bc8ffba/Assets/Scripts/FX/AIInteractionFXManager.cs#L17C33-L17C33) is responsible for queuing up FX in response to AI interaction, but it is also used by the rats to get information about the players capture state. A cleaner approach would be to let the state of enemy-player interaction be handled by one class, then have the FX system communicate with it and cue FX in response to state change. 









# Reflection













