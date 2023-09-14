using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxAction : _PlayerAction
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
                selectedTarget = null;
                enemyTargets = null;

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
                        playerState = PlayerState.ATTACKING;

                        TargetSelectionUi(false, null);
                    }
                }
            }
            else if (selectedSkillPrefab == skill2Prefab || selectedSkillPrefab == skill3Prefab) //single targeting
            {
                aoeSkillSelected = false;

                if (hit.collider != null && hit.collider.CompareTag("Enemy"))
                {
                    hit.collider.GetComponent<_EnemyAction>().EnemyHighlightTargetIndicator(true);

                    if (Input.GetMouseButtonDown(0))
                    {
                        selectedTarget = hit.collider.gameObject;

                        playerState = PlayerState.ATTACKING;

                        TargetSelectionUi(false, null);
                    }
                }
            }

            if (hit.collider == null)
            {
                foreach (GameObject enemy in enemyTargets)
                {
                    enemy.GetComponent<_EnemyAction>().EnemyHighlightTargetIndicator(false);
                }
            }
        }
    }

    protected override void UseSkill()
    {
        playerAttacking = true;

        if (selectedSkillPrefab == skill1Prefab) //skills that require movement
        {
            movingToTarget = true; //movement is triggered
        }
        //else if (selectedSkillPrefab == skill2Prefab || selectedSkillPrefab == skill3Prefab) //skills that do not require movement
        //{
        //    AttackAnimation();
        //}
    }

    protected override void AttackAnimation()
    {
        if (selectedSkillPrefab == skill1Prefab)
        {
            StartCoroutine(AttackStartDelay(0.5f, 0.5f));
        }
        //else if (selectedSkillPrefab == skill2Prefab)
        //{
        //    StartCoroutine(BuffStartDelay(0.5f, 1f));
        //}
        //else if (selectedSkillPrefab == skill3Prefab)
        //{
        //    StartCoroutine(BuffStartDelay(0.5f, 1f));
        //}
    }

    protected override void ApplySkill()
    {
        if (selectedSkillPrefab == skill1Prefab)
        {
            //aoe target skill
            selectedSkillPrefab.GetComponent<AoeAttack>().Attack(enemyTargets);
        }
        else if (selectedSkillPrefab == skill2Prefab)
        {
            //single target stun skill
        }
        else if (selectedSkillPrefab == skill3Prefab)
        {
            //single target taunt skill
        }

        StartCoroutine(EndTurnDelay(1f));
    }
}
