using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGoatAction : _EnemyAction
{
    [Header("Heal Behaviour")]
    [SerializeField] private float healCooldown;
    [SerializeField] private bool canHeal;

    [Header("Low Health State")]
    [SerializeField] private bool rageState;
    [SerializeField] private float rageCount;
    [SerializeField] private bool rageApplied;

    [Header("Death")]
    [SerializeField] private bool dead;

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
                if (characterStats.health <= 0.1f * characterStats.maxHealth)
                {
                    rageState = true;
                    EnemyToggleSkillText(false, "goat");
                }

                if (!characterStats.checkedStatus && !checkingStatus)
                {
                    StartCoroutine(characterStats.CheckStatusEffects());

                    if (healCooldown > 0)
                    {
                        --healCooldown;
                    }

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
            if (rageState)
            {
                if (rageCount == 0 && !rageApplied)
                {
                    Debug.Log("A");
                    rageApplied = true;

                    characterStats.BuffText(0, "rage");

                    characterStats.attack += 10;
                    _StatusEffectHud statusEffect = FindObjectOfType<_StatusEffectHud>();
                    statusEffect.SpawnEffectsBar(characterStats, 0, "rageGoat");
                    
                    enemyEndingTurn = true;
                }
                else if (rageCount == 0 && rageApplied)
                {
                    Debug.Log("B");
                    enemyEndingTurn = true;

                    StartCoroutine(RageEndTurn(0.5f));
                }
                else if (rageCount > 0)
                {
                    Debug.Log("C");
                    selectedSkillPrefab = skill1Prefab;
                    selectedTarget = playerTargets[0].gameObject;

                    enemyState = EnemyState.ATTACKING;

                    rageCount = 0;
                }
            }
            else
            {
                bool healingAlly = false;

                if (healCooldown == 0) //if can heal
                {
                    Debug.Log("Healing");
                    for (int i = 0; i < enemyTargets.Length; i++)
                    {
                        CharacterStats enemy = enemyTargets[i].GetComponent<CharacterStats>();
                        if (enemy.health / enemy.maxHealth <= 0.3f) //if enemy less than 30% hp -> heal target
                        {
                            selectedTarget = enemy.gameObject;
                            Debug.Log("Enemy Selected Target (Heal): " + selectedTarget.name);

                            selectedSkillPrefab = skill3Prefab;

                            healingAlly = true;
                            healCooldown = 2;

                            enemyState = EnemyState.ATTACKING;

                            break;
                        }
                    }
                }
            
                if (!healingAlly)
                {
                    if (battleManager.roundCounter == 1 || (enemyTargets.Length > 1 && battleManager.roundCounter % 3 == 0)) //speedup character at first round and at every 3 rounds
                    {
                        int randomIndex = Random.Range(0, enemyTargets.Length);
                        while (enemyTargets[randomIndex] == this.gameObject)
                        {
                            randomIndex = Random.Range(0, enemyTargets.Length);
                        }

                        selectedTarget = enemyTargets[randomIndex];
                        Debug.Log("Enemy Selected Target (Speedup): " + selectedTarget.name);

                        selectedSkillPrefab = skill2Prefab;

                        enemyState = EnemyState.ATTACKING;
                    }
                    else //do a normal attack
                    {
                        int randomIndex = Random.Range(0, playerTargets.Length);
                        selectedTarget = playerTargets[randomIndex];
                        Debug.Log("Enemy Selected Target: " + selectedTarget.name);

                        selectedSkillPrefab = skill1Prefab;

                        enemyState = EnemyState.ATTACKING;
                    }
                }
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
        #region Non-Movement Skills
        else if (selectedSkillPrefab == skill2Prefab || selectedSkillPrefab == skill3Prefab)
        {
            EnemyAttackAnimation();
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
        #region Non-Movement Skills
        else if (selectedSkillPrefab == skill2Prefab || selectedSkillPrefab == skill3Prefab)
        {
            StartCoroutine(EnemyBuffStartDelay(0.5f, 1f));
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
        else if (selectedSkillPrefab == skill2Prefab)
        {
            //ally attack buff skill
            selectedSkillPrefab.GetComponent<B_AttackBuff>().AttackBuff(selectedTarget);
        }
        else if (selectedSkillPrefab == skill3Prefab)
        {
            //ally heal cleanse skill
            selectedSkillPrefab.GetComponent<C_SingleHeal>().Heal(selectedTarget);
        }

        StartCoroutine(EnemyEndTurnDelay(0.5f));
    }

    protected IEnumerator RageEndTurn(float seconds)
    {
        yield return new WaitUntil(() => enemyEndingTurn);

        yield return new WaitForSeconds(seconds);

        enemyState = EnemyState.ENDING;

        yield return new WaitForSeconds(seconds);

        enemyState = EnemyState.WAITING;
        rageCount = 1;
    }

    protected override IEnumerator EnemyDeath()
    {
        gameObject.tag = "Dead";

        #region Goat Death State
        Enrage enrage = FindObjectOfType<Enrage>();
        B_AttackBuff atkBuff = GetComponentInChildren<B_AttackBuff>();
        EnemyRefreshTargets();
        
        if (!dead)
        {
            dead = true;
            for (int i = 0; i < enemyTargets.Length; i++)
            {
                CharacterStats enemyStats = enemyTargets[i].GetComponent<CharacterStats>();

                enemyStats.BuffText(atkBuff.skillBuffPercent, "enrage");

                if (enemyStats.enrageCounter <= 0) //don't overstack attack
                {
                    enrage.EnrageCalculation(enemyStats, atkBuff.skillBuffPercent);
                }

                _StatusEffectHud statusEffect = FindObjectOfType<_StatusEffectHud>();
                statusEffect.SpawnEffectsBar(enemyStats, 99, "enrage");

                enemyStats.enrageCounter += 99;
                if (enemyStats.enrageCounter > enrage.enrageLimit)
                {
                    enemyStats.enrageCounter = enrage.enrageLimit;
                }
            }
        }
        #endregion

        //animator.SetTrigger("Death");

        #region Update Turn Order
        battleManager.UpdateTurnOrderUi("death", characterStats);
        battleManager.charactersList.Remove(characterStats);
        battleManager.turnOrderList.Remove(characterStats);
        battleManager.originalTurnOrderList.Remove(characterStats);
        #endregion

        yield return new WaitForSeconds(1f);

        characterStats.characterHpHud.SetActive(false);

        enemyState = EnemyState.ENDING;

        yield return new WaitForSeconds(0.5f);

        gameObject.SetActive(false);
    }
}
