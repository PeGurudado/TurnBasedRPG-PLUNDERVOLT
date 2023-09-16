using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsManager : MonoBehaviour
{

    public bool UseSkillAtTarget(Character caster, Character target, Skill usedSkill)
    {        
        float totalPower = usedSkill.BasePower;

        totalPower = ScaleFromType(totalPower, caster.Stats, usedSkill.ScaleType);
        totalPower = ScaleFromExtraPercentAtBurnTargets(target.Stats, totalPower, usedSkill.ExtraPercentageAtBurnTargets); 

        float hitChance = CalculateHitChance(caster, target, usedSkill);

        bool hasHit;
        if(totalPower > 0) //Damage Skill
        {
            hasHit = target.Stats.TakeDamage((int) totalPower, hitChance);
        }
        else //Heal Skill
        {
            hasHit = true;
            target.Stats.Heal((int)totalPower * -1);
        }

        if (hasHit)
        {
            CheckContinuousHealingBuff(caster, target, usedSkill);
            CheckBurnedCondition(target, usedSkill);
        }

        return hasHit;
    }

    private float ScaleFromType(float baseValue, Stats baseStats, SkillScaleType scaleType)
    {
        if (scaleType == SkillScaleType.AddStrToPower)
        {
            baseValue += baseStats.Strenght;
        }
        else if (scaleType == SkillScaleType.AddIntToPower)
        {
            baseValue += baseStats.Intelligence;
        }
        else if (scaleType == SkillScaleType.MultIntToPower)
        {
            baseValue *= baseStats.Intelligence;
        }
        return baseValue;
    }

    private float ScaleFromExtraPercentAtBurnTargets(Stats targetStats, float baseValue, float extraPercent)
    {
        extraPercent = extraPercent / 100;

        if (targetStats.BurnTurns > 0) //Calculates extra damage from burned targets 
            return baseValue * (1 + extraPercent);
        else
            return baseValue;
    }

    private float CalculateHitChance(Character caster, Character target, Skill usedSkill)
    {
        if (usedSkill.TargetEnemies)
            return usedSkill.HitChance + caster.Stats.HitRatio - target.Stats.DodgeChance;
        else // Hit chance is always 100 if target allies 
            return 100;
    }

    private void CheckContinuousHealingBuff(Character caster, Character target, Skill usedSkill)
    {
        if (usedSkill.ContHealChance > Random.Range(0, 100))
        {
            float totalHeal = usedSkill.ContHealPotency;

            totalHeal = ScaleFromType(totalHeal, caster.Stats, usedSkill.ContHealScaleType);
            target.Stats.ApplyContinuousHealing(usedSkill.ContHealDuration, (int)totalHeal);
        }   
    }

    private void CheckBurnedCondition(Character target, Skill usedSkill)
    {
        if (usedSkill.BurnChance > Random.Range(0, 100))
        {
            target.Stats.ApplyBurning(usedSkill.BurnDuration);
        }
    }
}
