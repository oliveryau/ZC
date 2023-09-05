using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtons : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject character;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        if ( gameManager.state == BattleState.PLAYERTURN )
        {
            string chosenSkill = gameObject.name;
            gameObject.GetComponent<Button>().onClick.AddListener(() => AttachCallback(chosenSkill));
            character = GameObject.FindGameObjectWithTag("Player");
        }
    }

    private void AttachCallback(string btn)
    {
        switch (character.name)
        {
            case "Cat":
                if (btn.CompareTo("Skill1Btn") == 0)
                {
                    character.GetComponent<_CharacterAction>().SelectSkill("skill1");
                }
                else if (btn.CompareTo("Skill2Btn") == 0)
                {
                    character.GetComponent<_CharacterAction>().SelectSkill("skill2");
                }
                else if (btn.CompareTo("Skill3Btn") == 0)
                {
                    character.GetComponent<_CharacterAction>().SelectSkill("skill3");
                }
                break;
            default:
                Debug.Log("No player");
                break;
        }
    }
}
