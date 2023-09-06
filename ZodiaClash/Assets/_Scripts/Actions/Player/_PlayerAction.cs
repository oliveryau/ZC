using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _PlayerAction : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField] protected GameObject characterUi;
    [SerializeField] protected Sprite avatar;

    //animations
    [Header("Skill Selection")]
    [SerializeField] protected GameObject skill1Prefab;
    [SerializeField] protected GameObject skill2Prefab;
    [SerializeField] protected GameObject skill3Prefab;
    protected GameObject selectedSkillPrefab;
    protected GameObject selectedTarget;
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
        enemyTargets = GameObject.FindGameObjectsWithTag("Enemy");

        startPosition = transform.position;
        movingToTarget = false;
        movingToStart = false;
        reachedTarget = false;
        reachedStart = false;

        gameManager = FindObjectOfType<GameManager>();
        playerAttacking = false;
        playerTurnComplete = false;
    }

    public void DisplayUi()
    {
        if (!playerAttacking)
        {
            characterUi.SetActive(true);
        }
        else
        {
            characterUi.SetActive(false);
        }
    }

    public void PlayerMovement()
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

                StartCoroutine(AttackDelay(0.5f));
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

    public void SelectSkill(string btn)
    {
        if (gameManager.state != BattleState.PLAYERTURN)
        {
            return;
        }
        else
        {
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
            Debug.Log("Player Skill: " + selectedSkillPrefab.name);
        }
    }

    protected void SelectTarget()
    {
        if (Input.GetMouseButtonDown(0) && selectedSkillPrefab != null)
        {
            //raycasting mousePosition
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
            {
                selectedTarget = hit.collider.gameObject;
                UseSkill();
            }
        }
    }

    public IEnumerator AttackDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        ApplySkill();

        movingToStart = true;
    }

    public IEnumerator EndTurnDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        playerTurnComplete = true;
    }

    public virtual void UseSkill()
    {
        //do nothing
    }

    public virtual void ApplySkill()
    {
        //do nothing
    }
}
