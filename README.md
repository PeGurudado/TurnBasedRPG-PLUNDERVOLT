# TurnBasedRPG-PLUNDERVOLT

Challenge description

Game Developer Challenge

This mini-challenge aims to verify if the game developer candidate understands the programming fundamentals in C# and Unity.

Challenge
Deliverables: Document (PDF) and code in GitHub
Pre-requisites: Programming fundamentals
Overall
As a mini-challenge for a mentee game developer position, it aims to identify your work process. In this project, the quality of your deliveries
is what matters the most. Showing your thought process and building scalable, maintainable, and high-quality code will be the main focus
for the task.
Also, you can use this opportunity to grow your portfolio!
Description
You will be creating a turn-based RPG combat system. The game can start with a battle scene without real art, where a basic background
and basic art can represent the characters. For simplicity, it will consist of a team of two characters controlled by the player against two
enemies, although the code needs to support adding more in the future (scalability).
As a turn-based RPG, the player will be able to:
Use a basic attack
Use a skill
The UI should only include buttons to select the attack or skills. Since art is not the game's focus, the game should be tested via debugging
logs. Feel free to do anything on the UI if there is time to implement it.
Main stats
The main stats of each character that a player can increase can be found below:
Strength: It affects basic attack damage.
Vitality: Increases the max HP of a character.
Dexterity: Increases the chance of hitting a target
Agility: Increases the dodge chance
Intelligence: Increases mana.
Secondary stats
There are secondary stats that are calculated directly by the main stats:
HP: Each point in vitality increases the HP by 2. Each character starts with 5 HP.
MP (Mana): Each point in intelligence increases the MP by 3. Each character starts with 10 MP.
Dodge: Used to calculate if an attack/skill is missed. Each point in agility increases the dodge chance by 3%. Each character starts with
a 5% dodge.
Hit ratio: Used to calculate if an attack/skill hits. Each point in dexterity increases the hit ratio by 4%. Each character starts with a 20% hit
ratio.

Hit chance
Each attack and skill has its effect and chance to hit. When using an attack/skill, it is considered a hit using the following formula:

Also, note that effects are not applied if the attack/skill misses.
On the other hand, some skills are used to buff/heal allies. These do not have a chance to miss!
Skills
The skills to be implemented can be found below:
Basic attack
Effect: Deals damage equal to the character’s strength
Chance to hit: 60%
MP consumed: 0

Fireball
Effect: Deals damage equal to 2 plus the character’s intelligence. The target has a 30% chance of being BURNED for 3 turns.
Chance to hit: 60%
MP consumed: 3

Inferno
Effect: Hits all enemies. Deals damage equal to intelligence * 2. Targets that are burned take an additional 30% damage.
Chance to hit: 70%.
MP consumed: 7

BURNED (Condition)
Effect: The character loses two hp at the start of their turn.

Heal
Effect: Heal an ally for 2 * intelligence.
Chance to hit: -.
MP consumed: 4

Continuous healing
Effect: Apply continuous healing where the target heals HP at the start of its turn. This effect lasts three turns and the amount healed equals
the skill’s user intelligence. The target immediately heals 3 HP after receiving the healing.
Chance to hit: -.
MP consumed: 6
1 Hit = (Attack/skill)ChanceToHit + AllyHitRatio - EnemyDodgeChance

Tasks
The following items are mandatory for the delivery of this challenge:
1. Document the processes you are using for building the system.
a. This is a PDF file that may contain text, flowcharts, etc.
2. Implement the battle skill system according to this task’s description.
a. Two characters controlled by the player
i. Character 1 has the following skills:
1. Fireball
2. Inferno
ii. Character 2 has the following skills:
1. Heal
2. Continuous healing
b. Two enemies
i. Skills can be the same as the player’s character.
ii. The enemies can have a simple AI to choose what their action is.
1. Randomly choose the action.
c. When a character falls to 0 HP or below it, it will be permanently removed from battle.
d. Skills can’t be used if the user does not have enough MP to use it.
What Plundervolt expects
Plundervolt will review and analyze the following items (in order of priority):
1. Code Quality
a. Scalability
b. Maintainability
2. Code architecture
a. Turn-system
b. Usage of design patterns
3. Documentation
a. Thought process
b. Code comments
