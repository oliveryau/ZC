using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _CharacterAction : MonoBehaviour
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

    protected GameManager gameManager;
    public bool playerAttacking;
    protected bool playerTurnComplete;

    private void Start()
    {
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
                UseSkill(hit.collider.gameObject); //use the selected skill on the enemy
            }
        }
    }

    public IEnumerator AnimationDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        playerTurnComplete = true;
    }

    public virtual void UseSkill(GameObject target)
    {
        //do nothing
    }
}
