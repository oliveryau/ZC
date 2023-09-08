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
                if (!characterStats.checkedStatus)
                {
                    characterStats.CheckStatusEffects();
                }
                else if (characterStats.checkedStatus)
                {
                    playerState = PlayerState.PLAYERSELECTION;
                }
            }

            else if (playerState == PlayerState.PLAYERSELECTION)
            {
                RefreshTargets();

                ToggleUi(true);

                SelectTarget();
            }

            else if (playerState == PlayerState.ATTACKING)
            {
                ToggleUi(false);

                if (!playerAttacking)
                {
                    UseSkill();
                }

                PlayerMovement();
            }

            else if (playerState == PlayerState.ENDING)
            {
                gameManager.state = BattleState.NEXTTURN;

                selectedSkillPrefab = null;
                selectedTarget = null;
                enemyTargets = null;

                playerAttacking = false;
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
            foreach (GameObject enemy in enemyTargets)
            {
                enemy.GetComponent<_EnemyAction>().indicator.SetActive(true);
            }

            foreach (GameObject player in playerTargets)
            {
                player.GetComponent<_PlayerAction>().indicator.SetActive(false);
            }
        }
    }

    protected override void SelectTarget()
    {
        if (Input.GetMouseButtonDown(0) && selectedSkillPrefab != null)
        {
            if (selectedSkillPrefab == skill1Prefab || selectedSkillPrefab == skill2Prefab || selectedSkillPrefab == skill3Prefab) //skills that targets enemies
            {
                //raycasting mousePosition
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

                if (hit.collider != null && hit.collider.CompareTag("Enemy"))
                {
                    selectedTarget = hit.collider.gameObject;

                    playerState = PlayerState.ATTACKING;
                }                
            }
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
            selectedSkillPrefab.GetComponent<NormalAttack>().Attack(selectedTarget);
        }
        else if (selectedSkillPrefab == skill2Prefab)
        {
            //aoe target DoT skill
            selectedSkillPrefab.GetComponent<AoeAttack>().Attack(enemyTargets);
        }
        else if (selectedSkillPrefab == skill3Prefab)
        {
            //strong single target DoT skill
        }

        StartCoroutine(EndTurnDelay(0.5f));
    }
}
