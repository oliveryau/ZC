using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _ExplorationManager : MonoBehaviour
{
    public GameObject directionArrow;
    public GameObject[] enemyNpcs;

    [Header("Unique Dialogue Objects")]
    public DialogueObject defeatedFirstGuardDialogue;
    public DialogueObject defeatedGoatDialogue;
    public DialogueObject defeatedOxDialogue;

    private GameManager gameManager;

    public void FindAllEnemyNpcs()
    {
        gameManager = FindObjectOfType<GameManager>();

        enemyNpcs = GameObject.FindGameObjectsWithTag("Enemy");
    }

    //private void Update()
    //{
    //    if (gameManager.gameState == GameState.PLAY)
    //    {
    //        if (!Input.GetKeyDown(KeyCode.W) || !Input.GetKeyDown(KeyCode.A) || !Input.GetKeyDown(KeyCode.S) || !Input.GetKeyDown(KeyCode.D) ) 
    //        {
    //            directionArrow.SetActive(true);
    //        }
    //        else
    //        {
    //            directionArrow.SetActive(false);
    //        }
    //    }
    //}

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
