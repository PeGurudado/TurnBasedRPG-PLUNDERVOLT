using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterTargetButtonSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private Button button;
    private List<Character> targetCharacters;


    public TextMeshProUGUI ButtonText { get => buttonText; }
    public List<Character> ButtonCharacter { get => targetCharacters; set => targetCharacters = value; }


    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(AttackSelectedButton);
    }

    private void AttackSelectedButton()
    {
        StartCoroutine(BattleSystem.instance.AttackCharacterList(targetCharacters));
    }
}
