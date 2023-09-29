using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardAction : _EnemyAction
{
    private void Update()
    {
        UpdateEnemyState();
    }

    #region Guard State
    protected override void UpdateEnemyState()
    {
        if (battleManager.battleState == BattleState.ENEMYTURN && battleManager.activeEnemy == gameObject.name)
        {
            if (enemyState == EnemyState.WAITING)
            {
                enemyStartTurn = true;
                characterStats.healthPanel.GetComponent<Animator>().SetTrigger("moveOut");
                EnemyToggleUi(true);

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
                #region Stunned
                else if (characterStats.checkedStatus && characterStats.stunCounter > 0)
                {
                    enemyState = EnemyState.ENDING;
                    checkingStatus = false;
                }
                #endregion
                else if (characterStats.checkedStatus)
                {
                    enemyState = EnemyState.SKILLSELECT;
                    checkingStatus = false;
                }
            }

            else if (enemyState == EnemyState.SKILLSELECT)
            {
                EnemyRefreshTargets();

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
                    EnemyToggleSkillText(true);

                    EnemyUseSkill();
                }

                EnemyMovement();
            }

            else if (enemyState == EnemyState.ENDING)
            {
                EnemyToggleUi(false);
                EnemyToggleSkillText(false);

                characterStats.CheckEndStatusEffects();

                battleManager.battleState = BattleState.NEXTTURN;

                //hud
                enemyStartTurn = false;
                characterStats.healthPanel.GetComponent<Animator>().SetTrigger("moveIn");
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
                targetPosition = null;

                enemyAttacking = false;
                enemyEndingTurn = false;
                characterStats.checkedStatus = false;

                enemyState = EnemyState.WAITING;
            }
        }
    }
    #endregion

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

        #region Movement Skills
        if (selectedSkillPrefab == skill1Prefab)
        {
            targetPosition = selectedTarget.GetComponentInChildren<TargetPosition>().transform;

            movingToTarget = true;
        }
        #endregion
    }

    protected override void EnemyAttackAnimation()
    {
        #region Movement Skills
        if (selectedSkillPrefab == skill1Prefab)
        {
            StartCoroutine(EnemyAttackStartDelay(0.5f, 1f));
        }
        #endregion
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
