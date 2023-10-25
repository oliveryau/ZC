using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyState
{
    START, WAITING, CHECKSTATUS, SELECTION, ATTACKING, ENDING, DYING
}

public class _EnemyAction : MonoBehaviour
{
    public EnemyState enemyState;

    [Header("HUD")]
    [SerializeField] protected GameObject skillTextIndicator;
    [SerializeField] protected GameObject warningTextIndicator;
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

    [Header("Camera Positions")]
    [SerializeField] protected Transform enemyTeamCamPoint;
    [SerializeField] protected Transform playerTeamCamPoint;

    protected BattleManager battleManager;
    protected CharacterStats characterStats;
    protected CameraBattle cam;
    protected bool enemyAttacking;
    protected bool enemyEndingTurn;
    protected bool checkingStatus;

    private void Start()
    {
        originalSort = GetComponent<SpriteRenderer>().sortingOrder;

        moveSpeed = 50f;
        startPosition = transform.position;

        battleManager = FindObjectOfType<BattleManager>();
        characterStats = GetComponent<CharacterStats>();
        cam = FindObjectOfType<CameraBattle>();

        #region Enemy Moving In
        Vector3 offset = new(10f, 0, 0);
        transform.position = startPosition + offset;
        #endregion

        enemyState = EnemyState.START;
    }

    protected virtual void UpdateEnemyState()
    {
        //unique enemy states
    }

    protected void EnemyStartMoveIn()
    {
        //move enemy to start position
        bool startPoint = false;
        float startSpeed = 25f;

        transform.position = Vector3.MoveTowards(transform.position, startPosition, startSpeed * Time.deltaTime);

        if (transform.position == startPosition)
        {
            startPoint = true;

            if (startPoint)
            {
                enemyState = EnemyState.WAITING;
                startPoint = false;
            }
        }
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
                //characterStats.animator.SetBool("moveBack", true);

                GetComponent<SpriteRenderer>().sortingOrder = originalSort;

                if (reachedStart)
                {
                    //characterStats.animator.SetBool("moveBack", true);
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
        }
        else if (!display)
        {
            turnIndicator.SetActive(false);
        }
    }

    protected void EnemyToggleSkillText(bool display, string specialCase = null)
    {
        TextMeshProUGUI skillText = skillTextIndicator.GetComponentInChildren<TextMeshProUGUI>();
        Image skillTextImage = skillTextIndicator.GetComponent<Image>();

        if (display && specialCase != null)
        {
            switch (specialCase)
            {
                case "goat":
                    skillText.text = "Wrath of the Cursed Celestial";
                    skillTextImage.color = new Color32(140, 40, 40, 100);
                    skillTextIndicator.SetActive(true);
                    break;
                default:
                    break;
            }
        }
        else if (display)
        {
            skillText.text = selectedSkillPrefab.gameObject.name;
            skillTextImage.color = new Color32(140, 40, 40, 100);
            skillTextIndicator.SetActive(true);
        }
        else if (!display)
        {
            skillTextIndicator.SetActive(false);
            skillText.text = null;
            skillTextImage.color = new Color32(0, 0, 0, 100);
        }
    }

    protected IEnumerator EnemyToggleWarningText(string specialCase)
    {
        TextMeshProUGUI warningText = warningTextIndicator.GetComponentInChildren<TextMeshProUGUI>();

        switch (specialCase)
        {
            case "goat":
                warningText.text = "Celestial Annihilation will be used next!";
                warningTextIndicator.SetActive(true);
                break;
            default:
                break;
        }

        yield return new WaitForSeconds(2f);

        warningTextIndicator.SetActive(false);
        warningText.text = null;
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
