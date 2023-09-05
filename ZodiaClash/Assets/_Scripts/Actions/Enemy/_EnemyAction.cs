using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _EnemyAction : MonoBehaviour
{
    [Header("Skill Selection")]
    [SerializeField] protected GameObject skill1Prefab;
    [SerializeField] protected GameObject skill2Prefab;
    [SerializeField] protected GameObject skill3Prefab;
    [SerializeField]protected GameObject selectedSkillPrefab;
    protected GameObject selectedTarget;

    protected GameManager gameManager;
    protected bool enemyAttacking;
    protected bool enemyTurnComplete;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        enemyAttacking = false;
        enemyTurnComplete = false;
    }

    public void EnemySelectSkill()
    {
        if (selectedSkillPrefab == null)
        {
            selectedSkillPrefab = Random.Range(0, 2) == 0 ? skill1Prefab : skill2Prefab;
            Debug.Log("Skill chosen: " + selectedSkillPrefab.name);
            EnemySelectTarget();
        }
    }

    public void EnemySelectTarget()
    {
        //check if its certain type of enemy via gameManager.activeEnemy first, then run special target selection, else proceed
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length > 0)
        {
            int randomIndex = Random.Range(0, players.Length);
            selectedTarget = players[randomIndex];
            Debug.Log("Selected target: " + selectedTarget.name);

            EnemyUseSkill(selectedTarget);
        }
        else
        {
            //no players found
            Debug.Log("No players found, supposedly return to BattleState.NEXTTURN");
        }
    }

    public IEnumerator EnemyAnimationDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        enemyTurnComplete = true;
    }

    public virtual void EnemyUseSkill(GameObject target)
    {
        //do nothing
    }

}
