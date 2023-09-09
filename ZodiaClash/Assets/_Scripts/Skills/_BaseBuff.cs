using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _BaseBuff : MonoBehaviour
{
    [Header("Character")]
    [SerializeField] protected GameObject owner;
    [SerializeField] protected string animationName;

    [Header("Integer")]
    public int skillBuffPercent;

    protected CharacterStats ownerStats;
    protected CharacterStats targetStats;

    public virtual void CalculateBuff(GameObject target)
    {
        ownerStats = owner.GetComponent<CharacterStats>();
        targetStats = target.GetComponent<CharacterStats>();
    }
}
