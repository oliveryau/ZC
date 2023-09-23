using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerChi : MonoBehaviour
{
    [Header("Chi Amount")]
    public int currentChi;
    public int maxChi;

    [Header("HUD")]
    [SerializeField] private TextMeshProUGUI chiCount;
    [SerializeField] private Image[] chi;
    [SerializeField] private Sprite fullChi;
    [SerializeField] private Sprite emptyChi;

    private void Start()
    {
        currentChi = maxChi;

        chiCount.text = currentChi.ToString();
    }

    private void UpdateChi()
    {
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

        chiCount.text = currentChi.ToString();
    }

    public void UseChi(int amount)
    {
        currentChi -= amount;

        UpdateChi();
    }

    public void RegainChi()
    {
        currentChi += 1;

        if (currentChi > maxChi)
        {
            currentChi = maxChi;
        }

        UpdateChi();
    }
}
