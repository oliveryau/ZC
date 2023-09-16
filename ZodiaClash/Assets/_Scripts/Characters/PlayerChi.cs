using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerChi : MonoBehaviour
{
    public int currentChi;
    public int maxChi;

    public Image[] chi;
    public Sprite fullChi;
    public Sprite emptyChi;

    private void Start()
    {
        currentChi = maxChi;
    }

    public void UseChi(int amount)
    {
        currentChi -= amount;

        for (int i = 0; i < chi.Length; i++)
        {
            if (i < currentChi)
            {
                chi[i].sprite = fullChi;
            }
            else
            {
                chi[i].sprite = emptyChi;
            }

            if (i < maxChi)
            {
                chi[i].enabled = true;
            }
            else
            {
                chi[i].enabled = false;
            }
        }
    }

    public void RegainChi()
    {
        currentChi += 1;

        if (currentChi > maxChi)
        {
            currentChi = maxChi;
        }
    }
}
