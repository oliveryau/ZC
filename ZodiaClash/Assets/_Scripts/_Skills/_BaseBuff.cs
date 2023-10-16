using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _BaseBuff : MonoBehaviour
{
    [Header("Character")]
    [SerializeField] protected GameObject owner;

    [Header("Integer")]
    public int skillBuffPercent;

    protected CharacterStats ownerStats;
    protected CharacterStats targetStats;

    public void GetTargets(GameObject target)
    {
        ownerStats = owner.GetComponent<CharacterStats>();
        targetStats = target.GetComponent<CharacterStats>();
    }
}
