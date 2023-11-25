using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxAction : _PlayerAction
{
    private void Update()
    {
        UpdatePlayerState();
    }

    protected override void StartVoice()
    {
        //AudioManager.Instance.PlayEffectsOneShot("Ox Start");
    }

    public override void SelectSkill(string btn)
    {
        base.SelectSkill(btn);

        if (selectedSkillPrefab == skill1Prefab)
        {
            TargetSelectionUi(true, "enemy", "taunt"); //single target
            StartCoroutine(cam.ZoomIn(enemyTeamCamPoint));
        }
        else if (selectedSkillPrefab == skill2Prefab)
        {
            TargetSelectionUi(true, "enemy", "taunt"); //single target
            StartCoroutine(cam.ZoomIn(enemyTeamCamPoint));
        }
        else if (selectedSkillPrefab == skill3Prefab)
        {
            TargetSelectionUi(true, "ally", "taunt"); //self target
            StartCoroutine(cam.ZoomIn(playerTeamCamPoint));
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
                else if (selectedSkillPrefab == skill2Prefab) //single targeting
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

                            if (Input.GetMouseButtonDown(0))
                            {
                                if (playerChi.currentChi >= skill2ChiCost)
                                {
                                    playerChi.UseChi(skill2ChiCost);

                                    playerState = PlayerState.ATTACKING;

                                    TargetSelectionUi(false, null);
                                }
                            }
                        }
                    }
                }
            }
            #endregion
            #region Non-Taunted Behaviour
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
                else if (selectedSkillPrefab == skill2Prefab) //single targeting
                {
                    if (hit.collider != null && hit.collider.CompareTag("Enemy"))
                    {
                        hit.collider.GetComponent<_EnemyAction>().EnemyHighlightTargetIndicator(true);

                        if (Input.GetMouseButtonDown(0))
                        {
                            if (playerChi.currentChi >= skill2ChiCost)
                            {
                                selectedTarget = hit.collider.gameObject;
                                playerChi.UseChi(skill2ChiCost);

                                playerState = PlayerState.ATTACKING;

                                TargetSelectionUi(false, null);
                            }
                        }
                    }
                }
            }
            #endregion

            #region Unaffected Taunt Skill Behaviour
            if (selectedSkillPrefab == skill3Prefab) //self targeting
            {
                if (hit.collider != null && hit.collider.CompareTag("Player"))
                {
                    #region Only Target Self
                    if (hit.collider.gameObject != this.gameObject)
                    {
                        hit.collider.GetComponent<_PlayerAction>().HighlightTargetIndicator(false);
                    }
                    #endregion
                    else
                    {
                        hit.collider.GetComponent<_PlayerAction>().HighlightTargetIndicator(true);
                        hit.collider.GetComponent<CharacterStats>().playerAvatar.color = hit.collider.GetComponent<CharacterStats>().playerAvatarTargetColor;

                        if (Input.GetMouseButtonDown(0))
                        {
                            if (playerChi.currentChi >= skill3ChiCost)
                            {
                                playerChi.UseChi(skill3ChiCost);

                                playerState = PlayerState.ATTACKING;

                                TargetSelectionUi(false, null);
                                hit.collider.GetComponent<CharacterStats>().playerAvatar.color = hit.collider.GetComponent<CharacterStats>().playerAvatarOriginalColor;
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

                foreach (GameObject player in playerTargets)
                {
                    player.GetComponent<_PlayerAction>().HighlightTargetIndicator(false);
                    player.GetComponent<CharacterStats>().playerAvatar.color = player.GetComponent<CharacterStats>().playerAvatarOriginalColor;
                }
            }
            #endregion
        }
    }

    protected override void UseSkill()
    {
        playerAttacking = true;

        #region Movement Skills
        if (selectedSkillPrefab == skill1Prefab || selectedSkillPrefab == skill2Prefab)
        {
            targetPosition = selectedTarget.transform.Find("Target Position");
            movingToTarget = true; //movement is triggered
        }
        #endregion
        #region Non-Movement Skills
        else if (selectedSkillPrefab == skill3Prefab)
        {
            AttackAnimation();
            StartCoroutine(cam.ZoomIn(enemyTeamCamPoint));
        }
        #endregion
    }

    protected override void AttackAnimation()
    {
        #region Movement Skills
        if (selectedSkillPrefab == skill1Prefab || selectedSkillPrefab == skill2Prefab)
        {
            StartCoroutine(AttackStartDelay(0.5f, 0.5f));
        }
        #endregion
        #region Non-Movement Skills
        else if (selectedSkillPrefab = skill3Prefab)
        {
            StartCoroutine(BuffStartDelay(0.5f, 0.5f));
        }
        #endregion
    }

    protected override void ApplySkill()
    {
        if (selectedSkillPrefab == skill1Prefab)
        {
            //single target lifesteal skill
            selectedSkillPrefab.GetComponent<A_SingleDefBreak>().Attack(selectedTarget);
            AudioManager.Instance.PlayEffectsOneShot("Ox 1");
        }
        else if (selectedSkillPrefab == skill2Prefab)
        {
            //single target stun skill
            selectedSkillPrefab.GetComponent<B_SingleStun>().Attack(selectedTarget);
            AudioManager.Instance.PlayEffectsOneShot("Ox 2");
        }
        else if (selectedSkillPrefab == skill3Prefab)
        {
            //aoe target taunt skill
            selectedSkillPrefab.GetComponent<C_AoeTaunt>().Attack(enemyTargets);
            AudioManager.Instance.PlayEffectsOneShot("Ox 3");
        }

        StartCoroutine(EndTurnDelay(0.5f));
    }
}
