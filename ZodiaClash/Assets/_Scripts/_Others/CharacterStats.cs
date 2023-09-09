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
    public int speed;

    [Header("Debuffs")]
    public List<int> bleedStack;
    public int defBreakCounter;
    private float initialDefense;

    [Header("Buffs")]
    public int attackBuffCounter;
    private float initialAttack;

    [HideInInspector] public bool checkedStatus;

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
        initialDefense = defense;
        initialAttack = attack;
    }

    private void Update()
    {
        if (health >= maxHealth)
        {
            health = maxHealth;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

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

            //add delay, play death animation, then gameObject.SetActive(false)
            gameObject.SetActive(false);
        }
        else
        {
            //animator.Play("Damaged");
        }

        healthFillBattlefield.fillAmount = health / maxHealth;
        if (healthFillSecond != null)
        {
            healthFillSecond.fillAmount = health / maxHealth; //player second hp bar
        }
    }

    public void HealBuff(float buff)
    {
        health += buff;

        if (health >= maxHealth)
        {
            health = maxHealth;
        }

        healthFillBattlefield.fillAmount = health / maxHealth;
        if (healthFillSecond != null)
        {
            healthFillSecond.fillAmount = health / maxHealth; //player second hp bar
        }

    }

    public void CheckStatusEffects()
    {
        //debuffs
        #region Bleed
        if (bleedStack.Count > 0)
        {
            for (int i = 0; i < bleedStack.Count; i++)
            {
                if (bleedStack[i] > 0) //apply bleed if there is bleed stack
                {
                    A_SingleBleed bleed = FindObjectOfType<A_SingleBleed>();
                    bleed.ApplyBleed(this);
                    --bleedStack[i];
                }
            }

            for (int j = 0;  j < bleedStack.Count; j++)
            {
                if (bleedStack[j] <= 0) //remove bleed status
                {
                    bleedStack.RemoveAt(j);
                }
            }
        }
        #endregion

        #region Defense Break
        if (defBreakCounter > 0)
        {
            --defBreakCounter;
        }
        else if (defBreakCounter <= 0)
        {
            if (defense != initialDefense)
            {
                defense = initialDefense;
            }
        }
        #endregion

        //buffs
        #region Attack Buff
        if (attackBuffCounter > 0)
        {
            --attackBuffCounter;
        }
        #endregion

        checkedStatus = true;
    }

    public void CheckEndStatusEffects()
    {
        //usually for buffs
        #region Attack Buff
        if (attackBuffCounter <= 0)
        {
            if (attack != initialAttack)
            {
                attack = initialAttack;
            }
        }
        #endregion
    }
}
