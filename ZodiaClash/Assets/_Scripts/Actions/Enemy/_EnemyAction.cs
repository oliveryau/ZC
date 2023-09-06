using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _EnemyAction : MonoBehaviour
{
    [Header("Skill Selection")]
    [SerializeField] protected GameObject skill1Prefab;
    [SerializeField] protected GameObject skill2Prefab;
    [SerializeField] protected GameObject skill3Prefab;
    protected GameObject selectedSkillPrefab;
    protected GameObject selectedTarget;
    protected GameObject[] playerTargets; //all players

    [Header("Movements")]
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected Vector3 startPosition;
    [SerializeField] protected Transform targetPosition;

    protected GameManager gameManager;
    protected bool enemyAttacking;
    protected bool movingToTarget;
    protected bool movingToStart;
    protected bool reachedTarget;
    protected bool reachedStart;
    protected bool enemyTurnComplete;

    private void Start()
    {
        playerTargets = GameObject.FindGameObjectsWithTag("Player");

        startPosition = transform.position;
        movingToTarget = false;
        movingToStart = false;
        reachedTarget = false;
        reachedStart = false;

        gameManager = FindObjectOfType<GameManager>();
        enemyAttacking = false;
        enemyTurnComplete = false;
    }

    public void EnemyMovement()
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

                StartCoroutine(EnemyAttackDelay(0.5f));
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

    public virtual void EnemySelectSkill()
    {
        //do nothing
    }

    public void EnemySelectTarget()
    {
        //virtual method
        //check if its certain type of enemy via gameManager.activeEnemy first, then run special target selection, else proceed
        if (playerTargets.Length > 0)
        {
            int randomIndex = Random.Range(0, playerTargets.Length);
            selectedTarget = playerTargets[randomIndex];
            Debug.Log("Enemy target: " + selectedTarget.name);

            EnemyUseSkill();
        }
        else
        {
            //no players found
            Debug.LogError("No players found, supposedly return to BattleState.NEXTTURN");
        }
    }

    public IEnumerator EnemyAttackDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        EnemyApplySkill();

        movingToStart = true;
    }

    public IEnumerator EnemyEndTurnDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        enemyTurnComplete = true;
    }

    public virtual void EnemyUseSkill()
    {
        //do nothing
    }

    public virtual void EnemyApplySkill()
    {
        //do nothing
    }
}
