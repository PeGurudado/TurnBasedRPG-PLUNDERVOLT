using System;
using UnityEngine;

[Serializable]
public class Stats{

    //Main Stats

    [SerializeField] private int strenght, vitality, dexterity, agility, intelligence;

    private int burnTurns, burningValue = 2, continuousHealingTurns, continousHealingValue;

    //Secondary Stats
    private int hp = 5, mp = 10, hpMax, mpMax;
    private float dodge = 5, hitRatio = 20;

    public int HP { get => hp; set => hp = value; }
    public int HPMax { get => hpMax; }

    public int MP { get => mp; set => mp = value; }
    public int MPMax { get => mpMax; }

    public float HitRatio { get => hitRatio; }
    public float DodgeChance { get => dodge; }

    public int Strenght { get => strenght; }
    public int Intelligence { get => intelligence; }

    public int BurnTurns { get => burnTurns; set => burnTurns = value; }

    public int BurningValue { get => burningValue; }
    public int ContinuousHealingTurns { get => continuousHealingTurns; set => continuousHealingTurns = value; }
    public int ContinousHealingValue { get => continousHealingValue; set => continousHealingValue = value; }

    public Stats(int strenght, int vitality, int dexterity, int agility, int intelligence ){

        this.strenght = strenght;
        this.vitality = vitality;
        this.dexterity = dexterity;
        this.agility = agility;
        this.intelligence = intelligence;

        CalculateSecondaryStats();
    }

    public bool TakeDamage(int value, float hitChance)
    {
        if (hitChance > UnityEngine.Random.Range(0, 100)) //Check if got hit
        {
            hp -= value;
            return true;
        }
        return false;
    }

    public void ConsumeMP(int value)
    {
        mp -= value;

        if (mp < 0)
            mp = 0;
    }
    public bool IsAlive()
    {
        if (hp > 0)
            return true;
        else
            return false;
    }

    public int Heal(int value)
    {
        int prevHP = hp;

        hp += value;
        if (hp > hpMax)
            hp = hpMax;

        
        return hp - prevHP; //total healed
    }

    public void ApplyBurning(int duration)
    {
        burnTurns = duration;
    }

    public int CheckBurningCondition()
    {
        if(burnTurns > 0)
        {
            hp -= burningValue;
            burnTurns--;
            return burningValue;
        }
        return 0;
    }

    public void ApplyContinuousHealing(int duration, int healStrenght)
    {
        continousHealingValue = healStrenght;
        continuousHealingTurns = duration;
    }

    public int CheckContinuousHealing()
    {
        if(continuousHealingTurns > 0)
        {
            continuousHealingTurns--;            
            return Heal(continousHealingValue); ;
        }
        return 0;
    }

    public void CalculateSecondaryStats(){
        hp += (vitality * 2);
        mp += (intelligence * 3);
        hpMax = hp;
        mpMax = mp;
        dodge += (agility * 3);
        hitRatio += (dexterity * 4);
    }
}