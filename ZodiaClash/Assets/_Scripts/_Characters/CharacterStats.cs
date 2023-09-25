using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterStats : MonoBehaviour
{
    private BattleManager battleManager;
    private _StatusEffectHud statusEffectHud;
    private Animator animator;

    [Header("HUD")]
    [SerializeField] private GameObject characterHpHud;
    [SerializeField] private Image healthBarFill;
    public Transform statusEffectPanel;

    [Header("Other HUD")]
    public Sprite uniqueCharacterAvatar;
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

    [HideInInspector] public float increasedAttackValue;
    [HideInInspector] public float decreasedDefenseValue;
    [HideInInspector] public float increasedDefenseValue;

    [Header("Debuffs")]
    public int bleedStack;
    public int shatterCounter;
    public int stunCounter;
    public int tauntCounter;

    [Header("Buffs")]
    public bool speedCheck;
    public int enrageCounter;
    public int armorCounter;

    [HideInInspector] public bool checkedStatus;

    private void Start()
    {
        battleManager = FindObjectOfType<BattleManager>();
        statusEffectHud = FindObjectOfType<_StatusEffectHud>();
        animator = GetComponent<Animator>();

        //hp
        health = maxHealth;
        healthBarFill.fillAmount = health / maxHealth;

        if (characterName != null) characterName.text = gameObject.name.ToString();
        if (hpValueUi != null) hpValueUi.text = health.ToString();

        //status effect
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

        battleManager.charactersList.Remove(this);
        battleManager.turnOrderList.Remove(this);
        battleManager.originalTurnOrderList.Remove(this);
        battleManager.UpdateTurnOrderUi();

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

    public void HealBuff(float buffAmount, bool cleanse)
    {
        health += buffAmount;

        if (cleanse)
        {
            BuffText(buffAmount, "cleanse");
        }
        else
        {
            BuffText(buffAmount, "heal");
        }

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
            case "shatter":
                popup.text += "SHATTER\n" + value.ToString();
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
                popup.text = "ENRAGE";
                break;
            case "armor":
                popup.text = "ARMOR";
                break;
            case "cleanse":
                popup.text = "CLEANSE\n" + value.ToString();
                break;
            case "heal":
                popup.text = "HEAL\n" + value.ToString();
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

        #region Shatter
        if (shatterCounter > 0)
        {
            --shatterCounter;

            statusEffectHud.UpdateEffectsBar(this, "shatter");

            if (shatterCounter <= 0)
            {
                defense += decreasedDefenseValue; //add back decreased value
                decreasedDefenseValue = 0;
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

        //buffs
        #region Armor
        if (armorCounter > 0)
        {
            --armorCounter;

            statusEffectHud.UpdateEffectsBar(this, "armor");

            if (armorCounter <= 0)
            {
                defense -= increasedDefenseValue; //minus back increased value
                increasedDefenseValue = 0;
            }
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
        }
        #endregion

        #region Taunt
        if (tauntCounter > 0)
        {
            --tauntCounter;

            statusEffectHud.UpdateEffectsBar(this, "taunt");
        }
        #endregion

        //buffs
        #region Speed Buff
        if (speedCheck)
        {
            battleManager.revertingTurn = true;
            speedCheck = false;
        }
        #endregion

        #region Enrage
        if (enrageCounter > 0)
        {
            --enrageCounter;

            statusEffectHud.UpdateEffectsBar(this, "enrage");

            if (enrageCounter <= 0)
            {
                attack -= increasedAttackValue;
                increasedAttackValue = 0;
            }
        }
        #endregion
    }
}
