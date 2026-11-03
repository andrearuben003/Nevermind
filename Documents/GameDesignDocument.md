# Game Design Document Nevermind 

Andrea Ricciardi Christian Vezzoli Shaan Vashisht Matteo Vannini Andrea Coscia Vanoni 

## **1 Overview and vision statement** 

Nevrmind is a puzzle-platformer in which you can control what does and doesn't exist in the world through your memories. The game's main challenge should come from finding the correct sequence of actions to navigate each puzzle, not from platforming challenges.

## **2 Gameplay** 

### **2.1 Main Mechanics** 

#### **2.1.1 Player Mechanics** 

- **Movement:** The player has a physics based character controller that allows them to move sideways. 

- **Memory System:** The player has a set of memories that they can remember/forget. The elements in the level change their behavior depending on the memories the player currently remembers. When the player forgets a memory, a thought bubble containing it is dropped at their position. The player has to interact with the bubble to remember the memory. 

#### **2.1.2 Level Elements** 

- **Pressure Plate:** Sends signal when one or more objects are placed on top (the amount of weight needed can be specified). 

- **Button:** Sends signal when the player collides with it (only once). 

- **Lever:** Sends signal when the player collides with it (can switch on/off repeatedly). 

- **Moving Platform:** Platform that moves along a set path. 

- **Rotating Platform:** Platform that rotates. 

- **Gate:** Opens/Closes when signaled. 

- **Box:** Block that can be pushed or grabbed by the player. 

- **Ball:** Can be picked up and thrown. 

- **Kill Zone:** Kills the player on contact. 

- **Slime\*:** Surface mod, makes objects stick together. Big slime area can be swam through. 

- **Light Beam\*:** Emits a ray of light, can be rotated by the player. 

- **Light Beam\* - Solid:** Ray of light that acts as a physical wall/platform. 

- **Light Beam\* - Deadly:** Ray of light that kills the player. 

- **Light Receiver:** Sends signal when reached by a ray of light. 

- **Mirror:** Redirects light. 

- **Memory Screen:** Makes the player forget fragile memories when passed through. 

- **Memory Font:** Restores the player’s semi-permanent memories. 

- **Ice\*:** Surface mod, makes objects slide. 

- **Rubber\*:** Surface mod, makes objects bounce. 

- **Rocket:** Kills player if hit, can be used as a platform. 

- **Gravity\* Flux:** Pushes objects in a given direction. 

- **Gravity\* Point:** Attracts objects towards the point. 

_\*tied to a memory that enables/disable the object._ 


#### **2.1.3 Memories** 

- **Material (eg. Stone):** Manifests objects of a given material. 

- **Movement:** Allows the player to move. 

- **Jumping:** Allows the player to jump/Makes the player jump uncontrollably. 

- **Time:** Pauses the game when forgotten. 

- **Hands:** Allows the player to pick up and throw small objects (eg. balls, small boxes, memories). 

- **Object Specific:** See previous section. 

#### **Memory Types** 

- **Permanent:** Cannot be forgotten. 

- **Semi-Permanent:** Can be forgotten, will return to the player at a memory font. 

- **Fragile:** Can be forgotten, will be destroyed when passing through a memory screen. 

### **2.2 Level Design** 

A level is defined as a single scene composed of a succession of rooms, each occupying one screen and containing one puzzle. Each level is designed around a theme (eg. ”forest”, ”volcano”, ”temple”) and a central mechanic (eg. ”boxpushing”, ”slime”, ”lights”) that is explored throughout the level’s rooms. 

## **3 Characters\***

## **4 Story\***

## **5 World\***

## **6 Media list** 
_Will be defined once the initial prototype is ready_

## **7 Technical specification\***

## **8 Team** 

### **8.1 Developers**
   - Andrea Ruben Ricciardi 
   - Christian Vezzoli 
   - Shaan Edoardo Vashisht 
   - Matteo Vannini 
   - Andrea Coscia Vanoni

### **8.2 External Help**
   - Andrea Assabese (Artist)
   - Ulrike Lanting (Artist)
   - Matteo Linzas (Music)

## **9 Special Thanks** 

## **10 Team meetings and Deadlines** 

### **10.1 Week 1** 

#### **10.1.1 October 6 team meeting** 

