using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shatter : MonoBehaviour
{
    public int shatterLimit;

    public void ShatterCalculation(CharacterStats shatterTarget, int shatterPercent) //universal shatter formula
    {
        shatterTarget.defense *= 1 - (shatterPercent / 100f);
    }
}
