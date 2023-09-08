using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _BaseAttack : MonoBehaviour
{
    [Header("Character")]
    public GameObject owner;
    [SerializeField] protected string animationName;

    [Header("Integer")]
    [SerializeField] protected float skillAttackPercent;
    [SerializeField] protected float extraAttackPercent;

    [Header("Decimal")]
    [SerializeField] protected float minAttackMultiplier;
    [SerializeField] protected float maxAttackMultiplier;
    [SerializeField] protected float critRate;
    [SerializeField] protected float critMultiplier;

    //[SerializeField] private float magicCost; //character chi

    protected CharacterStats attackerStats;
    protected CharacterStats targetStats;
    protected float totalBuff;
    protected float damage;

    public void CalculateDamage(GameObject target)
    {
        attackerStats = owner.GetComponent<CharacterStats>();
        targetStats = target.GetComponent<CharacterStats>();

        //critical hit chance
        float randomValue = Random.Range(0f, 1f);
        Debug.Log("Crit Roll: " + randomValue);

        totalBuff = (skillAttackPercent + extraAttackPercent) / 100;

        if (randomValue <= critRate)
        {
            //critical hit
            Debug.Log("Critical Hit");
            damage = Mathf.RoundToInt(
                Mathf.Max(1, minAttackMultiplier, maxAttackMultiplier) *
                totalBuff * (attackerStats.attack * (100f / (100f + targetStats.defense)))
                * critMultiplier);
        }
        else
        {
            damage = Mathf.RoundToInt(
                Mathf.Max(1, minAttackMultiplier, maxAttackMultiplier) * 
                totalBuff * (attackerStats.attack * (100f / (100f + targetStats.defense))));
        }
        
        Debug.Log("Damage: " + damage);
    }
}
