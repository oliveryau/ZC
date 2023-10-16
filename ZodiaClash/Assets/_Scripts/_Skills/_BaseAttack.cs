using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _BaseAttack : MonoBehaviour
{
    [Header("Character")]
    [SerializeField] protected GameObject owner;

    [Header("Integer")]
    [SerializeField] protected int skillAttackPercent;

    [Header("Decimal")]
    [SerializeField] protected float minAttackMultiplier;
    [SerializeField] protected float maxAttackMultiplier;
    [SerializeField] protected float critRate;
    [SerializeField] protected float critMultiplier;

    protected CharacterStats attackerStats;
    protected CharacterStats targetStats;
    protected float damage;

    [SerializeField] protected bool critCheck;

    private void Start()
    {
        critCheck = false;
    }

    protected void CalculateDamage(GameObject target)
    {
        attackerStats = owner.GetComponent<CharacterStats>();
        targetStats = target.GetComponent<CharacterStats>();

        //critical hit chance
        float randomValue = Random.Range(0f, 1f);

        if (randomValue <= critRate)
        {
            //critical hit
            critCheck = true;

            damage = Mathf.RoundToInt(
                Mathf.Max(minAttackMultiplier, maxAttackMultiplier) *
                (skillAttackPercent / 100f) * (attackerStats.attack * (100f / (100f + targetStats.defense)))
                * critMultiplier);
        }
        else
        {
            damage = Mathf.RoundToInt(
                Mathf.Max(minAttackMultiplier, maxAttackMultiplier) * 
                (skillAttackPercent / 100f) * (attackerStats.attack * (100f / (100f + targetStats.defense))));
        }
    }
}
