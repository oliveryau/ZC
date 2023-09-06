using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JadeTankAction : _EnemyAction
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
                Debug.Log("Jade Tank attacked");
                gameManager.state = BattleState.NEXTTURN;

                selectedSkillPrefab = null;
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
            Debug.Log("Enemy chose skill: " + selectedSkillPrefab.name);

            EnemySelectTarget();
        }
    }

    public override void EnemyUseSkill()
    {
        if (selectedSkillPrefab != null)
        {
            enemyAttacking = true;

            movingToTarget = true;

            StartCoroutine(EnemyAnimationDelay(1f));
        }
    }

    public override void EnemyApplySkill()
    {
        if (selectedSkillPrefab == skill1Prefab)
        {
            //single target skill
            if (Vector3.Distance(transform.position, targetPosition.position) <= 0.1f)
            {
                selectedSkillPrefab.GetComponent<_NormalAttack>().Attack(selectedTarget);
            }
        }
        else if (selectedSkillPrefab == skill2Prefab)
        {
            //stun target skill
            if (Vector3.Distance(transform.position, targetPosition.position) <= 0.1f)
            {
                selectedSkillPrefab.GetComponent<AoeAttack>().Attack(playerTargets); //temporary
            }
        }

        enemyAttacking = false;
    }
}
