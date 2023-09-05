using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JadeSupportAction : _EnemyAction
{
    private void Update()
    {
        if (gameManager.state == BattleState.ENEMYTURN && gameManager.activeEnemy == gameObject.name)
        {
            if (!enemyAttacking)
            {
                EnemySelectSkill();
            }

            if (enemyTurnComplete)
            {
                //change to nextturn state after completing attack
                Debug.Log("Jade Support attacked");
                gameManager.state = BattleState.NEXTTURN;

                selectedSkillPrefab = null;
                enemyAttacking = false;
                enemyTurnComplete = false;
            }
        }
    }

    public override void EnemyUseSkill(GameObject target)
    {
        if (selectedSkillPrefab != null)
        {
            enemyAttacking = true;

            if (selectedSkillPrefab == skill1Prefab) //single target
            {
                selectedSkillPrefab.GetComponent<_NormalAttack>().Attack(target);
            }
            else if (selectedSkillPrefab == skill2Prefab)
            {
                Debug.Log("AOE Attack Player Team");
            }

            StartCoroutine(EnemyAnimationDelay(1f));
        }
    }
}
