using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Skill", menuName = "Game/Skill")]
public class Skill : ScriptableObject{

    [Header("General")]
    [SerializeField] private string skillName;
    [SerializeField] private string description;
    [SerializeField] private int mpCost;
    [SerializeField] private float hitChance;
    [SerializeField] bool targetEnemies;
    [SerializeField] bool areaAttack;

    [SerializeField] int basePower = 0;
    [SerializeField] SkillScaleType scaleType;
    [Header("Continuous Healing Buff")]
    [SerializeField] float contHealChance = 0;
    [SerializeField] float contHealPotency = 0;
    [SerializeField] SkillScaleType contHealScaleType;
    [SerializeField] int contHealDuration = 0;
    [Header("Burned Condition")]
    [SerializeField] float burnChance = 0;
    [SerializeField] int burnDuration = 0;
    [SerializeField] float extraPercentageAtBurnTargets = 0;

    public string Name { get => skillName; }
    public string Description { get => description; }
    public float HitChance { get => hitChance; }
    public int MpCost { get => mpCost; }
    public bool AreaAttack { get => areaAttack; }
    public bool TargetEnemies { get => targetEnemies; set => targetEnemies = value; }
    public int BasePower { get => basePower; }
    public SkillScaleType ScaleType { get => scaleType; }
    public float ContHealChance { get => contHealChance; }
    public float ContHealPotency { get => contHealPotency; }
    public SkillScaleType ContHealScaleType { get => contHealScaleType; }
    public int ContHealDuration { get => contHealDuration; }
    public float BurnChance { get => burnChance; }
    public int BurnDuration { get => burnDuration; }
    public float ExtraPercentageAtBurnTargets { get => extraPercentageAtBurnTargets; }
}

public enum SkillScaleType
{
    NoScale, AddIntToPower, MultIntToPower, AddStrToPower
}