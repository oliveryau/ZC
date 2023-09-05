using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxAction : _PlayerAction
{
    private void Update() //place in specific player child classes
    {
        if (gameManager.state == BattleState.PLAYERTURN && gameManager.activePlayer == gameObject.name)
        {
            DisplayUi();

            SelectTarget();

            PlayerMovement();

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
                movingToTarget = true;
                selectedSkillPrefab.GetComponent<_NormalAttack>().Attack(target);
            }
            else if (selectedSkillPrefab == skill2Prefab)
            {
                //stun
            }
            else if (selectedSkillPrefab == skill3Prefab)
            {
                //taunt
            }

            StartCoroutine(AnimationDelay(2f));
        }
    }
}
