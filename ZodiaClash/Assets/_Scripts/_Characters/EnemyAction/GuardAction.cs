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
        if (enemyState == EnemyState.START)
        {
            EnemyStartMoveIn();
        }
        else if (battleManager.battleState == BattleState.ENEMYTURN && battleManager.activeEnemy == gameObject.name)
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
                if (characterStats.checkedStatus && characterStats.stunCounter > 0)
                {
                    enemyState = EnemyState.ENDING;
                    checkingStatus = false;
                }
                #endregion
                else if (characterStats.checkedStatus)
                {
                    enemyState = EnemyState.SELECTION;
                    checkingStatus = false;
                }
            }

            else if (enemyState == EnemyState.SELECTION)
            {
                EnemyRefreshTargets();

                EnemySelection();
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
                if (characterStats.health > 0)
                {
                    EnemyToggleUi(false);
                    EnemyToggleSkillText(false);

                    characterStats.CheckEndStatusEffects();
                }

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
                enemyTargets = null;
                playerTargetsList.Clear();

                //others
                targetPosition = null;

                enemyAttacking = false;
                enemyEndingTurn = false;
                characterStats.checkedStatus = false;

                enemyState = EnemyState.WAITING;
            }
        }

        if (this.enemyState == EnemyState.DYING)
        {
            StartCoroutine(EnemyDeath());
        }
    }
    #endregion

    protected override void EnemySelection()
    {
        #region Taunted Behaviour
        if (characterStats.tauntCounter > 0)
        {
            selectedSkillPrefab = skill1Prefab;

            enemyState = EnemyState.ATTACKING;
        }
        #endregion
        else
        {
            #region Target Selection Aggression Values
            float randomValue = Random.Range(0f, 1f);
            if (playerTargetsList.Count >= 3)
            {
                #region 3 Player Characters: Ox 45%, Cat 35%, Goat 20%
                if (randomValue <= 0.2f)
                {
                    selectedTarget = goat;
                    Debug.Log("Enemy Selected Target: " + selectedTarget.name);
                }
                else if (randomValue <= 0.55f)
                {
                    selectedTarget = cat;
                    Debug.Log("Enemy Selected Target: " + selectedTarget.name);
                }
                else
                {
                    selectedTarget = ox;
                    Debug.Log("Enemy Selected Target: " + selectedTarget.name);
                }
                #endregion
            }
            else if (playerTargetsList.Count >= 2)
            {
                #region No Cat: Ox 69%, Goat 31%
                if (cat == null)
                {
                    if (randomValue <= 0.31f)
                    {
                        selectedTarget = goat;
                        Debug.Log("Enemy Selected Target: " + selectedTarget.name);
                    }
                    else
                    {
                        selectedTarget = ox;
                        Debug.Log("Enemy Selected Target: " + selectedTarget.name);
                    }
                }
                #endregion

                #region No Goat: Ox 56%, Cat 44%
                else if (goat == null)
                {
                    if (randomValue <= 0.44f)
                    {
                        selectedTarget = cat;
                        Debug.Log("Enemy Selected Target: " + selectedTarget.name);
                    }
                    else
                    {
                        selectedTarget = ox;
                        Debug.Log("Enemy Selected Target: " + selectedTarget.name);
                    }
                }
                #endregion

                #region No Ox: Cat 64%, Goat 36%
                else if (ox == null)
                {
                    if (randomValue <= 0.36f)
                    {
                        selectedTarget = goat;
                        Debug.Log("Enemy Selected Target: " + selectedTarget.name);
                    }
                    else
                    {
                        selectedTarget = cat;
                        Debug.Log("Enemy Selected Target: " + selectedTarget.name);
                    }
                }
                #endregion
            }
            else
            {
                #region Solo Target
                selectedTarget = playerTargetsList[0];
                Debug.Log("Enemy Selected Target: " + selectedTarget.name);
                #endregion
            }
            #endregion

            selectedSkillPrefab = skill1Prefab;

            enemyState = EnemyState.ATTACKING;
        }

    }

    protected override void EnemyUseSkill()
    {
        enemyAttacking = true;

        #region Movement Skills
        if (selectedSkillPrefab == skill1Prefab)
        {
            targetPosition = selectedTarget.transform.Find("Target Position");
            characterStats.animator.SetTrigger("skill1");

            movingToTarget = true;
        }
        #endregion
    }

    protected override void EnemyAttackAnimation()
    {
        #region Movement Skills
        if (selectedSkillPrefab == skill1Prefab)
        {
            StartCoroutine(EnemyAttackStartDelay(0.5f, 0.5f));
        }
        #endregion
    }

    protected override void EnemyApplySkill()
    {
        if (selectedSkillPrefab == skill1Prefab)
        {
            //single target attack
            selectedSkillPrefab.GetComponent<NormalAttack>().Attack(selectedTarget);
            AudioManager.Instance.PlayEffectsOneShot("Guard 1");
        }

        StartCoroutine(EnemyEndTurnDelay(0.5f));
    }

    protected override IEnumerator EnemyDeath()
    {
        gameObject.tag = "Dead";
        //characterStats.animator.SetTrigger("Death");

        #region Update Turn Order
        battleManager.UpdateTurnOrderUi("death", characterStats);
        battleManager.charactersList.Remove(characterStats);
        battleManager.turnOrderList.Remove(characterStats);
        battleManager.originalTurnOrderList.Remove(characterStats);
        #endregion

        yield return new WaitForSeconds(0.5f);

        characterStats.characterHpHud.SetActive(false);

        enemyState = EnemyState.ENDING;

        yield return new WaitForSeconds(0.5f);

        gameObject.SetActive(false);
    }
}
