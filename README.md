# Pedro-Moraes-Unity-Challenge

# Turn-based Battle Simulator - Documentation 

This README provides an overview of the project, including its core components and how they interact.
## Table of Contents
- [Character Information](#character-information)
- [Skills and Scriptable Objects](#skills-and-scriptable-objects)
- [Character Turn System](#character-turn-system)
- [Enemy AI Control](#enemy-ai-control)
- [Handling Character Death and Game Over](#handling-character-death-and-game-over)

## Character Information

In this project, each character possesses the following attributes:
- **Name:** The character's name.
- **Stats:** Various statistics that define a character's abilities (Mains stats, Secondary stats and Condition/Buffs stats).
- **Skills:** A list of customizable skills represented as Scriptable Objects. These skills can be found in the "Scriptable Objects/Skills" directory.

## Skills and Scriptable Objects

Skills in this project are represented as Scriptable Objects, specifically using the `Skill` class. These Scriptable Objects provide a versatile and customizable way to define various character abilities. Below is a breakdown of the key attributes and properties of the `Skill` Scriptable Object:

### General Information

- **Name:** The name of the skill.
- **Description:** A brief description of the skill's effect or purpose.
- **MP Cost:** The amount of mana (MP) required to use the skill.
- **Hit Chance:** The probability of the skill successfully hitting its target.
- **Target Selection:** Skills can target enemies or allies, depending on the `TargetEnemies` property, if targets an ally it will always hit.
- **Area Attack:** Indicates whether the skill affects all targets or just a single target.

### Skill Power

- **Base Power:** The base power of the skill, influencing its damage or effect potency.
- **Scale Type:** Determines how additional factors affect the skill's power. Options include:
  - `NoScale`: No additional scaling.
  - `AddIntToPower`: Add power based on the intelligence of the caster.
  - `MultIntToPower`: Multiply the power based to the intelligence of the caster.
  - `AddStrToPower`: Add power based to the strenght of the caster.

### Continuous Healing Buff

- **Continuous Healing Chance:** The probability of the skill applying continuous healing buff.
- **Continuous Healing Potency:** The strength or effectiveness of the continuous healing.
- **Continuous Healing Scale Type:** Similar to the skill power, this determines how it scales with the buff.
- **Continuous Healing Duration:** The duration for which the continuous healing effect lasts.

### Burned Condition

- **Burn Chance:** The likelihood of the skill inflicting a "burned" condition on the target.
- **Burn Duration:** The duration of the "burned" condition.
- **Extra Percentage at Burned Targets:** Percentage damage or effect potency applied to targets with the "burned" condition.

These properties allow you to create a wide variety of skills with different effects, such as offensive attacks, healing spells, and status-inflicting abilities. Customization and balancing of skills can be done by adjusting these attributes within the Scriptable Object.

## Character Turn System

The character turn system is a crucial part of the project, and it works as follows:

### Controlling Character Turns

- Characters take turns in the battle, and this system is managed by the `BattleSystem.cs`.
- The `InitBattle()` function initializes the battle, and the `SetCharacterTurn()` function controls the order of character turns.
- The `SetCharacterTurn()` function also handles enemy AI and calls `EnableEnemyAI()` to initiate their actions.
- Additionally, this function triggers `CheckConditionsAndDisplayStatus()` to evaluate conditions and buffs on all characters.

### Choosing Skills and Targets

- At the beginning of each turn, characters must choose their actions.
- The `DisplaySkills()` function displays available skills, allowing characters to select one.
- The `SelectSkill()` function is responsible for skill selection.
- After choosing a skill, the `DisplayTargetsSelection()` function presents targets, and the `AttackCharacterList()` function processes the selected targets.
- Finally, `SkillManager.UseSkillAtTarget()` is called for each target, and the attack status is displayed before moving to the next character's turn.

## Enemy AI Control

Enemy AI in this project is managed by the `EnemyController`. Here's how it operates:

- The `EnableEnemyAI()` function starts controlling the enemy character whose turn it currently is.
- The AI selects a random skill using `ChooseMove()` (or basic attack if no mana is available).
- It then picks a random target using `ChooseTarget()`.

## Handling Character Death and Game Over

Character survival and game over scenarios are managed as follows:

- Whenever a character takes damage, `CheckCharacterDeath()` is called to check if the character is still alive.
- If a character dies, they are removed from the character list.
- If there are no characters left in either the allies or enemies list, the game calls `Gameover()`.
- `Gameover()` disables any running AI and displays the game over screen.

