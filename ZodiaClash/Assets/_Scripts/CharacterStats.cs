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
    public int bleedCounts;

    //private float maxChi = 2;
    private bool dead = false;

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

    public IEnumerator CheckStatusEffects()
    {
        if (bleedCounts > 0)
        {
            A_SingleBleed bleed = FindObjectOfType<A_SingleBleed>();
            bleed.ApplyBleed(this);
            --bleedCounts;
        }

        yield return new WaitForSeconds(1f); //delay after apply effects

        checkedStatus = true;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        healthFillBattlefield.fillAmount = health / maxHealth;
        if (healthFillSecond != null)
        {
            healthFillSecond.fillAmount = health / maxHealth; //player second hp bar
        }
        //animator.Play("Damaged");

        //set damage text
        if (health <= 0)
        {
            Debug.Log(gameObject.name + " is dead");

            health = 0;
            dead = true;
            gameObject.tag = "Dead";

            healthBarBattlefield.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
