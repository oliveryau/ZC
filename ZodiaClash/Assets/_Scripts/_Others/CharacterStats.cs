using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [Header("HUD")]
    public Sprite avatar;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image healthBarFill;
    [SerializeField] private GameObject floatingText;

    [Header("Stats")]
    public float maxHealth;
    public float health;
    public float attack;
    public float defense;
    public int speed;

    [Header("Debuffs")]
    public int bleedStack;
    public int defBreakCounter;
    private float initialDefense;

    [Header("Buffs")]
    public int attackBuffCounter;
    private float initialAttack;

    [Header("Others")]
    [HideInInspector] public bool checkedStatus;

    //private float maxChi = 2;

    private void Start()
    {
        //hp
        health = maxHealth;
        healthBarFill.fillAmount = health / maxHealth;

        //status effects
        initialDefense = defense;
        initialAttack = attack;

        checkedStatus = false;
    }

    private void Update()
    {
        if (health >= maxHealth)
        {
            health = maxHealth;
        }
    }

    private IEnumerator Death()
    {
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

        healthBarFill.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.5f);
        //animator.Play("Death");
        gameObject.SetActive(false);
    }

    public void TakeDamage(float damage, bool critCheck, string debuff)
    {
        health -= damage;

        DamageText(damage, critCheck, debuff);

        //set damage text
        if (health <= 0)
        {
            StartCoroutine(Death());
        }
        else
        {
            //animator.Play("Damaged");
        }

        healthBarFill.fillAmount = health / maxHealth;
    }

    public void HealBuff(float buffAmount)
    {
        health += buffAmount;

        BuffText(buffAmount, "heal");

        if (health >= maxHealth)
        {
            health = maxHealth;
        }

        healthBarFill.fillAmount = health / maxHealth;
    }

    public void AttackBuff(float buffAmount)
    {
        BuffText(buffAmount, "atkBuff");
    }

    #region Text
    public void DamageText(float value, bool critCheck, string effect)
    {
        TextMeshPro popup = floatingText.GetComponent<TextMeshPro>();
        popup.color = Color.red;

        if (critCheck)
        {
            popup.text = "CRITICAL HIT\n\n";
        }

        switch (effect)
        {
            case "bleed":
                popup.text += "BLEED\n" + value.ToString();
                break;
            case "rend":
                popup.text += "REND\n" + value.ToString();
                break;
            case "defBreak":
                popup.text += "DEF DOWN\n" + value.ToString();
                break;
            default:
                popup.text += value.ToString();
                break;
        }

        Instantiate(floatingText, transform.position, Quaternion.identity, transform);

        popup.text = null;
    }

    public void BuffText(float value, string effect)
    {
        TextMeshPro popup = floatingText.GetComponent<TextMeshPro>();
        popup.color = Color.green;

        switch (effect)
        {
            case "heal":
                popup.text = "CLEANSE\n" + value.ToString();
                break;
            case "atkBuff":
                popup.text = "ATK UP\n+" + value.ToString() + "% ATK";
                break;
            default:
                Debug.LogError("No buff text, BUG");
                break;
        }

        Instantiate(floatingText, transform.position, Quaternion.identity, transform);

        popup.text = null;
    }
    #endregion

    public IEnumerator CheckStatusEffects()
    {
        //debuffs
        #region Bleed
        if (bleedStack > 0)
        {
            _Bleed bleed = FindObjectOfType<_Bleed>();
            bleed.BleedCalculation(this);

            TakeDamage(bleed.bleedDamage, false, "bleed");
            --bleedStack;
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

        yield return new WaitForSeconds(0.5f);

        checkedStatus = true;
    }

    public void CheckEndStatusEffects()
    {
        //usually for buffs
        #region Attack Buff
        if (attackBuffCounter > 0)
        {
            --attackBuffCounter;

            if (attackBuffCounter <= 0)
            {
                if (attack != initialAttack)
                {
                    attack = initialAttack;
                }
            }
        }
        #endregion
    }
}
