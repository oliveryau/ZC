using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardAction : _EnemyAction
{
    private void Update()
    {
        if (gameManager.state == BattleState.ENEMYTURN && gameManager.activeEnemy == gameObject.name)
        {
            if (!characterStats.checkedStatus)
            {
                characterStats.StartCoroutine(characterStats.CheckStatusEffects());
            }

            if (!reinitialisePlayerTargets)
            {
                RefreshPlayerTargets();
            }

            if (!enemyAttacking)
            {
                EnemySelectSkill();
            }

            EnemyMovement();

            if (enemyTurnComplete)
            {
                gameManager.state = BattleState.NEXTTURN;

                selectedSkillPrefab = null;
                selectedTarget = null;
                reinitialisePlayerTargets = false;
                playerTargets = null;

                characterStats.checkedStatus = false;
                enemyAttacking = false;
                enemyTurnComplete = false;
            }
        }
    }

    protected override void EnemySelectSkill()
    {
        if (selectedSkillPrefab == null)
        {
            selectedSkillPrefab = skill1Prefab;

            EnemySelectTarget();
        }
    }

    protected override void EnemySelectTarget()
    {
        if (selectedSkillPrefab == skill1Prefab)
        {
            if (playerTargets.Length > 0)
            {
                int randomIndex = Random.Range(0, playerTargets.Length);
                selectedTarget = playerTargets[randomIndex];
                Debug.Log("Enemy Selected Target: " + selectedTarget.name);

                EnemyUseSkill();
            }
        }
    }

    protected override void EnemyUseSkill()
    {
        enemyAttacking = true;

        if (selectedSkillPrefab == skill1Prefab) //skills that require movement
        {
            movingToTarget = true; //movement is triggered
        }
        //else //skills that do not require movement
        //{
        //    EnemyAttackAnimation();
        //}
    }

    protected override void EnemyAttackAnimation()
    {
        if (selectedSkillPrefab == skill1Prefab)
        {
            StartCoroutine(EnemyAttackStartDelay(0.5f, 1f));
        }
    }

    protected override void EnemyApplySkill()
    {
        if (selectedSkillPrefab == skill1Prefab)
        {
            //single target attack
            selectedSkillPrefab.GetComponent<NormalAttack>().Attack(selectedTarget);
        }

        StartCoroutine(EnemyEndTurnDelay(1f));
    }
}
