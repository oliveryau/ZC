using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardAction : _EnemyAction
{
    private void Update()
    {
        if (gameManager.state == BattleState.ENEMYTURN && gameManager.activeEnemy == gameObject.name)
        {
            if (enemyState == EnemyState.WAITING)
            {
                turnIndicator.SetActive(true);

                enemyState = EnemyState.CHECKSTATUS;
            }

            else if (enemyState == EnemyState.CHECKSTATUS)
            {
                if (!characterStats.checkedStatus)
                {
                    StartCoroutine(characterStats.CheckStatusEffects());
                }
                else if (characterStats.checkedStatus)
                {
                    enemyState = EnemyState.SKILLSELECT;
                }
            }

            else if (enemyState == EnemyState.SKILLSELECT)
            {
                RefreshPlayerTargets();

                EnemyToggleUi();

                EnemySelectSkill();
            }

            else if (enemyState == EnemyState.TARGETING)
            {
                EnemySelectTarget();
            }

            else if (enemyState == EnemyState.ATTACKING)
            {
                if (!enemyAttacking)
                {
                    EnemyUseSkill();
                }

                EnemyMovement();
            }

            else if (enemyState == EnemyState.ENDING)
            {
                characterStats.CheckEndStatusEffects();

                gameManager.state = BattleState.NEXTTURN;

                turnIndicator.SetActive(false);

                selectedSkillPrefab = null;
                selectedTarget = null;
                playerTargets = null;

                enemyAttacking = false;
                characterStats.checkedStatus = false;

                enemyState = EnemyState.WAITING;
            }
        }
    }

    protected override void EnemySelectSkill()
    {
        if (selectedSkillPrefab == null)
        {
            selectedSkillPrefab = skill1Prefab;

            enemyState = EnemyState.TARGETING;
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

                enemyState = EnemyState.ATTACKING;
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
