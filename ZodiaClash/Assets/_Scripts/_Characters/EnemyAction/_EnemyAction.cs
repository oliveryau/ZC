using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum EnemyState
{
    WAITING, CHECKSTATUS, SELECTION, ATTACKING, ENDING, DYING
}

public class _EnemyAction : MonoBehaviour
{
    public EnemyState enemyState;

    [Header("HUD")]
    [SerializeField] protected GameObject skillTextIndicator;
    public GameObject turnIndicator;
    public GameObject targetIndicator;
    [HideInInspector] public bool enemyStartTurn;
    protected int originalSort;

    [Header("Skill Selection")]
    [SerializeField] protected GameObject skill1Prefab;
    [SerializeField] protected GameObject skill2Prefab;
    [SerializeField] protected GameObject skill3Prefab;
    [SerializeField] protected GameObject selectedSkillPrefab;

    [Header("Target Selection")]
    [SerializeField] protected GameObject[] enemyTargets;
    [SerializeField] protected GameObject[] playerTargets;
    public GameObject selectedTarget;

    [Header("Movements")]
    [SerializeField] protected Vector3 startPosition;
    [SerializeField] protected Transform targetPosition;
    [SerializeField] protected Transform aoeTargetPosition;
    protected float moveSpeed;
    protected bool movingToTarget;
    protected bool movingToStart;
    protected bool reachedTarget;
    protected bool reachedStart;

    protected BattleManager battleManager;
    protected CharacterStats characterStats;
    protected Animator animator;
    protected bool enemyAttacking;
    protected bool enemyEndingTurn;
    protected bool checkingStatus;

    private void Start()
    {
        enemyState = EnemyState.WAITING;

        originalSort = GetComponent<SpriteRenderer>().sortingOrder;

        moveSpeed = 50f;
        startPosition = transform.position;

        battleManager = FindObjectOfType<BattleManager>();
        characterStats = GetComponent<CharacterStats>();
        animator = GetComponent<Animator>();
    }

    protected virtual void UpdateEnemyState()
    {
        //unique enemy states
    }

    protected void EnemyRefreshTargets()
    {
        enemyTargets = GameObject.FindGameObjectsWithTag("Enemy");
        playerTargets = GameObject.FindGameObjectsWithTag("Player");
    }

    protected virtual void EnemySelection()
    {
        //select targets first, then select skill based on targets
    }

    protected virtual void EnemyUseSkill()
    {
        //check if movement is needed, else play EnemyAttackAnimation()
    }

    protected void EnemyMovement()
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

            #region Moving to Target
            if (movingToTarget && !movingToStart)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, moveSpeed * Time.deltaTime);

                if (reachedTarget)
                {
                    GetComponent<SpriteRenderer>().sortingOrder = 10;

                    movingToTarget = false;

                    EnemyAttackAnimation();
                }
            }
            #endregion
            #region Moving back to Start
            else if (movingToStart && !movingToTarget)
            {
                transform.position = Vector3.MoveTowards(transform.position, startPosition, moveSpeed * Time.deltaTime);

                GetComponent<SpriteRenderer>().sortingOrder = originalSort;

                if (reachedStart)
                {
                    movingToStart = false;

                    enemyEndingTurn = true;
                }
            }
            #endregion
        }
    }

    protected virtual void EnemyAttackAnimation()
    {
        //play different attack animation timings for different skillPrefabs
    }

    protected virtual void EnemyApplySkill()
    {
        //apply actual damage and effects
    }

    protected virtual IEnumerator EnemyDeath()
    {
        //unique enemy death logic
        yield return null;
    }

    #region Delays
    protected IEnumerator EnemyAttackStartDelay(float startDelay, float endDelay)
    {
        yield return new WaitForSeconds(startDelay); //walk delay before attacking

        EnemyApplySkill();

        yield return new WaitForSeconds(endDelay); //delay to play animation

        movingToStart = true;
    }

    protected IEnumerator EnemyBuffStartDelay(float startDelay, float endDelay)
    {
        yield return new WaitForSeconds(startDelay); //delay before buff

        EnemyApplySkill();

        yield return new WaitForSeconds(endDelay); //delay to play animation

        enemyEndingTurn = true;
    }

    protected IEnumerator EnemyEndTurnDelay(float seconds)
    {
        yield return new WaitUntil(() => enemyEndingTurn);
        
        yield return new WaitForSeconds(seconds);

        enemyState = EnemyState.ENDING;
    }
    #endregion

    #region UI Toggling
    protected void EnemyToggleUi(bool display)
    {
        if (display)
        {
            turnIndicator.SetActive(true);
            characterStats.healthPanel.color = characterStats.healthPanelTargetColor;
        }
        else if (!display)
        {
            turnIndicator.SetActive(false);
            characterStats.healthPanel.color = characterStats.healthPanelOriginalColor;
        }
    }

    protected void EnemyToggleSkillText(bool display)
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
    public void EnemyHighlightTargetIndicator(bool highlight)
    {
        SpriteRenderer targetSelect = targetIndicator.GetComponent<SpriteRenderer>();
        targetSelect.color = highlight ? Color.red : Color.white;
    }
    #endregion
}
