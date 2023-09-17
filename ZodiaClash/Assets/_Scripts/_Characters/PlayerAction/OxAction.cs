using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxAction : _PlayerAction
{
    private void Update()
    {
        UpdatePlayerState();
    }

    public override void SelectSkill(string btn)
    {
        base.SelectSkill(btn);

        if (selectedSkillPrefab == skill1Prefab || selectedSkillPrefab == skill2Prefab || selectedSkillPrefab == skill3Prefab)
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
            if (characterStats.tauntCheck)
            {
                if (selectedSkillPrefab == skill1Prefab) //aoe targeting
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
                            playerChi.RegainChi();

                            playerState = PlayerState.ATTACKING;

                            TargetSelectionUi(false, null);
                        }
                    }
                }
                else if (selectedSkillPrefab == skill2Prefab) //single targeting
                {
                    aoeSkillSelected = false;

                    if (hit.collider != null && hit.collider.CompareTag("Enemy"))
                    {
                        selectedTarget.GetComponent<_EnemyAction>().EnemyHighlightTargetIndicator(true);

                        if (Input.GetMouseButtonDown(0))
                        {
                            if (playerChi.currentChi < skill2ChiCost)
                            {
                                Debug.Log("Cannot use skill!");
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
                else if (selectedSkillPrefab == skill3Prefab) //single targeting
                {
                    aoeSkillSelected = false;

                    if (hit.collider != null && hit.collider.CompareTag("Enemy"))
                    {
                        selectedTarget.GetComponent<_EnemyAction>().EnemyHighlightTargetIndicator(true);

                        if (Input.GetMouseButtonDown(0))
                        {
                            if (playerChi.currentChi < skill3ChiCost)
                            {
                                Debug.Log("Cannot use skill!");
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
                if (selectedSkillPrefab == skill1Prefab) //aoe targeting
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
                            playerChi.RegainChi();

                            playerState = PlayerState.ATTACKING;

                            TargetSelectionUi(false, null);
                        }
                    }
                }
                else if (selectedSkillPrefab == skill2Prefab) //single targeting
                {
                    aoeSkillSelected = false;

                    if (hit.collider != null && hit.collider.CompareTag("Enemy"))
                    {
                        hit.collider.GetComponent<_EnemyAction>().EnemyHighlightTargetIndicator(true);

                        if (Input.GetMouseButtonDown(0))
                        {
                            if (playerChi.currentChi < skill2ChiCost)
                            {
                                Debug.Log("Cannot use skill!");
                            }
                            else if (playerChi.currentChi >= skill2ChiCost)
                            {
                                selectedTarget = hit.collider.gameObject;
                                playerChi.UseChi(skill2ChiCost);

                                playerState = PlayerState.ATTACKING;

                                TargetSelectionUi(false, null);
                            }
                        }
                    }
                }
                else if (selectedSkillPrefab == skill3Prefab) //single targeting
                {
                    aoeSkillSelected = false;

                    if (hit.collider != null && hit.collider.CompareTag("Enemy"))
                    {
                        hit.collider.GetComponent<_EnemyAction>().EnemyHighlightTargetIndicator(true);

                        if (Input.GetMouseButtonDown(0))
                        {
                            if (playerChi.currentChi < skill3ChiCost)
                            {
                                Debug.Log("Cannot use skill!");
                            }
                            else if (playerChi.currentChi >= skill3ChiCost)
                            {
                                selectedTarget = hit.collider.gameObject;
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

        if (selectedSkillPrefab == skill1Prefab || selectedSkillPrefab == skill2Prefab) //skills that require movement
        {
            movingToTarget = true; //movement is triggered
        }
        else if (selectedSkillPrefab == skill3Prefab) //skills that do not require movement
        {
            AttackAnimation();
        }
    }

    protected override void AttackAnimation()
    {
        if (selectedSkillPrefab == skill1Prefab || selectedSkillPrefab == skill2Prefab)
        {
            StartCoroutine(AttackStartDelay(0.5f, 0.5f));
        }
        else if (selectedSkillPrefab == skill3Prefab)
        {
            StartCoroutine(BuffStartDelay(0.5f, 1f));
        }
    }

    protected override void ApplySkill()
    {
        if (selectedSkillPrefab == skill1Prefab)
        {
            //aoe attack skill
            selectedSkillPrefab.GetComponent<AoeAttack>().Attack(enemyTargets);
        }
        else if (selectedSkillPrefab == skill2Prefab)
        {
            //single target stun skill
            selectedSkillPrefab.GetComponent<B_SingleStun>().Attack(selectedTarget);
        }
        else if (selectedSkillPrefab == skill3Prefab)
        {
            //single target taunt skill
            selectedSkillPrefab.GetComponent<C_SingleTaunt>().Taunt(selectedTarget);
        }

        StartCoroutine(EndTurnDelay(1f));
    }
}
