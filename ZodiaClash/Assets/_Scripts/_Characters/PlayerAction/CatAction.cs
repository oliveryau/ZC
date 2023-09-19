using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAction : _PlayerAction
{
    private void Update()
    {
        UpdatePlayerState();
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

                        selectedTarget.GetComponent<CharacterStats>().healthPanel.color = Color.black;

                        if (Input.GetMouseButtonDown(0))
                        {
                            playerChi.RegainChi();

                            playerState = PlayerState.ATTACKING;

                            TargetSelectionUi(false, null);

                            selectedTarget.GetComponent<CharacterStats>().healthPanel.color = Color.clear;
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

                            enemy.GetComponent<CharacterStats>().healthPanel.color = Color.black;
                        }

                        if (Input.GetMouseButtonDown(0))
                        {
                            if (playerChi.currentChi >= skill2ChiCost)
                            {
                                playerChi.UseChi(skill2ChiCost);

                                playerState = PlayerState.ATTACKING;

                                TargetSelectionUi(false, null);

                                foreach (GameObject enemy in enemyTargets)
                                {
                                    enemy.GetComponent<CharacterStats>().healthPanel.color = Color.clear;
                                }
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

                            enemy.GetComponent<CharacterStats>().healthPanel.color = Color.black;
                        }

                        if (Input.GetMouseButtonDown(0))
                        {
                            if (playerChi.currentChi >= skill3ChiCost)
                            {
                                playerChi.UseChi(skill3ChiCost);

                                playerState = PlayerState.ATTACKING;

                                TargetSelectionUi(false, null);

                                foreach (GameObject enemy in enemyTargets)
                                {
                                    enemy.GetComponent<CharacterStats>().healthPanel.color = Color.clear;
                                }
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

                        hit.collider.GetComponent<CharacterStats>().healthPanel.color = Color.black;

                        if (Input.GetMouseButtonDown(0))
                        {
                            selectedTarget = hit.collider.gameObject;
                            playerChi.RegainChi();

                            playerState = PlayerState.ATTACKING;

                            TargetSelectionUi(false, null);

                            hit.collider.GetComponent<CharacterStats>().healthPanel.color = Color.clear;
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

                            enemy.GetComponent<CharacterStats>().healthPanel.color = Color.black;
                        }

                        if (Input.GetMouseButtonDown(0))
                        {
                            if (playerChi.currentChi >= skill2ChiCost)
                            {
                                playerChi.UseChi(skill2ChiCost);

                                playerState = PlayerState.ATTACKING;

                                TargetSelectionUi(false, null);

                                foreach (GameObject enemy in enemyTargets)
                                {
                                    enemy.GetComponent<CharacterStats>().healthPanel.color = Color.clear;
                                }
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

                            enemy.GetComponent<CharacterStats>().healthPanel.color = Color.black;
                        }

                        if (Input.GetMouseButtonDown(0))
                        {
                            if (playerChi.currentChi >= skill3ChiCost)
                            {
                                playerChi.UseChi(skill3ChiCost);

                                playerState = PlayerState.ATTACKING;

                                TargetSelectionUi(false, null);

                                foreach (GameObject enemy in enemyTargets)
                                {
                                    enemy.GetComponent<CharacterStats>().healthPanel.color = Color.clear;
                                }
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

                    enemy.GetComponent<CharacterStats>().healthPanel.color = Color.clear;
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
            StartCoroutine(AttackStartDelay(0.5f, 1f));
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
