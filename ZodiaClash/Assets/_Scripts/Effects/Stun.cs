using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : MonoBehaviour
{
    public int stunLimit;

    public void StunCalculation(CharacterStats stunTarget, int stunCount)
    {
        stunTarget.stunCounter += stunCount;

        if (stunTarget.stunCounter > stunLimit)
        {
            stunTarget.stunCounter = stunLimit;
        }
    }
}
