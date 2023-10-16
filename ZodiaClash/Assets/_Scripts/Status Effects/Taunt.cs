using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taunt : MonoBehaviour
{
    public int tauntLimit;

    public void AoeTauntCalculation(GameObject tauntStarter, GameObject[] targets, int tauntCount, float tauntChance)
    {
        foreach (GameObject target in targets)
        {
            if (target.gameObject.CompareTag("Player"))
            {
                target.GetComponent<_PlayerAction>().selectedTarget = tauntStarter;
            }
            else if (target.gameObject.CompareTag("Enemy"))
            {
                target.GetComponent<_EnemyAction>().selectedTarget = tauntStarter;
            }

            CharacterStats targetCharaStats = target.GetComponent<CharacterStats>();
            targetCharaStats.tauntCounter += tauntCount;
            if (targetCharaStats.tauntCounter > tauntLimit) //dont overstack taunt
            {
                targetCharaStats.tauntCounter = tauntLimit;
            }
            targetCharaStats.StatusText("taunt");

            _StatusEffectHud statusEffect = FindObjectOfType<_StatusEffectHud>();
            statusEffect.SpawnEffectsBar(targetCharaStats, tauntCount, "taunt");
        }
    }
}
