using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAction : _CharacterAction
{
    private void Update() //place in specific player child classes
    {
        if (gameManager.state == BattleState.PLAYERTURN && gameManager.activePlayer == gameObject.name)
        {
            DisplayUi();

            SelectTarget();

            if (playerTurnComplete)
            {
                gameManager.state = BattleState.NEXTTURN;

                selectedSkillPrefab = null;
                playerAttacking = false;
                playerTurnComplete = false;
            }
        }
        else
        {
            characterUi.SetActive(false);
        }
    }

    public override void UseSkill(GameObject target)
    {
        if (selectedSkillPrefab != null) 
        {
            playerAttacking = true;

            if (selectedSkillPrefab == skill1Prefab) //single target
            {
                selectedSkillPrefab.GetComponent<_NormalAttack>().Attack(target);
            }
            else if (selectedSkillPrefab == skill2Prefab) //aoe target
            {
                GameObject[] targets = GameObject.FindGameObjectsWithTag("Enemy");
                selectedSkillPrefab.GetComponent<AoeAttack>().Attack(targets);
            }
            else if (selectedSkillPrefab == skill3Prefab)
            {
                //Another skill
            }

            StartCoroutine(AnimationDelay(1f));
        }

    }
}
