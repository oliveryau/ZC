using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState
{
    WAITING, CHECKSTATUS, PLAYERSELECTION, ATTACKING, ENDING
}

public class _PlayerAction : MonoBehaviour
{
    public PlayerState playerState;

    [Header("HUD")]
    [SerializeField] protected GameObject characterSkillUi;
    [SerializeField] protected Button skill2Enable;
    [SerializeField] protected Button skill3Enable;
    public GameObject turnIndicator;
    public GameObject targetIndicator;

    //animations
    [Header("Skill Selection")]
    [SerializeField] protected GameObject skill1Prefab;
    [SerializeField] protected GameObject skill2Prefab;
    [SerializeField] protected GameObject skill3Prefab;
    [SerializeField] protected GameObject selectedSkillPrefab;

    [Header("Skill Chi Cost")]
    [SerializeField] protected int skill2ChiCost;
    [SerializeField] protected int skill3ChiCost;

    [Header("Target Selection")]
    [SerializeField] protected GameObject[] playerTargets;
    [SerializeField] protected GameObject[] enemyTargets;
    public GameObject selectedTarget;
    protected bool aoeSkillSelected;

    [Header("Movements")]
    [SerializeField] protected Vector3 startPosition;
    [SerializeField] protected Transform targetPosition;
    protected float moveSpeed;
    protected bool movingToTarget;
    protected bool movingToStart;
    protected bool reachedTarget;
    protected bool reachedStart;

    protected GameManager gameManager;
    protected CharacterStats characterStats;
    protected PlayerChi playerChi;
    protected bool playerAttacking;
    protected bool endingTurn;
    protected bool checkingStatus;

    private void Start()
    {
        playerState = PlayerState.WAITING;

        moveSpeed = 60f;
        startPosition = transform.position;
        movingToTarget = false;
        movingToStart = false;
        reachedTarget = false;
        reachedStart = false;

        gameManager = FindObjectOfType<GameManager>();
        characterStats = GetComponent<CharacterStats>();
        playerChi = FindObjectOfType<PlayerChi>();
        playerAttacking = false;
        endingTurn = false;
        checkingStatus = false;
    }

    protected void UpdatePlayerState()
    {
        if (gameManager.state == BattleState.PLAYERTURN && gameManager.activePlayer == gameObject.name)
        {
            if (playerState == PlayerState.WAITING)
            {
                turnIndicator.SetActive(true);

                //taunt
                if (characterStats.tauntCheck)
                {
                    //if taunt target is dead
                    if (!selectedTarget.gameObject.activeSelf)
                    {
                        selectedTarget = null;

                        characterStats.tauntCounter = 0;
                        characterStats.tauntCheck = false;
                    }
                }

                playerState = PlayerState.CHECKSTATUS;
            }

            else if (playerState == PlayerState.CHECKSTATUS)
            {
                if (!characterStats.checkedStatus && !checkingStatus)
                {
                    StartCoroutine(characterStats.CheckStatusEffects());
                    checkingStatus = true;
                }
                else if (characterStats.checkedStatus && characterStats.stunCheck)
                {
                    playerState = PlayerState.ENDING;
                    checkingStatus = false;
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

                //hud
                turnIndicator.SetActive(false);

                //skill
                selectedSkillPrefab = null;
                if (!characterStats.tauntCheck)
                {
                    selectedTarget = null;
                }

                //target
                enemyTargets = null;

                //others
                playerAttacking = false;
                endingTurn = false;
                characterStats.checkedStatus = false;

                playerState = PlayerState.WAITING;
            }
        }
    }

    protected void RefreshTargets()
    {
        playerTargets = GameObject.FindGameObjectsWithTag("Player");
        enemyTargets = GameObject.FindGameObjectsWithTag("Enemy");
    }

    #region UI Toggling
    protected void ToggleSkillUi(bool value)
    {
        if (value == true)
        {
            characterSkillUi.SetActive(true);

            if (playerChi.currentChi < skill2ChiCost)
            {
                skill2Enable.interactable = false;
            }

            if (playerChi.currentChi < skill3ChiCost)
            {
                skill3Enable.interactable = false;
            }
        }
        else if (value == false)
        {
            SkillButtons[] skillButtons = FindObjectsOfType<SkillButtons>();
            foreach (SkillButtons skillButton in skillButtons)
            {
                skillButton.ResetSkillColor();
            }

            characterSkillUi.SetActive(false);

            skill2Enable.interactable = true;
            skill3Enable.interactable = true;
        }
    }

    protected void TargetSelectionUi(bool value, string targets)
    {
        if (value)
        {
            if (targets == "ally") //show ally targeting
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
            else if (targets == "enemy") //show enemy targeting
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
        }
        else if (!value)
        {
            //hide all targeting
            foreach (GameObject enemy in enemyTargets)
            {
                enemy.GetComponent<_EnemyAction>().targetIndicator.SetActive(false);
            }

            foreach (GameObject player in playerTargets)
            {
                player.GetComponent<_PlayerAction>().targetIndicator.SetActive(false);
            }
        }
    }
    #endregion

    protected void PlayerMovement()
    {
        if (transform.position == startPosition)
        {
            reachedStart = true;
            reachedTarget = false;
        }
        
        if (transform.position == targetPosition.position)
        {
            reachedTarget = true;
            reachedStart = false;
        }

        if (movingToTarget && !movingToStart)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, moveSpeed * Time.deltaTime);

            if (reachedTarget)
            {
                movingToTarget = false;

                AttackAnimation();
            }
        }
        else if (movingToStart && !movingToTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, moveSpeed * Time.deltaTime);

            if (reachedStart)
            {
                movingToStart = false;

                endingTurn = true;
            }
        }
    }

    public virtual void SelectSkill(string btn)
    {
        //show specific target ui for specific skillPrefabs in override methods

        if (btn.CompareTo("skill1") == 0)
        {
            selectedSkillPrefab = skill1Prefab;
        }
        else if (btn.CompareTo("skill2") == 0)
        {
            selectedSkillPrefab = skill2Prefab;
        }
        else if (btn.CompareTo("skill3") == 0)
        {
            selectedSkillPrefab = skill3Prefab;
        }

        Debug.Log("Player Skill Chosen: " + selectedSkillPrefab.name);
    }

    protected virtual void SelectTarget()
    {
        //check for specific skillPrefab, then userInput
    }

    protected virtual void UseSkill()
    {
        //check if movement is needed, else play AttackAnimation()
    }

    protected virtual void AttackAnimation()
    {
        //customised attack animation timings for different skills
    }

    protected virtual void ApplySkill()
    {
        //apply actual damage and effects
    }

    protected IEnumerator AttackStartDelay(float startDelay, float endDelay)
    {
        yield return new WaitForSeconds(startDelay); //delay before attacking

        ApplySkill();

        yield return new WaitForSeconds(endDelay); //delay to play animation

        movingToStart = true;
    }

    protected IEnumerator BuffStartDelay(float startDelay, float endDelay)
    {
        yield return new WaitForSeconds(startDelay); //delay before buff

        ApplySkill();

        yield return new WaitForSeconds(endDelay); //delay to play animation

        endingTurn = true;
    }

    protected IEnumerator EndTurnDelay(float seconds)
    {
        yield return new WaitUntil(() => endingTurn);

        yield return new WaitForSeconds(seconds);

        playerState = PlayerState.ENDING;
    }

    #region Target UI
    public void HighlightTargetIndicator(bool highlight)
    {
        SpriteRenderer targetSelect = targetIndicator.GetComponent<SpriteRenderer>();
        targetSelect.color = highlight ? Color.cyan : Color.black;
    }
    #endregion
}
