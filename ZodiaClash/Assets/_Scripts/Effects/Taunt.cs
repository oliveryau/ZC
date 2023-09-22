using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Taunt : MonoBehaviour
{
    [SerializeField] private int tauntLimit;

    public void AoeTauntCalculation(GameObject tauntStarter, GameObject[] targets, int tauntCount, float tauntChance)
    {
        int randomIndex = Random.Range(0, targets.Length);
        CharacterStats mainTauntTarget = targets[randomIndex].GetComponent<CharacterStats>(); //select a random target to taunt

        if (mainTauntTarget.gameObject.CompareTag("Player"))
        {
            mainTauntTarget.GetComponent<_PlayerAction>().selectedTarget = tauntStarter;
        }
        else if (mainTauntTarget.gameObject.CompareTag("Enemy"))
        {
            mainTauntTarget.GetComponent<_EnemyAction>().selectedTarget = tauntStarter;
        }

        mainTauntTarget.tauntCounter += tauntCount;
        if (mainTauntTarget.tauntCounter > tauntLimit)
        {
            mainTauntTarget.tauntCounter = tauntLimit;
        }

        mainTauntTarget.DamageText(0, false, "taunt");

        _StatusEffectHud statusEffect = FindObjectOfType<_StatusEffectHud>();
        statusEffect.SpawnEffectsBar(mainTauntTarget.GetComponent<CharacterStats>(), tauntCount, "taunt");

        for (int i = 0; i < targets.Length; i++) //trigger a chance to taunt another target
        {
            float randomValue = Random.Range(0f, 1f);

            if (i == randomIndex)
            {
                continue;
            }
            else if (i != randomIndex) //check if it is not the mainTauntTarget
            {
                if (randomValue <= tauntChance)
                {
                    CharacterStats otherTarget = targets[i].GetComponent<CharacterStats>();

                    if (otherTarget.gameObject.CompareTag("Player"))
                    {
                        otherTarget.GetComponent<_PlayerAction>().selectedTarget = tauntStarter;
                    }
                    else if (otherTarget.gameObject.CompareTag("Enemy"))
                    {
                        otherTarget.GetComponent<_EnemyAction>().selectedTarget = tauntStarter;
                    }

                    otherTarget.tauntCounter += tauntCount;
                    if (otherTarget.tauntCounter > tauntLimit)
                    {
                        otherTarget.tauntCounter = tauntLimit;
                    }

                    otherTarget.DamageText(0, false, "taunt");

                    statusEffect.SpawnEffectsBar(otherTarget.GetComponent<CharacterStats>(), tauntCount, "taunt");

                    break; //only 2 targets maximum
                }
            }
        }
    }
}
