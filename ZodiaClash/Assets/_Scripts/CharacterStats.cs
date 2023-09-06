using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] private Animator animator;
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

    private bool dead = false;
    private float maxChi = 2;

    private void Start()
    {
        health = maxHealth;

        healthFillBattlefield.fillAmount = health / maxHealth;
        if (healthFillSecond != null )
        {
            healthFillSecond.fillAmount = health / maxHealth; //player second hp bar
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
        //animator.Play("Damage");

        //set damage text
        if (health <= 0)
        {
            Debug.Log(gameObject.name + " is dead");
            dead = true;
            gameObject.tag = "Dead";
            healthBarBattlefield.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
