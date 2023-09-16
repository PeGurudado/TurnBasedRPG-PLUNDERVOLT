using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] RectTransform enemiesTurnFrame;
    Character currentEnemyTurn;
    Skill currentSelectedSkill;

    float enemyMoveDelay = 1f;

    public void EnableEnemyAI(Character enemyTurn)
    {
        currentEnemyTurn = enemyTurn;
        enemiesTurnFrame.gameObject.SetActive(true);

        Invoke("ChooseMove", enemyMoveDelay);
    }

    public void DisableAI()
    {
        CancelInvoke();
        enemiesTurnFrame.gameObject.SetActive(false);
    }

    private void ChooseMove()
    {
        List<Skill> possibleSkillsToUse = new List<Skill>();

        foreach (var skill in currentEnemyTurn.SkillsList)
        {
            if (currentEnemyTurn.Stats.MP > skill.MpCost)
                possibleSkillsToUse.Add(skill);
        }        

        if(possibleSkillsToUse.Count == 0)
        {
            //Chooses Basic Attack
            currentSelectedSkill = currentEnemyTurn.BasicAttack;
        }
        else
        {
            //Choose random skill from possible skills list
            currentSelectedSkill = possibleSkillsToUse[Random.Range(0, possibleSkillsToUse.Count)];
        }
        StartCoroutine(BattleSystem.instance.SelectSkill(currentSelectedSkill));
        Invoke("ChooseTarget", enemyMoveDelay);
    }

    private void ChooseTarget()
    {
        var targets = BattleSystem.instance.GetTargetsForSelectedSkill();
        if (currentSelectedSkill.AreaAttack) //If areaAttack attack all targets
            StartCoroutine(BattleSystem.instance.AttackCharacterList(targets));
       else { //Chooses one random target to attack
            int randIndex = Random.RandomRange(0, targets.Count);

            StartCoroutine(BattleSystem.instance.AttackCharacterList(new List<Character> { targets[randIndex] }));
        }

    }

}
