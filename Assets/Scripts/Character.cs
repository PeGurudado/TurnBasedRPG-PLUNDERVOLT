
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Character : MonoBehaviour{

    [SerializeField] private string name;
    [SerializeField] private Skill basicAttack;
    [SerializeField] private List<Skill> skillsList;
    [SerializeField] private Stats stats;
    [SerializeField] private bool isEnemy;

    public string Name { get => name; }
    public Stats Stats { get => stats; }

    public bool IsEnemy { get => isEnemy; }

    public List<Skill> SkillsList { get => skillsList; }

    public Skill BasicAttack { get => basicAttack; }

    public Character(string name, List<Skill> skills, int strenght, int vitality, int dexterity, int agility, int intelligence ){
        this.name = name;
        this.skillsList = skills;
        stats = new Stats(strenght, vitality, dexterity, agility, intelligence);
    }

}