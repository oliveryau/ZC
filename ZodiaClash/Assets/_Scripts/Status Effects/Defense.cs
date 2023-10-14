using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defense : MonoBehaviour
{
    public int shatterLimit;
    public int armorLimit;

    public void ShatterCalculation(CharacterStats shatterTarget, int shatterPercent) //universal shatter formula
    {
        float shatterValue = (1 - (shatterPercent / 100f)) * shatterTarget.defense;
        shatterTarget.defense -= shatterValue;

        shatterTarget.decreasedDefenseValue = shatterValue;
    }

    public void ArmorCalculation(CharacterStats armorTarget, int armorPercent)
    {
        float armorValue = (1 - (armorPercent / 100f)) * armorTarget.defense;
        armorTarget.defense += armorValue;

        armorTarget.increasedDefenseValue = armorValue;
    }
}
