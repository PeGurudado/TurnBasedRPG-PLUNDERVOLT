using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillButtonSlot : MonoBehaviour
{
    [SerializeField] private Skill skillButton;
    private TextMeshProUGUI buttonText;
    private Button button;
    [SerializeField] private bool setSkillNameAtStart = true;

    public Skill SkillButton { get => skillButton; set => skillButton = value; }


    private void Awake()
    {
        button = GetComponent<Button>();
        buttonText = GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(SelectSkillButton);

        if (setSkillNameAtStart)
        {
            string buttonMessage = skillButton.Name;
            if (skillButton.MpCost > 0)
                buttonMessage += "<color=blue> " + skillButton.MpCost + "</color>";
            buttonText.text = buttonMessage;
        }
    }

    private void SelectSkillButton()
    {
        StartCoroutine(BattleSystem.instance.SelectSkill(skillButton));
    }

}
