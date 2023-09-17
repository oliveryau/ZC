using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAction : _PlayerAction
{
    private void Update()
    {
        if (gameManager.state == BattleState.PLAYERTURN && gameManager.activePlayer == gameObject.name)
        {
            if (playerState == PlayerState.WAITING)
            {
                playerState = PlayerState.CHECKSTATUS;
            }

            else if (playerState == PlayerState.CHECKSTATUS)
            {
                if (!characterStats.checkedStatus && !checkingStatus)
                {
                    turnIndicator.SetActive(true);

                    StartCoroutine(characterStats.CheckStatusEffects());
                    checkingStatus = true;
                }
                else if (characterStats.stunCheck)
                {
                    playerState = PlayerState.ENDING;

                    characterStats.stunCheck = false;
                    checkingStatus = false;
                }
                else if (characterStats.checkedStatus)
                {
                    playerState = PlayerState.PLAYERSELECTION;
                    checkingStatus = false;
                }
            }

            else if (playerState == PlayerState.PLAYERSELECTION)
            {
                RefreshTargets();

                ToggleSkillUi(true);

                SelectTarget();
            }

            else if (playerState == PlayerState.ATTACKING)
            {
                ToggleSkillUi(false);

                if (!playerAttacking)
                {
                    UseSkill();
                }

                PlayerMovement();
            }

            else if (playerState == PlayerState.ENDING)
            {
                characterStats.CheckEndStatusEffects();

                gameManager.state = BattleState.NEXTTURN;

                turnIndicator.SetActive(false);

                selectedSkillPrefab = null;
                enemyTargets = null;
                selectedTarget = null;

                playerAttacking = false;
                endingTurn = false;

                characterStats.checkedStatus = false;

                playerState = PlayerState.WAITING;
            }
        }
    }

    public override void SelectSkill(string btn)
    {
        base.SelectSkill(btn);

        if (selectedSkillPrefab == skill1Prefab)
        {
            aoeSkillSelected = false;

            TargetSelectionUi(true, "enemy");
        }
        else if (selectedSkillPrefab == skill2Prefab || selectedSkillPrefab == skill3Prefab) //aoe
        {
            aoeSkillSelected = true;

            TargetSelectionUi(true, "enemy");
        }
    }

    protected override void SelectTarget()
    {
        if (selectedSkillPrefab != null)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            #region Taunted Behaviour
            if (characterStats.tauntCheck)
            {
                if (selectedSkillPrefab == skill1Prefab) //single targeting
                {
                    aoeSkillSelected = false;

                    if (hit.collider != null && hit.collider.CompareTag("Enemy"))
                    {
                        selectedTarget.GetComponent<_EnemyAction>().EnemyHighlightTargetIndicator(true);

                        if (Input.GetMouseButtonDown(0))
                        {
                            playerChi.RegainChi();

                            playerState = PlayerState.ATTACKING;

                            TargetSelectionUi(false, null);
                        }
                    }
                }
                else if (selectedSkillPrefab == skill2Prefab) //aoe targeting
                {
                    aoeSkillSelected = true;

                    if (hit.collider != null && hit.collider.CompareTag("Enemy"))
                    {
                        foreach (GameObject enemy in enemyTargets)
                        {
                            enemy.GetComponent<_EnemyAction>().EnemyHighlightTargetIndicator(true);
                        }

                        if (Input.GetMouseButtonDown(0))
                        {
                            if (playerChi.currentChi < skill2ChiCost)
                            {
                                Debug.LogError("Cannot use skill!");
                            }
                            else if (playerChi.currentChi >= skill2ChiCost)
                            {
                                playerChi.UseChi(skill2ChiCost);

                                playerState = PlayerState.ATTACKING;

                                TargetSelectionUi(false, null);
                            }
                        }
                    }
                }
                else if (selectedSkillPrefab == skill3Prefab) //aoe targeting
                {
                    aoeSkillSelected = true;

                    if (hit.collider != null && hit.collider.CompareTag("Enemy"))
                    {
                        foreach (GameObject enemy in enemyTargets)
                        {
                            enemy.GetComponent<_EnemyAction>().EnemyHighlightTargetIndicator(true);
                        }

                        if (Input.GetMouseButtonDown(0))
                        {
                            if (playerChi.currentChi < skill3ChiCost)
                            {
                                Debug.LogError("Cannot use skill!");
                            }
                            else if (playerChi.currentChi >= skill3ChiCost)
                            {
                                playerChi.UseChi(skill3ChiCost);

                                playerState = PlayerState.ATTACKING;

                                TargetSelectionUi(false, null);
                            }
                        }
                    }
                }
            }
            #endregion
            #region Normal Targeting Behaviour
            else if (!characterStats.tauntCheck)
            {
                if (selectedSkillPrefab == skill1Prefab) //single targeting
                {
                    aoeSkillSelected = false;

                    if (hit.collider != null && hit.collider.CompareTag("Enemy"))
                    {
                        hit.collider.GetComponent<_EnemyAction>().EnemyHighlightTargetIndicator(true);

                        if (Input.GetMouseButtonDown(0))
                        {
                            selectedTarget = hit.collider.gameObject;
                            playerChi.RegainChi();

                            playerState = PlayerState.ATTACKING;

                            TargetSelectionUi(false, null);
                        }
                    }
                }
                else if (selectedSkillPrefab == skill2Prefab) //aoe targeting
                {
                    aoeSkillSelected = true;

                    if (hit.collider != null && hit.collider.CompareTag("Enemy"))
                    {
                        foreach (GameObject enemy in enemyTargets)
                        {
                            enemy.GetComponent<_EnemyAction>().EnemyHighlightTargetIndicator(true);
                        }

                        if (Input.GetMouseButtonDown(0))
                        {
                            if (playerChi.currentChi < skill2ChiCost)
                            {
                                Debug.LogError("Cannot use skill!");
                            }
                            else if (playerChi.currentChi >= skill2ChiCost)
                            {
                                playerChi.UseChi(skill2ChiCost);

                                playerState = PlayerState.ATTACKING;

                                TargetSelectionUi(false, null);
                            }
                        }
                    }
                }
                else if (selectedSkillPrefab == skill3Prefab) //aoe targeting
                {
                    aoeSkillSelected = true;

                    if (hit.collider != null && hit.collider.CompareTag("Enemy"))
                    {
                        foreach (GameObject enemy in enemyTargets)
                        {
                            enemy.GetComponent<_EnemyAction>().EnemyHighlightTargetIndicator(true);
                        }

                        if (Input.GetMouseButtonDown(0))
                        {
                            if (playerChi.currentChi < skill3ChiCost)
                            {
                                Debug.LogError("Cannot use skill!");
                            }
                            else if (playerChi.currentChi >= skill3ChiCost)
                            {
                                playerChi.UseChi(skill3ChiCost);

                                playerState = PlayerState.ATTACKING;

                                TargetSelectionUi(false, null);
                            }
                        }
                    }
                }
            }
            #endregion

            #region No Detection on Targets
            if (hit.collider == null)
            {
                foreach (GameObject enemy in enemyTargets)
                {
                    enemy.GetComponent<_EnemyAction>().EnemyHighlightTargetIndicator(false);
                }
            }
            #endregion
        }
    }

    protected override void UseSkill()
    {
        playerAttacking = true;

        if (selectedSkillPrefab == skill1Prefab || selectedSkillPrefab == skill2Prefab || selectedSkillPrefab == skill3Prefab) //skills that require movement
        {
            movingToTarget = true; //movement is triggered
        }
    }

    protected override void AttackAnimation()
    {
        if (selectedSkillPrefab == skill1Prefab || selectedSkillPrefab == skill2Prefab || selectedSkillPrefab == skill3Prefab)
        {
            StartCoroutine(AttackStartDelay(0.5f, 0.5f));
        }
    }

    protected override void ApplySkill()
    {
        if (selectedSkillPrefab == skill1Prefab)
        {
            //single target DoT skill
            selectedSkillPrefab.GetComponent<A_SingleBleed>().Attack(selectedTarget);
        }
        else if (selectedSkillPrefab == skill2Prefab)
        {
            //aoe target DoT skill
            selectedSkillPrefab.GetComponent<B_AoeBleed>().Attack(enemyTargets);
        }
        else if (selectedSkillPrefab == skill3Prefab)
        {
            //strong single target DoT skill
            selectedSkillPrefab.GetComponent<C_AoeActivateBleed>().Attack(enemyTargets);
        }

        StartCoroutine(EndTurnDelay(1f));
    }
}
