using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enrage : MonoBehaviour
{
    public int enrageLimit;

    public void EnrageCalculation(CharacterStats enrageTarget, int enragePercent) //universal bleed formula
    {
        enrageTarget.attack *= (enragePercent / 100f) + 1;
    }
}
