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

    //[SerializeField] private bool magicAttack;
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
        Debug.Log("Random Value: " + randomValue);
        if (randomValue <= critRate)
        {
            totalBuff = (skillAttackPercent + extraAttackPercent) / 100;
            damage = Mathf.Max(0, Random.Range(minAttackMultiplier, maxAttackMultiplier) *
                totalBuff * (attackerStats.attack - targetStats.defense)) * critMultiplier;
        }
        else
        {
            totalBuff = (skillAttackPercent + extraAttackPercent) / 100;
            damage = Mathf.Max(0, Random.Range(minAttackMultiplier, maxAttackMultiplier) *
                totalBuff * (attackerStats.attack - targetStats.defense));
        }
        
        Debug.Log("Damage: " + damage);
    }
}
