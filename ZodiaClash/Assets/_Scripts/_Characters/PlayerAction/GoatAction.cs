using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoatAction : _PlayerAction
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
            TargetSelectionUi(true, "enemy");
        }
        else if (selectedSkillPrefab == skill2Prefab || selectedSkillPrefab == skill3Prefab)
        {
            TargetSelectionUi(true, "ally");
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
            }
            #endregion
            #region Normal Targeting Behaviour
            else if (!characterStats.tauntCheck)
            {
                if (selectedSkillPrefab == skill1Prefab) //single targeting
                {
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
                else if (selectedSkillPrefab == skill2Prefab) //ally targeting
                {
                    if (hit.collider != null && hit.collider.CompareTag("Player"))
                    {
                        hit.collider.GetComponent<_PlayerAction>().HighlightTargetIndicator(true);

                        if (Input.GetMouseButtonDown(0))
                        {
                            if (playerChi.currentChi < skill2ChiCost)
                            {
                                Debug.LogError("Cannot use skill!");
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
                else if (selectedSkillPrefab == skill3Prefab) //ally targeting
                {
                    if (hit.collider != null && hit.collider.CompareTag("Player"))
                    {
                        hit.collider.GetComponent<_PlayerAction>().HighlightTargetIndicator(true);

                        if (Input.GetMouseButtonDown(0))
                        {
                            if (playerChi.currentChi < skill3ChiCost)
                            {
                                Debug.LogError("Cannot use skill!");
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

            #region No Detections on Target
            if (hit.collider == null)
            {
                foreach (GameObject enemy in enemyTargets)
                {
                    enemy.GetComponent<_EnemyAction>().EnemyHighlightTargetIndicator(false);
                }

                foreach (GameObject player in playerTargets)
                {
                    player.GetComponent<_PlayerAction>().HighlightTargetIndicator(false);
                }
            }
            #endregion
        }
    }

    protected override void UseSkill()
    {
        playerAttacking = true;

        if (selectedSkillPrefab == skill1Prefab) //skills that require movement
        {
            movingToTarget = true; //movement is triggered
        }
        else if (selectedSkillPrefab == skill2Prefab || selectedSkillPrefab == skill3Prefab)
        {
            AttackAnimation();
        }
    }

    protected override void AttackAnimation()
    {
        if (selectedSkillPrefab == skill1Prefab)
        {
            StartCoroutine(AttackStartDelay(0.5f, 0.5f));
        }
        else if (selectedSkillPrefab == skill2Prefab || selectedSkillPrefab == skill3Prefab)
        {
            StartCoroutine(BuffStartDelay(0.5f, 0.5f));
        }
    }

    protected override void ApplySkill()
    {
        if (selectedSkillPrefab == skill1Prefab)
        {
            //single target def break skill
            selectedSkillPrefab.GetComponent<A_AttackDefBreak>().Attack(selectedTarget);
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

        StartCoroutine(EndTurnDelay(1f));
    }
}