The team agreed upon the core idea and goal of the game: collecting memories and use them to solve puzzles. 

Discussed main game mechanics: the team is split between memories being physical objects or items that stay in an intentory of sort (or maybe a hybrid). 

Discussed the way memories can be remebered and forgotten: the team is undecided between a fixed place or everywhere (this also depends on the way memories work, so that has to be decided first). 

Discussed upon the idea of _levels_ : the team is split between a metroidvania style, based more on exploration, and a hardcore puzzle style, based more on mechanical skill and puzzle solving. 

Sketched some potential ideas for memories: meterials (like bricks), gravity, yourself (become a ghost), light, dark, time (may be used to pause the game), enemies. Sketched some more ideas like collectibles, bosses (that might unlock memory ’slots’), ’bad’ memories (debuffs forced in some cases), frail memories (they can break). 

#### **10.1.2 October 13 deadline** 

- Bring a prototype based on the way memories work: either them being physical objects or items in an inventory (everyone) 

### **10.2 Week 2** 

#### **10.2.1 October 13 team meeting** 

Prototypes from everyone have been seen or tested to discuss the game design. After playtests and discussions, the team agreed upon: 

- Making levels with more screens to keep a theme (like Celeste) 

- Making memories physical objects that can be stored in an inventory or dropped onto the ground 

- Letting the player remember and forget everywhere 

- The number of memory slots (how many memories can be held at one): the player starts with 1 or 2 and defeating bosses unlocks more slots (maximum of 5 to 7) 

Due to the nature of the game, memory classes have been designed: 

- Permanent: typically given at the end of a level, they are permanent upgrades (like double jump) 

- Semi-permanent: they only stay with the player during a level  

- Frail: they can break if the player touches some specific objects (like in Portal) 

Sketched some more ideas for memories: hands (ability to grab items), weapons? (can be too violent), object properties? (maybe better to stick with the objects themselves), rubber tile/spring (forces you to bounce/jump). 

Sketched ideas for bosses: forces you memories and you have to adapt; gives you ’bad’ memories. 

The idea of collectibles remains viable, it will be discussed later on. Andrea Ruben has shown the team his drawings and sketches for a potential game style. 

#### **10.2.2 October 20 deadline** 

- Sketch levels (even on paper) to test for the viability of some specific memories. Try to test as many as possible with as many ideas as possible (everyone) 

### **10.3 Week 3** 

#### **10.3.1 October 20 team meeting** 

The team members viewed each other’s level sketches, taking note of common themes and mechanics. 

Based on these observations, notable mechanics were added to the GDD and the team agreed to begin work on implementing these mechanics in the Unity project. 

#### **10.3.2 October 27 deadline** 

- Add Unity project to repository ASAP (Andrea Ruben) 

- Add character controller ASAP (Andrea Ruben) 

- Implement Memory System skeleton ASAP (Shaan) 

- Implement mechanics: 

|Andrea Ruben|Christian|Shaan|Matteo|Andrea Coscia|
|---|---|---|---|---|
|Box|Light Beam|Memory System|Moving Platform|Pressure Plate|
|Ball|Light Beam - Solid|Memory Screen|Rotating Platform|Button|
|Kill Zone|Light Beam - Deadly|Memory Font|Ice|Lever|
|Rocket|Light Receiver|Gravity Flux|Rubber|Gate|
|Slime|Mirror|Gravity Point|||

### **10.4 Week 4** 

#### **10.4.1 October 27 team meeting** 

The team showed the work they did over the week and shared feedback on what to improve.  

Andrea Ruben: completed most of the work on the slime system, has to work out boxes 

sticking together. 

Christian: completed work on light system, must work on light-overlap mechanic. Shaan: completed work on the memory system and gravity mechanics. 

Matteo: completed work on platforms and rubber, ice is mostly working but implementation must be revisited. 

Andrea Coscia: initial implementation of components is workable, will refine graphical behaviors. 

#### **10.4.2 November 10 deadline** 

|Andrea Ruben|Christian|Shaan|Matteo|Andrea Coscia|
|---|---|---|---|---|
|Slime connection|Light contamination|Kill Zone|Revisit Ice|Button/Lever/Plate graphics improvements|
|Box|Light system improvements|Level System||Weighted Plate system|
|Ball|||||
|Rocket|||||

### **10.5 Week 5**