using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    WAITING, CHECKSTATUS, SKILLSELECT, TARGETING, ATTACKING, ENDING
}

public class _EnemyAction : MonoBehaviour
{
    public EnemyState enemyState;

    [Header("HUD")]
    public GameObject indicator;

    [Header("Skill Selection")]
    [SerializeField] protected GameObject skill1Prefab;
    [SerializeField] protected GameObject skill2Prefab;
    [SerializeField] protected GameObject skill3Prefab;
    [SerializeField] protected GameObject selectedSkillPrefab;

    [Header("Target Selection")]
    [SerializeField] protected GameObject[] playerTargets;
    protected GameObject selectedTarget;

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
    protected bool enemyAttacking;

    [Header("Status Effects")]
    [HideInInspector] public int bleedCounts;

    private void Start()
    {
        enemyState = EnemyState.WAITING;

        moveSpeed = 30f;
        startPosition = transform.position;
        movingToTarget = false;
        movingToStart = false;
        reachedTarget = false;
        reachedStart = false;

        gameManager = FindObjectOfType<GameManager>();
        characterStats = GetComponent<CharacterStats>();
        enemyAttacking = false;
    }

    protected void RefreshPlayerTargets()
    {
        playerTargets = GameObject.FindGameObjectsWithTag("Player");
    }

    protected void EnemyMovement()
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

                EnemyAttackAnimation();
            }
        }
        else if (movingToStart && !movingToTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, moveSpeed * Time.deltaTime);

            if (reachedStart)
            {
                movingToStart = false;
            }
        }
    }

    protected virtual void EnemySelectSkill()
    {
        //do nothing
    }

    protected virtual void EnemySelectTarget()
    {
        //check for specific skill prefab, then proceed
    }

    protected virtual void EnemyUseSkill()
    {
        //check if movement is needed, else play EnemyAttackAnimation()
    }

    protected virtual void EnemyAttackAnimation()
    {
        //play different attack animation timings for different skillPrefabs
    }

    protected virtual void EnemyApplySkill()
    {
        //apply actual damage and effects
    }

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
    }

    protected IEnumerator EnemyEndTurnDelay(float seconds)
    {
        //if (!movingToTarget) //check for skills that do not require movement
        //{

        //}
        yield return new WaitUntil(() => reachedStart);
        
        yield return new WaitForSeconds(seconds);

        enemyState = EnemyState.ENDING;
    }
}
