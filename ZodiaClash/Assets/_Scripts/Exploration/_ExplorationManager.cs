using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _ExplorationManager : MonoBehaviour
{
    public GameObject[] enemyNpcs;
    public GameObject guard;
    public GameObject goat;

    public void FindAllEnemyNpcs()
    {
        enemyNpcs = GameObject.FindGameObjectsWithTag("Enemy");
    }

    public string GetSpecificNpc(int index)
    {
        //foreach (GameObject enemy in enemyNpcs)
        //{
        //    switch (index)
        //    {
        //        case 2:
        //            if (enemy.name == "Guard Exploration")
        //            {
        //                return enemy;
        //            }
        //        break;
        //        case 3:
        //            if (enemy.name == "Enemy Goat Exploration")
        //            {
        //                return enemy;
        //            }
        //            break;
        //        default:
        //            Debug.Log("No NPCS found, BUG!");
        //        break;
        //    }
        //}

        switch (index)
        {
            case 2:
                return "Guard Exploration";
            case 3:
                return "Enemy Goat Exploration";
        }
        return null;
    }
}
