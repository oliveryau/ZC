using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_AoeBleed : AoeAttack
{
    [Header("Effects")]
    [SerializeField] private float bleedRate; //bleed chance
    [SerializeField] private int bleedCount; //number of turns to bleed
    private float bleedDamage; //damage of bleed

    public override void Attack(GameObject[] targets)
    {
        base.Attack(targets);

        //apply bleed status effect to every character
        foreach (GameObject target in targets)
        {
            float randomValue = Random.Range(0f, 1f);
            Debug.Log("Bleed Roll: " + randomValue);

            if (randomValue <= bleedRate)
            {
                Debug.Log("AOE Bleed: " + target.gameObject.name);
                target.GetComponent<CharacterStats>().bleedList.Add(bleedCount);
            }
        }
    }
}
