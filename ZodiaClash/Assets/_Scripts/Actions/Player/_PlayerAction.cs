using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    WAITING, CHECKSTATUS, PLAYERSELECTION, ATTACKING, ENDING
}

public class _PlayerAction : MonoBehaviour
{
    public PlayerState playerState;

    [Header("HUD")]
    [SerializeField] protected GameObject characterUi;
    [SerializeField] protected Sprite avatar;
    public GameObject indicator;

    //animations
    [Header("Skill Selection")]
    [SerializeField] protected GameObject skill1Prefab;
    [SerializeField] protected GameObject skill2Prefab;
    [SerializeField] protected GameObject skill3Prefab;
    [SerializeField] protected GameObject selectedSkillPrefab;
    protected GameObject selectedTarget;
    [SerializeField] protected GameObject[] playerTargets;
    [SerializeField] protected GameObject[] enemyTargets;

    [Header("Movements")]
    protected float moveSpeed;
    [SerializeField] protected Vector3 startPosition;
    [SerializeField] protected Transform targetPosition;
    protected bool movingToTarget;
    protected bool movingToStart;
    protected bool reachedTarget;
    protected bool reachedStart;

    protected GameManager gameManager;
    protected CharacterStats characterStats;
    protected bool playerAttacking;

    [Header("Status Effects")]
    [HideInInspector] public int bleedCounts;

    private void Start()
    {
        playerState = PlayerState.WAITING;

        moveSpeed = 30f;
        startPosition = transform.position;
        movingToTarget = false;
        movingToStart = false;
        reachedTarget = false;
        reachedStart = false;

        gameManager = FindObjectOfType<GameManager>();
        characterStats = GetComponent<CharacterStats>();
        playerAttacking = false;
    }

    protected void RefreshTargets()
    {
        playerTargets = GameObject.FindGameObjectsWithTag("Player");
        enemyTargets = GameObject.FindGameObjectsWithTag("Enemy");
    }

    protected void ToggleUi(bool value)
    {
        if (value == true)
        {
            characterUi.SetActive(true);
        }
        else if (value == false)
        {
            characterUi.SetActive(false);
        }

        if (selectedTarget != null) //after selecting target, hide all player and enemy indicators
        {
            if (selectedTarget.gameObject.CompareTag("Enemy"))
            {
                foreach (GameObject enemy in enemyTargets)
                {
                    enemy.GetComponent<_EnemyAction>().indicator.SetActive(false);
                }
            }
            else if (selectedTarget.gameObject.CompareTag("Player"))
            {
                foreach (GameObject player in playerTargets)
                {
                    player.GetComponent<_PlayerAction>().indicator.SetActive(false);
                }
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
        //if (!movingToTarget) //check for skills that do not require movement
        //{

        //}
        yield return new WaitUntil(() => reachedStart);

        yield return new WaitForSeconds(seconds);

        playerState = PlayerState.ENDING;
    }
}
