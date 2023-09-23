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
            TargetSelectionUi(true, "enemy", characterStats.tauntCounter > 0);
        }
        else if (selectedSkillPrefab == skill2Prefab || selectedSkillPrefab == skill3Prefab) //aoe
        {
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
            if (characterStats.tauntCounter > 0)
            {
                if (selectedSkillPrefab == skill1Prefab) //single targeting
                {
                    if (hit.collider != null && hit.collider.CompareTag("Enemy"))
                    {
                        if (hit.collider.gameObject != selectedTarget.gameObject)
                        {
                            selectedTarget.GetComponent<_EnemyAction>().EnemyHighlightTargetIndicator(false);
                        }
                        else
                        {
                            //can only target taunted character

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
                }
            }
            #endregion
            #region Normal Targeting Behaviour
            else if (characterStats.tauntCounter <= 0)
            {
                if (selectedSkillPrefab == skill1Prefab) //single targeting
                {
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
            //aoe activate bleed skill
            selectedSkillPrefab.GetComponent<C_AoeActivateBleed>().Attack(enemyTargets);
        }

        StartCoroutine(EndTurnDelay(0.5f));
    }
}
