using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image healthFill;

    [Header("Stats")]
    public float maxHealth;
    public float health;
    public float attack;
    public float defense;
    public float speed;

    [HideInInspector] public int nextActTurn;
    private bool dead = false;
    private float maxChi = 2;

    private void Start()
    {
        health = maxHealth;
        healthFill.fillAmount = health / maxHealth;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthFill.fillAmount = health / maxHealth;
        //animator.Play("Damage");

        //set damage text
        if (health <= 0)
        {
            Debug.Log("Character is now dead");
            dead = true;
            gameObject.tag = "Dead";
            healthBar.gameObject.SetActive(false);
        }
    }
}
