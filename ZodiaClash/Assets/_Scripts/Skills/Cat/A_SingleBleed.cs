using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_SingleBleed : NormalAttack
{
    [Header("Effects")]
    [SerializeField] private float bleedRate;
    private float bleedDamage;

    public override void Attack(GameObject target)
    {
        base.Attack(target);

        float randomValue = Random.Range(0f, 1f);
        Debug.Log("Bleed Roll: " + randomValue);

        if (randomValue <= bleedRate)
        {
            //apply bleed status effect to character
        }
    }

    public void Bleed()
    {
        //calculate and apply bleed per turn
    }
}
