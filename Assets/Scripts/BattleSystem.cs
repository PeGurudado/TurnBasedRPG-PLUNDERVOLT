
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    [SerializeField] private EnemyController enemyController;
    [SerializeField] private SkillsManager skillsManager;

    [SerializeField] private List<Character> allies = new List<Character>();
    [SerializeField] private List<Character> enemies = new List<Character>();
    
    [Tooltip("Indicate the character turn from the characters list, " +
        "for default will start wich entity 0 from list")]
    [SerializeField] private int currentCharacterTurn = -1;
    private bool isPlayersTurn = true; 

    [SerializeField] private TextMeshProUGUI characterStatus;
    [SerializeField] private CharacterTargetButtonSlot targetCharButton;


    [SerializeField] private RectTransform battleFrame;
    [Header("Attack Frame")]
    [SerializeField] private RectTransform attackSelectionFrame;
    [SerializeField] private RectTransform attackFrame;
    [SerializeField] private TextMeshProUGUI attackNameText;
    [SerializeField] private TextMeshProUGUI attackDescriptionText;


    [SerializeField] private RectTransform charactersStatusFrame;
    [Header("Battle Status Frame")]
    [SerializeField] private RectTransform battleStatusFrame;
    [SerializeField] private TextMeshProUGUI battleStatusText;


    [Header("Skills List")]
    [SerializeField] private RectTransform skillsList;
    [SerializeField] private RectTransform skillsListFrame;
    [SerializeField] private SkillButtonSlot skillSelectionButton;

    [Header("Gameover")]
    [SerializeField] private RectTransform gameoverFrame;
    [SerializeField] private TextMeshProUGUI gameoverText;

    private Skill selectedSkill;
    private Character characterTurn;
    public static BattleSystem instance;

    private float attackWindowDuration = 2;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);
    }

    void Start()
    {
        InitBattle(allies, enemies);
    }

    void InitBattle(List<Character> alliesCharacters, List<Character> enemiesCharacters)
    {
        SetCharactersSecondaryStats(alliesCharacters);
        SetCharactersSecondaryStats(enemiesCharacters);

        SetCharacterTurn();
    }

    void SetCharacterTurn()
    {
        if (isPlayersTurn)
        {
            if(currentCharacterTurn < allies.Count-1)
            {
                currentCharacterTurn++;
                characterTurn = allies[currentCharacterTurn]; //Set turn to first character from Allies 
            }
            else// It goes back to enemies if has pass through all allies
            {
                isPlayersTurn = false;
                currentCharacterTurn = -1;
                SetCharacterTurn();
                return;
            }
        }
        else
        {
            if(currentCharacterTurn < enemies.Count-1)
            {
                currentCharacterTurn++;
                characterTurn = enemies[currentCharacterTurn];

                //Enemies turn, activate enemy AI 
                enemyController.EnableEnemyAI(characterTurn);
            }
            else// It goes back to player if has pass through all enemies 
            {
                isPlayersTurn = true;
                currentCharacterTurn = -1;

                //Goes back to player turn, disable enemy AI
                enemyController.DisableAI();

                SetCharacterTurn();
                return;
            }            
        }

        StartCoroutine(CheckConditionsAndDisplayStatus());
    }

    public void SetCharactersSecondaryStats(List<Character> characters)
    {
        foreach (var character in characters)
        {
            character.Stats.CalculateSecondaryStats();
        }
    }

    public void DisplaySkills()
    {
        battleFrame.gameObject.SetActive(false);
        skillsListFrame.gameObject.SetActive(true);


        int childCount = skillsList.childCount;

        for (int i = childCount - 1; i >= 0; i--)
        {
            Transform child = skillsList.GetChild(i);
            Destroy(child.gameObject);
        }

        for (int i = 0; i < characterTurn.SkillsList.Count; i++)
        {
            var skillButton = Instantiate(skillSelectionButton, skillsList);
            skillButton.SkillButton = characterTurn.SkillsList[i];
        }
    }

    private void DisplayCharactersStatus()
    {
        List<Character> charactersList;

        if (isPlayersTurn)
            charactersList = allies;
        else
            charactersList = enemies;

        charactersStatusFrame.gameObject.SetActive(true);

        Transform charStatusContent = charactersStatusFrame.GetChild(0).transform;

        int childCount = charStatusContent.childCount;

        for (int i = childCount - 1; i >= 0; i--)
        {
            Transform child = charStatusContent.GetChild(i);
            Destroy(child.gameObject);
        }

        for (int i = 0; i < charactersList.Count; i++)
        {
            var newText = Instantiate(characterStatus, charStatusContent);
            newText.text = 
                charactersList[i].Name +
                " HP "+ charactersList[i].Stats.HP
                +"/"+ charactersList[i].Stats.HPMax
                +" MP "+ charactersList[i].Stats.MP 
                + "/"+charactersList[i].Stats.MPMax;

            if (currentCharacterTurn == i)// Highlights current turn character
                newText.text = "<color=yellow>> </color>" + newText.text;
        }      
    }

    // This function should be called only when there is a selected skill
    public List<Character> GetTargetsForSelectedSkill()
    {
        List<Character> newTargetsList = new List<Character>();

        List<Character> targetsList = selectedSkill.TargetEnemies
            ? (characterTurn.IsEnemy ? allies : enemies)
            : (characterTurn.IsEnemy ? enemies : allies);

        foreach (var target in targetsList) // Clones the target list so that it can be altered without changing at main list
        {
            newTargetsList.Add(target); 
        }        

        return newTargetsList;
    }

    private void DisplayTargetsSelection()
    {
        List<Character> allTargets;

        allTargets = GetTargetsForSelectedSkill();

        attackSelectionFrame.gameObject.SetActive(true);
        Transform charStatusContent = attackSelectionFrame.GetChild(0).transform;

        int childCount = charStatusContent.childCount;

        for (int i = childCount - 1; i >= 0; i--)
        {
            Transform child = charStatusContent.GetChild(i);
            Destroy(child.gameObject);
        }

        if (selectedSkill.AreaAttack) //If it's area attack, create a single button with all characters infos
        {
            var targetButton = Instantiate(targetCharButton, charStatusContent);
            targetButton.ButtonCharacter = allTargets;
            targetButton.ButtonText.text = "";

            for (int i = 0; i < allTargets.Count; i++) 
            {
                targetButton.ButtonText.text +=
                    allTargets[i].Name +
                    " HP " + allTargets[i].Stats.HP
                    + "/" + allTargets[i].Stats.HPMax
                    + " MP " + allTargets[i].Stats.MP
                    + "/" + allTargets[i].Stats.MPMax;
                targetButton.ButtonText.text += "\n";
            }
        }
        else
        {
            for (int i = 0; i < allTargets.Count; i++)
            {
                var targetButton = Instantiate(targetCharButton, charStatusContent);
                targetButton.ButtonCharacter = new List<Character>() { allTargets[i] }; 
                targetButton.ButtonText.text =
                    allTargets[i].Name +
                    " HP " + allTargets[i].Stats.HP
                    + "/" + allTargets[i].Stats.HPMax
                    + " MP " + allTargets[i].Stats.MP
                    + "/" + allTargets[i].Stats.MPMax;
            }
        }

        battleFrame.gameObject.SetActive(false);
        DisplayAttackFrame();
    }

    void DisplayAttackFrame()
    {
        attackFrame.gameObject.SetActive(true);
        attackNameText.text = selectedSkill.Name;
        attackDescriptionText.text = selectedSkill.Description;
    }

    public IEnumerator SelectSkill(Skill skill)
    {        
        charactersStatusFrame.gameObject.SetActive(false);        

        if (characterTurn.Stats.MP < skill.MpCost)
        {
            battleStatusFrame.gameObject.SetActive(true); //Shows battle status frame

            battleStatusText.text = characterTurn.Name + " doesn't have enough MP to use "+ skill.Name;
            yield return new WaitForSeconds(attackWindowDuration);
            battleStatusFrame.gameObject.SetActive(false); //Close frame
            DisplayCharactersStatus();
        }
        else //Selects skill only if has enough MP 
        {
            selectedSkill = skill;

            battleStatusFrame.gameObject.SetActive(false);
            skillsListFrame.gameObject.SetActive(false);

            DisplayTargetsSelection();
        }
    }

    public IEnumerator AttackCharacterList(List<Character> targetCharacters)
    {
        attackFrame.gameObject.SetActive(false);
        attackSelectionFrame.gameObject.transform.localScale = Vector3.zero;

        battleStatusFrame.gameObject.SetActive(true); //Shows battle status frame

        characterTurn.Stats.ConsumeMP(selectedSkill.MpCost); // Spend Skill MP cost
        for (int i = 0; i < targetCharacters.Count; i++)
        {
            int prevHealth = targetCharacters[i].Stats.HP, prevBurnTurn = targetCharacters[i].Stats.BurnTurns, prevContHealTurns = targetCharacters[i].Stats.ContinuousHealingTurns;
            bool hasHit = skillsManager.UseSkillAtTarget(characterTurn, targetCharacters[i], selectedSkill);
            int healthChange = prevHealth - targetCharacters[i].Stats.HP;

            if (!hasHit)
            {
                battleStatusText.text = characterTurn.Name + " has used " + selectedSkill.Name +
                    " at " + targetCharacters[i].Name + " but he missed...";
            }
            else if (healthChange > 0)
            {
                battleStatusText.text = characterTurn.Name + " has used " + selectedSkill.Name +
                    " at " + targetCharacters[i].Name + " and did " + healthChange + " damage";
            }
            else if (healthChange < 0)
            {
                battleStatusText.text = characterTurn.Name + " has used " + selectedSkill.Name +
                    " at " + targetCharacters[i].Name + " and healed " + (-healthChange);
            }
            else
            {
                battleStatusText.text = characterTurn.Name + " has used " + selectedSkill.Name +
                    " at " + targetCharacters[i].Name;
            }
            yield return new WaitForSeconds(attackWindowDuration);

            if (targetCharacters[i].Stats.BurnTurns > prevBurnTurn)
            {
                battleStatusText.text = targetCharacters[i].Name + " is now burning.";
                yield return new WaitForSeconds(attackWindowDuration);
            }

            if (targetCharacters[i].Stats.ContinuousHealingTurns > prevContHealTurns)
            {
                battleStatusText.text = targetCharacters[i].Name + " is now with continuous healing buff.";
                yield return new WaitForSeconds(attackWindowDuration);
            }

            if (CheckCharacterDeath(targetCharacters[i]))
            {
                yield return new WaitForSeconds(attackWindowDuration);

            }
        }      

        DisableBattleStatus();
        attackSelectionFrame.gameObject.transform.localScale = Vector3.one;
        attackSelectionFrame.gameObject.SetActive(false);
    }

    private void DisableBattleStatus()
    {
        battleStatusFrame.gameObject.SetActive(false);
        SetCharacterTurn();        
    }

    public void DeselectBackButton()
    {
        attackFrame.gameObject.SetActive(false);
        attackSelectionFrame.gameObject.SetActive(false);
        skillsListFrame.gameObject.SetActive(false);

        battleFrame.gameObject.SetActive(true);
        DisplayCharactersStatus();
    }

    IEnumerator CheckConditionsAndDisplayStatus()
    {
        battleFrame.gameObject.SetActive(false);
        charactersStatusFrame.gameObject.SetActive(false);
        battleStatusFrame.gameObject.SetActive(true);

        int burningValue = characterTurn.Stats.CheckBurningCondition();

        if (burningValue > 0) //If is burning, apply burn condition and returns true
        {
            battleStatusText.text = characterTurn.Name + " is burning and got "+ characterTurn.Stats.BurningValue + " damage.";

            yield return new WaitForSeconds(attackWindowDuration);
        }

        int healingValue = characterTurn.Stats.CheckContinuousHealing();

        if (healingValue > 0)
        {
            battleStatusText.text = characterTurn.Name + " was healed " + healingValue + " from Continuous Healing";

            yield return new WaitForSeconds(attackWindowDuration);
        }


        if(CheckCharacterDeath(characterTurn))
        {
            yield return new WaitForSeconds(attackWindowDuration);
            currentCharacterTurn--;
            SetCharacterTurn();
        }

        //After displaying conditions effects, close window and shows Characters status
        battleStatusFrame.gameObject.SetActive(false);
        battleFrame.gameObject.SetActive(true);
        DisplayCharactersStatus();
    }

    private bool CheckCharacterDeath(Character character)
    {
        if (!character.Stats.IsAlive())
        {
            if (allies.Contains(character))
            {
                allies.Remove(character);
            }
            else if (enemies.Contains(character))
                enemies.Remove(character);

            Destroy(character.gameObject);
            battleStatusText.text = character.Name + " has died.";

            if(allies.Count == 0 || enemies.Count == 0)
            {
                Invoke("Gameover",attackWindowDuration);
            }

            return true;
        }
        return false;
    }

    private void Gameover()
    {        
        enemyController.DisableAI();
        gameoverFrame.gameObject.SetActive(true);

        if(allies.Count > 0)
            gameoverText.text = "It's gameover, player wins";
        else
            gameoverText.text = "It's gameover, enemies wins";
    }

    public void ResetBattleButton() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //Reloads the current scene
    }
}
