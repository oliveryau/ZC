using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _PlayerAction : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField] protected GameObject characterUi;
    [SerializeField] protected Sprite avatar;
    [SerializeField] protected GameObject indicator;

    //animations
    [Header("Skill Selection")]
    [SerializeField] protected GameObject skill1Prefab;
    [SerializeField] protected GameObject skill2Prefab;
    [SerializeField] protected GameObject skill3Prefab;
    [SerializeField] protected GameObject selectedSkillPrefab;
    protected GameObject selectedTarget;
    protected bool reinitialiseEnemyTargets;
    [SerializeField] protected GameObject[] enemyTargets; //all enemies

    [Header("Movements")]
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected Vector3 startPosition;
    [SerializeField] protected Transform targetPosition;
    protected bool movingToTarget;
    protected bool movingToStart;
    protected bool reachedTarget;
    protected bool reachedStart;

    protected GameManager gameManager;
    protected bool playerAttacking;
    protected bool playerTurnComplete;

    private void Start()
    {
        reinitialiseEnemyTargets = false;

        startPosition = transform.position;
        movingToTarget = false;
        movingToStart = false;
        reachedTarget = false;
        reachedStart = false;

        gameManager = FindObjectOfType<GameManager>();
        playerAttacking = false;
        playerTurnComplete = false;
    }

    protected void RefreshEnemyTargets()
    {
        //find all enemies that are present again
        reinitialiseEnemyTargets = true;

        enemyTargets = GameObject.FindGameObjectsWithTag("Enemy");
    }

    protected void DisplayUi()
    {
        if (!playerAttacking)
        {
            characterUi.SetActive(true);
        }
        else
        {
            characterUi.SetActive(false);
        }

        if (selectedTarget != null) //after selecting target, hide all player and enemy indicators
        {
            foreach (GameObject enemy in enemyTargets)
            {
                enemy.GetComponent<_EnemyAction>().indicator.SetActive(false);
            }
        }
    }

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
            }
        }
    }

    public virtual void SelectSkill(string btn)
    {
        //show specific target indicators based on skillPrefab
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
        //play different attack animation timings for different skillPrefabs
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
    }

    protected IEnumerator EndTurnDelay(float seconds)
    {
        yield return new WaitUntil(() => reachedStart);
        //check for skills that do not require movement

        yield return new WaitForSeconds(seconds);

        playerTurnComplete = true;
    }
}
