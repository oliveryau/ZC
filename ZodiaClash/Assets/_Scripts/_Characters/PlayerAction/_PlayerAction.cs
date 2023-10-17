using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState
{
    WAITING, CHECKSTATUS, SELECTION, ATTACKING, ENDING, DYING
}

public class _PlayerAction : MonoBehaviour
{
    public PlayerState playerState;

    [Header("HUD")]
    [SerializeField] protected GameObject characterAvatar;
    [SerializeField] protected GameObject characterSkillUi;
    [SerializeField] protected TextMeshProUGUI skill2ChiCostUi;
    [SerializeField] protected TextMeshProUGUI skill3ChiCostUi;
    [SerializeField] protected Button skill2Enable;
    [SerializeField] protected Button skill3Enable;
    [SerializeField] protected GameObject skillTextIndicator;
    public GameObject turnIndicator;
    public GameObject targetIndicator;
    protected int originalSort;

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

    [Header("Movements")]
    [SerializeField] protected Vector3 startPosition;
    [SerializeField] protected Transform targetPosition;
    [SerializeField] protected Transform aoeTargetPosition;
    protected float moveSpeed;
    [SerializeField] protected bool movingToTarget;
    [SerializeField] protected bool movingToStart;
    protected bool reachedTarget;
    protected bool reachedStart;

    protected BattleManager battleManager;
    protected CharacterStats characterStats;
    protected PlayerChi playerChi;
    protected CameraBattle cam;
    protected bool playerAttacking;
    protected bool endingTurn;
    protected bool checkingStatus;

    private void Start()
    {
        playerState = PlayerState.WAITING;

        originalSort = GetComponent<SpriteRenderer>().sortingOrder;

        moveSpeed = 50f;
        startPosition = transform.position;

        battleManager = FindObjectOfType<BattleManager>();
        characterStats = GetComponent<CharacterStats>();
        playerChi = FindObjectOfType<PlayerChi>();
        cam = FindObjectOfType<CameraBattle>();
    }

    #region Player State
    protected void UpdatePlayerState()
    {
        if (battleManager.battleState == BattleState.PLAYERTURN && battleManager.activePlayer == gameObject.name)
        {
            if (playerState == PlayerState.WAITING)
            {
                turnIndicator.SetActive(true);
                characterAvatar.GetComponent<Animator>().SetTrigger("increase");

                #region Taunted Behaviour
                if (characterStats.tauntCounter > 0)
                {
                    //if taunt target is dead
                    if (!selectedTarget.gameObject.activeSelf)
                    {
                        selectedTarget = null;

                        characterStats.tauntCounter = 0;
                    }
                }
                #endregion

                playerState = PlayerState.CHECKSTATUS;
            }

            else if (playerState == PlayerState.CHECKSTATUS)
            {
                if (!characterStats.checkedStatus && !checkingStatus)
                {
                    StartCoroutine(characterStats.CheckStatusEffects());
                    checkingStatus = true;
                }
                else if (characterStats.checkedStatus && characterStats.stunCounter > 0)
                {
                    playerState = PlayerState.ENDING;
                    checkingStatus = false;
                }
                else if (characterStats.checkedStatus)
                {
                    playerState = PlayerState.SELECTION;
                    checkingStatus = false;
                }
            }

            else if (playerState == PlayerState.SELECTION)
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
                    ToggleSkillText(true);

                    UseSkill();
                }

                PlayerMovement();
            }

