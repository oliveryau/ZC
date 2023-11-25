using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAction : _PlayerAction
{
    private void Update()
    {
        UpdatePlayerState();
    }

    protected override void StartVoice()
    {
        AudioManager.Instance.PlayEffectsOneShot("Cat Start");
    }

    public override void SelectSkill(string btn)
    {
        base.SelectSkill(btn);

        if (selectedSkillPrefab == skill1Prefab) //single target
        {
            TargetSelectionUi(true, "enemy", "taunt");
        }
        else if (selectedSkillPrefab == skill2Prefab || selectedSkillPrefab == skill3Prefab) //aoe target
        {
            TargetSelectionUi(true, "enemy");
        }

        StartCoroutine(cam.ZoomIn(enemyTeamCamPoint));
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
                            selectedTarget.GetComponent<_EnemyAction>().EnemyHighlightTargetIndicator(true);
                            //selectedTarget.GetComponent<CharacterStats>().healthPanel.color = selectedTarget.GetComponent<CharacterStats>().healthPanelTargetColor;

                            if (Input.GetMouseButtonDown(0))
                            {
                                playerChi.RegainChi();

                                playerState = PlayerState.ATTACKING;

                                TargetSelectionUi(false, null);
                                //selectedTarget.GetComponent<CharacterStats>().healthPanel.color = selectedTarget.GetComponent<CharacterStats>().healthPanelOriginalColor;
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

                        if (Input.GetMouseButtonDown(0))
                        {
                            selectedTarget = hit.collider.gameObject;
                            playerChi.RegainChi();

                            playerState = PlayerState.ATTACKING;

                            TargetSelectionUi(false, null);
                        }
                    }
                }
            }
            #endregion

            #region Unaffected Taunt Skill Behaviour
            if (selectedSkillPrefab == skill2Prefab) //aoe targeting
            {
                if (hit.collider != null && hit.collider.CompareTag("Enemy"))
                {
                    foreach (GameObject enemy in enemyTargets)
                    {
                        enemy.GetComponent<_EnemyAction>().EnemyHighlightTargetIndicator(true);
                        enemy.GetComponent<CharacterStats>().healthPanel.GetComponent<Animator>().SetBool("hover", true);
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
                                enemy.GetComponent<CharacterStats>().healthPanel.GetComponent<Animator>().SetBool("hover", false);
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
                        enemy.GetComponent<CharacterStats>().healthPanel.GetComponent<Animator>().SetBool("hover", true);
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
                                enemy.GetComponent<CharacterStats>().healthPanel.GetComponent<Animator>().SetBool("hover", false);
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

        #region Movement Skills
        if (selectedSkillPrefab == skill1Prefab)
        {
            targetPosition = selectedTarget.transform.Find("Target Position");
            characterStats.animator.SetTrigger("skill1");
        }
        else if (selectedSkillPrefab == skill2Prefab)
        {
            targetPosition = aoeTargetPosition; //aoe target pos
            characterStats.animator.SetTrigger("skill2");
        }
        else if (selectedSkillPrefab == skill3Prefab)
        {
            targetPosition = aoeTargetPosition; //aoe target pos
            characterStats.animator.SetTrigger("skill3");
        }

        movingToTarget = true; //movement is triggered
        #endregion
    }

    protected override void AttackAnimation()
    {
        #region Movement Skills
        if (selectedSkillPrefab == skill1Prefab)
        {
            StartCoroutine(AttackStartDelay(0.5f, 0.5f));
        }
        else if (selectedSkillPrefab == skill2Prefab)
        {
            StartCoroutine(AttackStartDelay(0.5f, 0.5f));
        }
        else if (selectedSkillPrefab == skill3Prefab)
        {
            StartCoroutine(AttackStartDelay(0.5f, 0.5f));
        }
        #endregion
    }

    protected override void ApplySkill()
    {
        if (selectedSkillPrefab == skill1Prefab)
        {
            //single target DoT skill
            selectedSkillPrefab.GetComponent<A_SingleBleed>().Attack(selectedTarget);
            AudioManager.Instance.PlayEffectsOneShot("Cat 1");
        }
        else if (selectedSkillPrefab == skill2Prefab)
        {
            //aoe target DoT skill
            selectedSkillPrefab.GetComponent<B_AoeBleed>().Attack(enemyTargets);
            AudioManager.Instance.PlayEffectsOneShot("Cat 2");
        }
        else if (selectedSkillPrefab == skill3Prefab)
        {
            //aoe activate bleed skill
            selectedSkillPrefab.GetComponent<C_AoeActivateBleed>().Attack(enemyTargets);
            AudioManager.Instance.PlayEffectsOneShot("Cat 3");
        }

        StartCoroutine(EndTurnDelay(0.5f));
    }
}
