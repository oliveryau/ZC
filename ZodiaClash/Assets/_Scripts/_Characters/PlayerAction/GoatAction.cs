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
            TargetSelectionUi(true, "enemy", "taunt"); //single target
            StartCoroutine(cam.ZoomIn(enemyTeamCamPoint));
        }
        else if (selectedSkillPrefab == skill2Prefab)
        {
            TargetSelectionUi(true, "ally", "speedBuff"); //ally target excluding self
            StartCoroutine(cam.ZoomIn(playerTeamCamPoint));
        }
        else if (selectedSkillPrefab == skill3Prefab)
        {
            TargetSelectionUi(true, "ally"); //ally target
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
                        //hit.collider.GetComponent<CharacterStats>().healthPanel.color = hit.collider.GetComponent<CharacterStats>().healthPanelTargetColor;

                        if (Input.GetMouseButtonDown(0))
                        {
                            selectedTarget = hit.collider.gameObject;
                            playerChi.RegainChi();

                            playerState = PlayerState.ATTACKING;

                            TargetSelectionUi(false, null);
                            //hit.collider.GetComponent<CharacterStats>().healthPanel.color = hit.collider.GetComponent<CharacterStats>().healthPanelOriginalColor;
                        }
                    }
                }
            }
            #endregion

            #region Unaffected Taunt Skill Behaviour
            if (selectedSkillPrefab == skill2Prefab) //ally targeting excluding self
            {
                if (hit.collider != null && hit.collider.CompareTag("Player"))
                {
                    #region Cannot Target Itself
                    if (hit.collider.gameObject == this.gameObject)
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
                            if (playerChi.currentChi >= skill2ChiCost)
                            {
                                selectedTarget = hit.collider.gameObject;
                                playerChi.UseChi(skill2ChiCost);

                                playerState = PlayerState.ATTACKING;

                                TargetSelectionUi(false, null);
                                hit.collider.GetComponent<CharacterStats>().playerAvatar.color = hit.collider.GetComponent<CharacterStats>().playerAvatarOriginalColor;
                            }
                        }
                    }
                }
            }
            else if (selectedSkillPrefab == skill3Prefab) //ally targeting
            {
                if (hit.collider != null && hit.collider.CompareTag("Player"))
                {
                    hit.collider.GetComponent<_PlayerAction>().HighlightTargetIndicator(true);
                    hit.collider.GetComponent<CharacterStats>().playerAvatar.color = hit.collider.GetComponent<CharacterStats>().playerAvatarTargetColor;

                    if (Input.GetMouseButtonDown(0))
                    {
                        if (playerChi.currentChi >= skill3ChiCost)
                        {
                            selectedTarget = hit.collider.gameObject;
                            playerChi.UseChi(skill3ChiCost);

                            playerState = PlayerState.ATTACKING;

                            TargetSelectionUi(false, null);
                            hit.collider.GetComponent<CharacterStats>().playerAvatar.color = hit.collider.GetComponent<CharacterStats>().playerAvatarOriginalColor;
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
        if (selectedSkillPrefab == skill1Prefab)
        {
            targetPosition = selectedTarget.transform.Find("Target Position");
            movingToTarget = true;
        }
        #endregion
        #region Non-Movement Skills
        else if (selectedSkillPrefab == skill2Prefab || selectedSkillPrefab == skill3Prefab)
        {
            AttackAnimation();
        }
        #endregion
    }

    protected override void AttackAnimation()
    {
        #region Movement Skills
        if (selectedSkillPrefab == skill1Prefab)
        {
            StartCoroutine(AttackStartDelay(0.5f, 1f));
        }
        #endregion
        #region Non-Movement Skills
        else if (selectedSkillPrefab == skill2Prefab || selectedSkillPrefab == skill3Prefab)
        {
            StartCoroutine(BuffStartDelay(0.5f, 1f));
        }
        #endregion
    }

    protected override void ApplySkill()
    {
        if (selectedSkillPrefab == skill1Prefab)
        {
            //single target def break skill
            selectedSkillPrefab.GetComponent<A_AttackDefBreak>().Attack(selectedTarget);
            AudioManager.Instance.PlayEffectsOneShot("Goat 1");
        }
        else if (selectedSkillPrefab == skill2Prefab)
        {
            //ally attack buff skill
            selectedSkillPrefab.GetComponent<B_AttackBuff>().AttackBuff(selectedTarget);
            AudioManager.Instance.PlayEffectsOneShot("Goat 2");
        }
        else if (selectedSkillPrefab == skill3Prefab)
        {
            //ally heal cleanse skill
            selectedSkillPrefab.GetComponent<C_SingleHeal>().Heal(selectedTarget);
            AudioManager.Instance.PlayEffectsOneShot("Goat 3");
        }

        StartCoroutine(EndTurnDelay(0.5f));
    }
}
