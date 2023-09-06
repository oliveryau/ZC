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
                selectedTarget = null;
                playerAttacking = false;
                playerTurnComplete = false;
            }
        }
    }

    public override void UseSkill()
    {
        if (selectedSkillPrefab != null)
        {
            playerAttacking = true;

            if (selectedSkillPrefab == skill1Prefab || selectedSkillPrefab == skill2Prefab)
            {
                //single target and stun
                movingToTarget = true;
            }
            else if (selectedSkillPrefab == skill3Prefab)
            {
                //taunt
            }
        }
    }

    public override void ApplySkill()
    {
        if (selectedSkillPrefab == skill1Prefab)
        {
            //single target skill
            selectedSkillPrefab.GetComponent<_NormalAttack>().Attack(selectedTarget);
        }
        else if (selectedSkillPrefab == skill2Prefab)
        {
            //stun target skill
            selectedSkillPrefab.GetComponent<AoeAttack>().Attack(enemyTargets); //temporary
        }
        else if (selectedSkillPrefab == skill3Prefab)
        {
            //taunt skill
            selectedSkillPrefab.GetComponent<_NormalAttack>().Attack(selectedTarget); //temporary
        }

        StartCoroutine(EndTurnDelay(1f));
    }
}
