using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterStats : MonoBehaviour
{
    private GameManager gameManager;
    private StatusEffectHud statusEffectHud;
    private Animator animator;

    [Header("HUD")]
    [SerializeField] private GameObject characterHpHud;
    [SerializeField] private Image healthBarFill;
    public Transform statusEffectPanel;

    [Header("Other HUD")]
    [SerializeField] private TextMeshProUGUI hpValueUi;
    [SerializeField] private TextMeshProUGUI characterName;
    public Image healthPanel;
    [SerializeField] private GameObject floatingText;

    [Header("Stats")]
    public float maxHealth;
    public float health;
    public float attack;
    public float defense;
    public int speed;
    private float initialAttack;
    private float initialDefense;

    [Header("Debuffs")]
    public int bleedStack;
    public int shatterCounter;
    public int stunCounter;
    public bool stunCheck;
    public int tauntCounter;
    public bool tauntCheck;

    [Header("Buffs")]
    public int attackBuffCounter;

    [HideInInspector] public bool checkedStatus;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        statusEffectHud = FindObjectOfType<StatusEffectHud>();
        animator = GetComponent<Animator>();

        if (characterName != null) characterName.text = gameObject.name.ToString();

        //hp
        health = maxHealth;

        if (hpValueUi != null) hpValueUi.text = health.ToString();
        healthBarFill.fillAmount = health / maxHealth;

        //status effects
        initialDefense = defense;
        initialAttack = attack;

        stunCheck = false;
        tauntCheck = false;
        checkedStatus = false;
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
        characterHpHud.SetActive(false);
        gameObject.tag = "Dead";
        //animator.Play("Death");

        gameManager.charactersList.Remove(this);
        gameManager.turnOrderList.Remove(this);
        gameManager.UpdateTurnOrderUi();

        yield return new WaitForSeconds(0.5f);

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
            if (animator != null)
            {
                animator.SetTrigger("damaged");
            }
        }

        if (hpValueUi != null) hpValueUi.text = health.ToString();
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

        if (hpValueUi != null) hpValueUi.text = health.ToString();
        healthBarFill.fillAmount = health / maxHealth;
    }

    #region Text Effects
    public void DamageText(float value, bool critCheck, string effect)
    {
        TextMeshPro popup = floatingText.GetComponent<TextMeshPro>();
        popup.color = Color.white;

        if (critCheck)
        {
            popup.text = "CRITICAL HIT\n\n";
            popup.color = Color.red;
        }

        switch (effect)
        {
            case "bleed":
                popup.text += "BLEED\n" + value.ToString();
                break;
            case "rend":
                popup.text += "REND\n" + value.ToString();
                break;
            case "shatter":
                popup.text += "DEF DOWN\n" + value.ToString();
                break;
            case "stun":
                popup.text += "STUN\n" + value.ToString();
                break;
            case "stun0":
                popup.text += "STUN";
                break;
            case "taunt":
                popup.text += "TAUNT";
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
            case "enrage":
                popup.text = "ATK UP\n+" + value.ToString() + "% ATK";
                break;
            case "heal":
                popup.text = "CLEANSE\n" + value.ToString();
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
            Bleed bleed = FindObjectOfType<Bleed>();
            bleed.BleedCalculation(this);

            TakeDamage(bleed.bleedDamage, false, "bleed");
            --bleedStack;

            statusEffectHud.UpdateEffectsBar(this, "bleed");
        }
        #endregion

        #region Defense Break
        if (shatterCounter > 0)
        {
            --shatterCounter;

            statusEffectHud.UpdateEffectsBar(this, "shatter");

            if (shatterCounter <= 0)
            {
                if (defense != initialDefense)
                {
                    defense = initialDefense;
                }
            }
        }
        #endregion

        #region Stun
        if (stunCounter > 0)
        {
            DamageText(0, false, "stun0");
        }
        #endregion

        #region Taunt
        if (tauntCounter > 0)
        {
            DamageText(0, false, "taunt");
        }
        #endregion

        yield return new WaitForSeconds(0.5f);

        checkedStatus = true;
    }

    public void CheckEndStatusEffects()
    {
        //debuffs
        #region Stun
        if (stunCounter > 0)
        {
            --stunCounter;

            statusEffectHud.UpdateEffectsBar(this, "stun");

            if (stunCounter <= 0)
            {
                stunCheck = false;
            }
        }
        #endregion

        #region Taunt
        if (tauntCounter > 0)
        {
            --tauntCounter;

            statusEffectHud.UpdateEffectsBar(this, "taunt");

            if (tauntCounter <= 0)
            {
                tauntCheck = false;
            }
        }
        #endregion

        //buffs
        #region Attack Buff
        if (attackBuffCounter > 0)
        {
            --attackBuffCounter;

            statusEffectHud.UpdateEffectsBar(this, "enrage");

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
