# Stealth Rogue

Daiwei (David) Lu and Don Choi

Art by Jack Rong

Music by Jesse Li

This game is a roguelike action/stealth game under development.  Current game assets included are royalty free non-final standins.

## Gameplay Ideas
-Goal of the game: The goal of the game is to free souls and escape. To free souls, you have to kill these angler fish like creatures with lights over them
that patrol the area. They're not able to attack the player, but will alert the boss of the level if one of them sees you and are not killed fast enough. Once
all these creatures have been killed on the level, you can escape the level. Alternatively, the boss can be killed when danger is 100% to escape the leve
Notes: Not the final idea, but the core concepts will probably be present in some way. I want to give something to the player to be "distracted" by in order 
for them to have to split attentions between the boss and some other task. I've noticed most popular horror games do this, and I want to include some form of 
this gamplay.
-Player: The player is given 2 tools at the beginning of the game: a flashlight and a basic weapon. The flashlight allows the player to see details in the environemnt,
such as items to pick up or traps. However, when the light is flashed on enemies, the enemies will approach the light to check the source of it. The basic weapon
deals normal damage to an enemy in a melee range. The player is also given a roll, allowing the player to quickly move out of the way (likely limited by a stamina bar,
or make the movement quick but still slower than walking normally, done in a few games)

Danger Percentage: From 0-100, danger increases the longer you stay in a level, and can increase/decrease from various events. Taking damage would increase
danger, healing would decrease danger, attacking the boss decreases danger, etc. The boss can get various new abilities as danger levels rise. In addition, 
the boss becomes more aware of the player's location as danger rises. Once the meter hits 100%, danger can no longer be decreased, and the boss is killable
but also does double dmg (just an idea for now, not sure about this one).
Notes: Gives us a lot more control on the flow of a level. Allows us to make levels feel climatic with proper buildup while still keeping the roguelike elements.

General Concepts/Ideas: Ideally, each boss should have an "oh fuck" moment. What is meant by this is anticipation/fear followed by a release of that
tension and segwaying into a more action filled sequence. This is likely going to be the gameplay loop for this game, and what will keep a player 
engaged and coming back for more. Each boss's gameplay is designed to deliver on this, and the music/art should follow suit.
Note: One question still being worked on is how a player escapes a boss. It needs to feel real and tense while not being unfair. However, if escaping becomes too easy, the
boss will feel cheap.

Bell Boss
-Gameplay: Rings a bell to go invisible, and has to ring the bell again to reappear. The boss can only attack the player when the 
boss is visible. The boss will likely not have "true" invisibility but instead a blur effect if the player is watching carefully.
-Emotion: The ideal state of mind the player should be in is to be constantly looking at the surroundings while doing tasks (looting, exploring, etc.) 
in the background. This boss should make even the mundane exciting and tense because the anticipation of the boss showing up is what drives the player.
The emotional release for this boss in particular has to be nailed, and the boss uncloaking behind you should be an explosive moment.
-Notes: Music has to allow for the bell to be clearly heard and not be mistaken in the music. Could likely use for more atmospheric 
music rather than driven by a specific melody.

Trapper Boss
-Gameplay: Sets traps across the map. Traps are invisible unless a light is flash on them. When a player is caught by a trap, they take damage, have to spam a button to
get out, and the boss is alerted of the exact location of the player and will begin to chase them. It creates an interesting dynamic in which the player wants to keep their
flashlight on to look for traps, but doing so can put them at an advantage as enemies will be attracted to the light
-Emotion: The gameplay, music, and art need to be pushing the player to panic. This becomes very emotionally engaging to the player who needs to stay calm and continue to avoid
traps even when the stakes are raised.

Eater Boss
-Gameplay: Aggressively finds the location of the player and charges the player with its body. Eats/destroys everything in its path, including other enemies, items, etc. Fast but
can't turn effectively, meant to be avoided with rolling.
-Emotion: Player should feel overwhelmed and relying on reactions. This is a lot more action packed rather than the anticipation/stress of the other bosses.
-Notes: Might need to tweak the gameplay, doesn't quite have a big enough "oh fuck" moment like the other bosses.

## Current Task List

David - Replace existing systems with activatable monobehaviors

Don - Enemy goal system

Jack - Art Prototyping

Jesse - Music Prototyping


## Next Meeting

4:00 PM Saturday, March 23

Lewis 408


# Comment Structure

Necessary for making any modifications far simpler

## Functions

/\*

 \* Description: _what does following method do_

 \* 


 \* Dependants: _whenever you create a function calling this method, add it to this list_

 \* 

 \* _any additional comments_

 \*/

## Variables/Other Objects

/*

 \*Purpose: _what variable is used for_

 \*

 \*Dependants: _any methods accessing this variable_

 \*

 \*/
