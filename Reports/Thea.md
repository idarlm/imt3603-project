|Description | My Weight |
|----|----|
|Gameplay video | 10 |
|Code video | 0 |
|Good Code  | 20 |
|Bad Code | 20 |
|Development process | 20 |
|Reflection | 30 |

## Good code
What I define as good code: clean, easy to read, not over complicated, modular...

- interactionTextUI
    - Lateupdate for the UI text
    - teksta følge kamera
    - static
    - Show and Close can be used in diiferent parts of code, kunne enkelt vårte oppdatert til å ta inn paramter og såtte custom text på ulike ting.
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


Både og:
- interacttrigger
    - good: clean, easy to read, using UI system to display/close text, and event system
    - input.keydown instead of the playerinput system
    - checking gameObject name -> what if multiplayer? also not good to hardcode string

## Bad code
What I define as bad code: hard to understand, even with comments. Not reusing code, hardcoded values...
   
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
 
- visibilityUI

## Reflection
- Mål vs resultat
  - litt høge ambisjona
  - uklare forventninga
  - ikkje fullført men fornøgd med ka vi he klart
    
- språk og engine valg
  - unity, ingen erfaring
  - gikk fint å lære
  - C# nært nok c++ til at d gikk fint
    
- kode struktur
    - litt rot, sia folk e vant med ulike ting men ett system på d
      
- ka ej he lært
    - unity
    - input systemet
    - Serializefield
    - UI
    - assets
    - gi objekt logikk
      
- utfordringa
  - ulike nivå i kunnskap
  - vanskelig å holde følge
  - logikken til puzzelane
  
- gruppe dynamic
    - litt for ulike kunnskaps nivå og ulike måta å gjere ting på
    - snakka mer om ambisjona i starten.
      
- prosessen
    - ville brukt mer tid på dokumentasjon -> issueboard, kommentara, korleis ting funka
      
- tid
    - starta seint, mer tid
      
- videre arbeid
    - potensiale til å bli et bra spel, må ferdi stillast, so kunna en laga flere level
      
- konklusjon
    - overall lært mykje, spennande prosess. Ønske om at ting hadde vore mer organisert i faget. 


