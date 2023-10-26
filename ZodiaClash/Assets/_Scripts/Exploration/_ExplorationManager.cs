using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _ExplorationManager : MonoBehaviour
{
    public GameObject[] enemyNpcs;

    [Header("Unique Dialogue Objects")]
    public DialogueObject defeatedFirstGuardDialogue;
    public DialogueObject defeatedGoatDialogue;
    public DialogueObject defeatedOxDialogue;

    public void FindAllEnemyNpcs()
    {
        enemyNpcs = GameObject.FindGameObjectsWithTag("Enemy");
    }

    public string GetSpecificNpc(int index)
    {
        switch (index)
        {
            case 2:
                return "Guard Exploration";
            case 3:
                return "Enemy Goat Exploration";
            case 4:
                return "Guard2 Exploration";
        }
        return null;
    }
}
