using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoatAction : _PlayerAction
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

                    TargetSelectionUi(false, null);
                }
            }
            else if (selectedSkillPrefab == skill2Prefab || selectedSkillPrefab == skill3Prefab) //skills that targets allies
            {
                if (hit.collider != null && hit.collider.CompareTag("Player"))
                {
                    selectedTarget = hit.collider.gameObject;

                    playerState = PlayerState.ATTACKING;

                    TargetSelectionUi(false, null);
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
        else if (selectedSkillPrefab == skill2Prefab || selectedSkillPrefab == skill3Prefab)
        {
            AttackAnimation();
        }
    }

    protected override void AttackAnimation()
    {
        if (selectedSkillPrefab == skill1Prefab)
        {
            StartCoroutine(AttackStartDelay(0.5f, 1f));
        }
        else if (selectedSkillPrefab == skill2Prefab || selectedSkillPrefab == skill3Prefab)
        {
            StartCoroutine(BuffStartDelay(0.5f, 1f));
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
