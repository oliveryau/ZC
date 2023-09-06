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
    protected bool enemyTurnComplete;

    private void Start()
    {
        playerTargets = GameObject.FindGameObjectsWithTag("Player");

        startPosition = transform.position;

        gameManager = FindObjectOfType<GameManager>();
        enemyAttacking = false;
        movingToTarget = false;
        movingToStart = false;
        enemyTurnComplete = false;
    }

    public void EnemyMovement()
    {
        if (movingToTarget && !movingToStart)
        {
            Debug.Log("Moving");
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, targetPosition.position, step);

            if (Vector3.Distance(transform.position, targetPosition.position) <= 0.01f)
            {
                Debug.Log("Moved to Attack Player");
                movingToTarget = false;

                EnemyApplySkill();

                movingToStart = true;
            }
        }
        else if (movingToStart && !movingToTarget)
        {
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, startPosition, step);

            if (Vector3.Distance(transform.position, startPosition) <= 0.01f)
            {
                Debug.Log("Moved to Start Pos");
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
            Debug.Log("Enemy selected target: " + selectedTarget.name);

            EnemyUseSkill();
        }
        else
        {
            //no players found
            Debug.LogError("No players found, supposedly return to BattleState.NEXTTURN");
        }
    }

    public IEnumerator EnemyAnimationDelay(float seconds)
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
