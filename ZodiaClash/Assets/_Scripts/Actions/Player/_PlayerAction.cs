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
    protected GameObject[] enemyTargets; //all enemies

    [Header("Movements")]
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected Vector3 startPosition;
    [SerializeField] protected Transform targetPosition;

    protected GameManager gameManager;
    protected bool playerAttacking;
    protected bool movingToTarget;
    protected bool movingToStart;
    protected bool playerTurnComplete;

    private void Start()
    {
        enemyTargets = GameObject.FindGameObjectsWithTag("Enemy");

        startPosition = transform.position;

        gameManager = FindObjectOfType<GameManager>();
        playerAttacking = false;
        movingToTarget = false;
        movingToStart = false;
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
        if (movingToTarget && !movingToStart)
        {
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, targetPosition.position, step);

            if (Vector3.Distance(transform.position, targetPosition.position) <= 0.01f)
            {
                Debug.Log("Moved to Attack Targets");
                movingToTarget = false;

                ApplySkill();

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
            Debug.Log(selectedSkillPrefab.name);
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

    public IEnumerator AnimationDelay(float seconds)
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
