using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [Header("HUD")]
    [SerializeField] private Image healthBarBattlefield;
    [SerializeField] private Image healthFillBattlefield;
    [SerializeField] private Image healthBarSecond;
    [SerializeField] private Image healthFillSecond;

    [Header("Stats")]
    public float maxHealth;
    public float health;
    public float attack;
    public float defense;
    public float speed;

    [Header("Status Effects")]
    public bool checkedStatus;
    public List<int> bleedList;

    //private float maxChi = 2;

    private void Start()
    {
        health = maxHealth;
        healthFillBattlefield.fillAmount = health / maxHealth;
        if (healthFillSecond != null )
        {
            healthFillSecond.fillAmount = health / maxHealth; //player second hp bar
        }

        checkedStatus = false;
    }

    private void Update()
    {
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        healthFillBattlefield.fillAmount = health / maxHealth;
        if (healthFillSecond != null)
        {
            healthFillSecond.fillAmount = health / maxHealth; //player second hp bar
        }

        //set damage text
        if (health <= 0)
        {
            Debug.Log(gameObject.name + " Died");

            if (gameObject.CompareTag("Player"))
            {
                _PlayerAction player = GetComponent<_PlayerAction>();
                player.playerState = PlayerState.ENDING;
            }
            else if (gameObject.CompareTag("Enemy"))
            {
                _EnemyAction enemy = GetComponent<_EnemyAction>();
                enemy.enemyState = EnemyState.ENDING;
            }

            health = 0;
            gameObject.tag = "Dead";

            healthBarBattlefield.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
        else
        {
            //animator.Play("Damaged");
        }
    }

    public void CheckStatusEffects()
    {
        if (bleedList.Count != 0) //bleed status
        {
            for (int i = 0; i < bleedList.Count; i++)
            {
                if (bleedList[i] > 0) //apply bleed
                {
                    A_SingleBleed bleed = FindObjectOfType<A_SingleBleed>();
                    bleed.ApplyBleed(this);
                    --bleedList[i];
                }
            }

            for (int j = 0;  j < bleedList.Count; j++)
            {
                if (bleedList[j] <= 0) //remove bleed status
                {
                    bleedList.RemoveAt(j);
                }
            }
        }

        checkedStatus = true;
    }
}
