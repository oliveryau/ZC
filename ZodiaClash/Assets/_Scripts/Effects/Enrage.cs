using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enrage : MonoBehaviour
{
    public int enrageLimit;

    public void EnrageCalculation(CharacterStats enrageTarget, int enragePercent) //universal bleed formula
    {
        float enrageValue = (enragePercent / 100f) * enrageTarget.attack;
        enrageTarget.attack += enrageValue;

        enrageTarget.increasedAttackValue = enrageValue;
    }
}
