using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleanse : MonoBehaviour
{
    public void RemoveAllStatus(CharacterStats cleanseTarget)
    {
        //dispel all negative effects here
        #region Bleed
        if (cleanseTarget.bleedStack > 0)
        {
            cleanseTarget.bleedStack = 0;
        }
        #endregion

        #region Defense Break
        if (cleanseTarget.shatterCounter > 0)
        {
            cleanseTarget.shatterCounter = 0;
        }
        #endregion

        #region Stun
        if (cleanseTarget.stunCounter > 0)
        {
            cleanseTarget.stunCounter = 0;
            //cleanseTarget.stunCheck = false;
        }
        #endregion

        #region Taunt
        if (cleanseTarget.tauntCounter > 0)
        {
            cleanseTarget.tauntCounter = 0;
        }
        #endregion

        //reset all negative effects icons
        _StatusEffectHud statusEffect = FindObjectOfType<_StatusEffectHud>();
        statusEffect.UpdateEffectsBar(cleanseTarget, "cleanse");
    }
}
