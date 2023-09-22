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

                #region Taunted Behaviour
                if (characterStats.tauntCounter > 0)
                {
                    //if taunt target is dead
                    if (!selectedTarget.gameObject.activeSelf)
                    {
                        selectedTarget = null;

                        characterStats.tauntCounter = 0;
                    }
                }
                #endregion

                enemyState = EnemyState.CHECKSTATUS;
            }

            else if (enemyState == EnemyState.CHECKSTATUS)
            {
                if (!characterStats.checkedStatus && !checkingStatus)
                {
                    StartCoroutine(characterStats.CheckStatusEffects());
                    checkingStatus = true;
                }
                else if (characterStats.checkedStatus && characterStats.stunCounter > 0)
                {
                    //stunCheck
                    enemyState = EnemyState.ENDING;
                    checkingStatus = false;
                }
                else if (characterStats.checkedStatus)
                {
                    enemyState = EnemyState.SKILLSELECT;
                    checkingStatus = false;
                }
            }

            else if (enemyState == EnemyState.SKILLSELECT)
            {
                EnemyRefreshTargets();

                EnemyToggleUi();

                EnemySelectSkill();
            }

            else if (enemyState == EnemyState.TARGETING)
            {
                if (characterStats.tauntCounter <= 0)
                {
                    EnemySelectTarget();
                }
                #region Taunted Behaviour
                else if (characterStats.tauntCounter > 0)
                {
                    //taunt
                    enemyState = EnemyState.ATTACKING;
                }
                #endregion
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

                //hud
                turnIndicator.SetActive(false);

                //skill
                selectedSkillPrefab = null;
                if (characterStats.tauntCounter <= 0)
                {
                    selectedTarget = null;
                }

                //target
                playerTargets = null;

                //others
                enemyAttacking = false;
                enemyEndingTurn = false;
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

        StartCoroutine(EnemyEndTurnDelay(0.5f));
    }
}
