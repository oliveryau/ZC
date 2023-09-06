using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JadeWarriorAction : _EnemyAction
{
    private void Update()
    {
        if (gameManager.state == BattleState.ENEMYTURN && gameManager.activeEnemy == gameObject.name)
        {
            if (!enemyAttacking)
            {
                EnemySelectSkill();
            }

            EnemyMovement();

            if (enemyTurnComplete)
            {
                Debug.Log("Jade Warrior End Turn");
                gameManager.state = BattleState.NEXTTURN;

                selectedSkillPrefab = null;
                selectedTarget = null;
                enemyAttacking = false;
                enemyTurnComplete = false;
            }
        }
    }

    public override void EnemySelectSkill()
    {
        if (selectedSkillPrefab == null)
        {
            selectedSkillPrefab = Random.Range(0, 2) == 0 ? skill1Prefab : skill2Prefab;
            Debug.Log("Enemy Skill: " + selectedSkillPrefab.name);

            EnemySelectTarget();
        }
    }

    public override void EnemyUseSkill()
    {
        if (selectedSkillPrefab != null)
        {
            enemyAttacking = true;

            movingToTarget = true;
        }
    }

    public override void EnemyApplySkill()
    {
        if (selectedSkillPrefab == skill1Prefab)
        {
            //single target attack
            selectedSkillPrefab.GetComponent<_NormalAttack>().Attack(selectedTarget);
        }
        else if (selectedSkillPrefab == skill2Prefab)
        {
            //aoe target skill
            selectedSkillPrefab.GetComponent<AoeAttack>().Attack(playerTargets);
        }

        StartCoroutine(EnemyEndTurnDelay(1f));
    }
}
