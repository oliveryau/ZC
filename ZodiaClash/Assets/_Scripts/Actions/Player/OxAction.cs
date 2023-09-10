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
                turnIndicator.SetActive(true);

                playerState = PlayerState.CHECKSTATUS;
            }

            else if (playerState == PlayerState.CHECKSTATUS)
            {
                if (!characterStats.checkedStatus)
                {
                    StartCoroutine(characterStats.CheckStatusEffects());
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
                characterStats.CheckEndStatusEffects();

                gameManager.state = BattleState.NEXTTURN;

                turnIndicator.SetActive(false);

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

        if (selectedSkillPrefab == skill1Prefab || selectedSkillPrefab == skill3Prefab)
        {
            foreach (GameObject enemy in enemyTargets)
            {
                enemy.GetComponent<_EnemyAction>().targetIndicator.SetActive(true);
            }

            foreach (GameObject player in playerTargets)
            {
                player.GetComponent<_PlayerAction>().targetIndicator.SetActive(false);
            }
        }
        else if (selectedSkillPrefab == skill2Prefab)
        {
            foreach (GameObject player in playerTargets)
            {
                player.GetComponent<_PlayerAction>().targetIndicator.SetActive(true);
            }

            foreach (GameObject enemy in enemyTargets)
            {
                enemy.GetComponent<_EnemyAction>().targetIndicator.SetActive(false);
            }
        }
    }

    protected override void SelectTarget()
    {
        if (Input.GetMouseButtonDown(0) && selectedSkillPrefab != null)
        {
            //raycasting mousePosition
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (selectedSkillPrefab == skill1Prefab) //skills that targets enemies
            {

                if (hit.collider != null && hit.collider.CompareTag("Enemy"))
                {
                    selectedTarget = hit.collider.gameObject;

                    playerState = PlayerState.ATTACKING;
                }
            }
            else if (selectedSkillPrefab == skill2Prefab) //skills that targets allies
            {
                if (hit.collider != null && hit.collider.CompareTag("Player"))
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
            StartCoroutine(AttackStartDelay(0.5f, 1f));
        }
        //else if (selectedSkillPrefab == skill2Prefab)
        //{
        //    StartCoroutine(AttackStartDelay(0.5f, 1f));
        //}
        //else if (selectedSkillPrefab == skill3Prefab)
        //{
        //    StartCoroutine(AttackStartDelay(0.5f, 1f));
        //}
    }

    protected override void ApplySkill()
    {
        if (selectedSkillPrefab == skill1Prefab)
        {
            //single target stun skill
            selectedSkillPrefab.GetComponent<NormalAttack>().Attack(selectedTarget);
        }
        else if (selectedSkillPrefab == skill2Prefab)
        {
            //ally shield buff skill
        }
        else if (selectedSkillPrefab == skill3Prefab)
        {
            //single target taunt skill
        }

        StartCoroutine(EndTurnDelay(1f));
    }
}