            else if (playerState == PlayerState.ENDING)
            {
                if (characterStats.health > 0)
                {
                    ToggleSkillText(false);

                    characterStats.CheckEndStatusEffects();
                }

                battleManager.battleState = BattleState.NEXTTURN;

                //hud
                characterAvatar.GetComponent<Animator>().SetTrigger("decrease");
                turnIndicator.SetActive(false);

                //skill
                selectedSkillPrefab = null;
                if (characterStats.tauntCounter <= 0)
                {
                    selectedTarget = null;
                }

                //others
                targetPosition = null;

                playerAttacking = false;
                endingTurn = false;
                characterStats.checkedStatus = false;

                playerState = PlayerState.WAITING;
            }
        }

        if (this.playerState == PlayerState.DYING)
        {
            StartCoroutine(PlayerDeath());
        }
    }
    #endregion

    protected void RefreshTargets()
    {
        playerTargets = GameObject.FindGameObjectsWithTag("Player");
        enemyTargets = GameObject.FindGameObjectsWithTag("Enemy");
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

    protected void PlayerMovement()
    {
        if (!movingToTarget && !movingToStart)
        {
            return;
        }
        else
        {
            #region Set Position
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
            #endregion

            #region Moving to Attack
            if (movingToTarget && !movingToStart)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, moveSpeed * Time.deltaTime);

                if (reachedTarget)
                {
                    GetComponent<SpriteRenderer>().sortingOrder = 10;

                    movingToTarget = false;

                    AttackAnimation();
                }
            }
            #endregion
            #region Moving back to Start
            else if (movingToStart && !movingToTarget)
            {
                transform.position = Vector3.MoveTowards(transform.position, startPosition, moveSpeed * Time.deltaTime);
                characterStats.animator.SetBool("moveBack", true);
                StartCoroutine(cam.ZoomOut());

                GetComponent<SpriteRenderer>().sortingOrder = originalSort;

                if (reachedStart)
                {
                    characterStats.animator.SetBool("moveBack", false);
                    movingToStart = false;

                    endingTurn = true;
                }
            }
            #endregion
        }
    }

    protected virtual void AttackAnimation()
    {
        //customised attack animation timings for different skills
    }

    protected virtual void ApplySkill()
    {
        //apply actual damage and effects
    }

    protected IEnumerator PlayerDeath()
    {
        gameObject.tag = "Dead";
        //animator.SetTrigger("Death");
        
        #region Update Turn Order
        battleManager.UpdateTurnOrderUi("death", characterStats);
        battleManager.charactersList.Remove(characterStats);
        battleManager.turnOrderList.Remove(characterStats);
        battleManager.originalTurnOrderList.Remove(characterStats);
        #endregion

        yield return new WaitForSeconds(1f);

        characterStats.playerAvatar.color = Color.gray;

        playerState = PlayerState.ENDING;

        yield return new WaitForSeconds(0.5f);

        gameObject.SetActive(false);
    }

    #region Delays
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

        StartCoroutine(cam.ZoomOut());
        endingTurn = true;
    }

    protected IEnumerator EndTurnDelay(float seconds)
    {
        yield return new WaitUntil(() => endingTurn);

        yield return new WaitForSeconds(seconds);

        playerState = PlayerState.ENDING;
    }
    #endregion

    #region UI Toggling
    protected void ToggleSkillUi(bool display)
    {
        if (display == true)
        {
            skill2ChiCostUi.text = skill2ChiCost.ToString();
            skill3ChiCostUi.text = skill3ChiCost.ToString();

            if (playerChi.currentChi < skill2ChiCost)
            {
                skill2Enable.interactable = false;
            }

            if (playerChi.currentChi < skill3ChiCost)
            {
                skill3Enable.interactable = false;
            }

            characterSkillUi.SetActive(true);
            battleManager.skillChiHud.SetActive(true);
        }
        else if (display == false)
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

    protected void TargetSelectionUi(bool display, string targets, string effect = null)
    {
        if (display)
        {
            #region Ally Targeting
            if (targets == "ally")
            {
                foreach (GameObject player in playerTargets)
                {
                    player.GetComponent<_PlayerAction>().targetIndicator.SetActive(true);

                    #region Ally Special Targeting
                    if (effect != null)
                    {
                        switch (effect)
                        {
                            case "speedBuff":

                                if (player == this.gameObject)
                                {
                                    player.GetComponent<_PlayerAction>().targetIndicator.SetActive(false);
                                }
                                break;

                            case "taunt":

                                if (player != this.gameObject)
                                {
                                    player.GetComponent<_PlayerAction>().targetIndicator.SetActive(false);
                                }

                                break;

                            default:
                                Debug.LogError("No effects found");
                                break;
                        }
                    }
                    #endregion
                }

                foreach (GameObject enemy in enemyTargets)
                {
                    enemy.GetComponent<_EnemyAction>().targetIndicator.SetActive(false);
                }

            }
            #endregion
            #region Enemy Targeting
            else if (targets == "enemy") //show enemy targeting
            {
                foreach (GameObject enemy in enemyTargets)
                {
                    enemy.GetComponent<_EnemyAction>().targetIndicator.SetActive(true);

                    #region Enemy Special Targeting
                    if (effect != null)
                    {
                        switch (effect)
                        {
                            case "taunt":

                                if (characterStats.tauntCounter > 0)
                                {
                                    if (enemy != selectedTarget)
                                    {
                                        enemy.GetComponent<_EnemyAction>().targetIndicator.SetActive(false);
                                    }
                                }

                                break;

                            default:
                                Debug.LogError("No effects found");
                                break;
                        }

                    }
                    #endregion
                }

                foreach (GameObject player in playerTargets)
                {
                    player.GetComponent<_PlayerAction>().targetIndicator.SetActive(false);
                }
            }
            #endregion
        }
        else if (!display)
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

    protected void ToggleSkillText(bool display)
    {
        TextMeshProUGUI skillText = skillTextIndicator.GetComponentInChildren<TextMeshProUGUI>();

        if (display)
        {
            skillText.text = selectedSkillPrefab.gameObject.name;
            skillTextIndicator.SetActive(true);
        }
        else if (!display)
        {
            skillTextIndicator.SetActive(false);
            skillText.text = null;
        }
    }
    #endregion

    #region Target UI
    public void HighlightTargetIndicator(bool highlight)
    {
        SpriteRenderer targetSelect = targetIndicator.GetComponent<SpriteRenderer>();
        targetSelect.color = highlight ? Color.green : Color.white;
    }
    #endregion
}
