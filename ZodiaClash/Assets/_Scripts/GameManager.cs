using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState
{
    NEWROUND, NEXTTURN, PLAYERTURN, ENEMYTURN, WIN, LOSE
}

public class GameManager : MonoBehaviour
{
    public BattleState state;
    
    private List<CharacterStats> charactersList = new List<CharacterStats>();
    private CharacterStats currentCharacter;
    private bool roundInProgress;

    [Header("Turn Management")]
    public int roundCounter;
    public int currentPlayerIndex;
    public string activePlayer;
    public string activeEnemy;

    private void Start()
    {
        state = BattleState.NEWROUND;

        CharacterStats[] characters = FindObjectsOfType<CharacterStats>();
        foreach (CharacterStats chara in characters)
        {
            RegisterCharacter(chara);
        }

        roundCounter = 0;
        roundInProgress = false;
    }

    private void Update()
    {
        if (state == BattleState.NEWROUND)
        {
            if (!roundInProgress)
            {
                //introduce UI for a new round and any other conditions for a new round need to be met
                roundInProgress = true;
                ++roundCounter;

                StartCoroutine(NewRoundDelay(1f)); //delay and switch states
            }
        }
        else if (state == BattleState.NEXTTURN)
        {
            Debug.Log("State: Next Turn");
            //check if battle has ended
            if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 0) //no enemies left
            {
                Debug.Log("State: Win");
                state = BattleState.WIN;
            }
            else if (GameObject.FindGameObjectsWithTag("Player").Length <= 0) //no players left
            {
                Debug.Log("State: Lose");
                state = BattleState.LOSE;
            }
            else
            {
                //continue battle
                if (currentPlayerIndex < charactersList.Count)
                {
                    currentCharacter = charactersList[currentPlayerIndex];
                    if (currentCharacter.tag == "Enemy")
                    {
                        activeEnemy = currentCharacter.gameObject.name;

                        Debug.Log("State: Enemy Turn");
                        Debug.Log("Active Character: " + activeEnemy);
                        state = BattleState.ENEMYTURN;
                    }
                    else if (currentCharacter.tag == "Player")
                    {
                        activePlayer = currentCharacter.gameObject.name;

                        Debug.Log("State: Player Turn");
                        Debug.Log("Active Character: " + activePlayer);
                        state = BattleState.PLAYERTURN;
                    }
                    currentPlayerIndex++;
                }
                else
                {
                    Debug.Log("State: End Round");
                    state = BattleState.NEWROUND;
                    currentPlayerIndex = 0; //reset the index for the next round
                    roundInProgress = false;
                }
            }
        }
    }

    public void RegisterCharacter(CharacterStats character)
    {
        CharacterStats characterToAdd = character.GetComponent<CharacterStats>();
        if (characterToAdd != null)
        {
            charactersList.Add(characterToAdd);
        }
    }
    
    public void DetermineTurnOrder()
    {
        charactersList.Sort((a, b) => b.speed.CompareTo(a.speed));

        foreach (CharacterStats chara in charactersList)
        {
            Debug.Log("Character: " + chara.gameObject.name + ", Speed: " + chara.speed);
        }
    }

    public IEnumerator NewRoundDelay(float seconds)
    {
        DetermineTurnOrder();

        yield return new WaitForSeconds(seconds);
        Debug.Log("New Round Delay");

        state = BattleState.NEXTTURN;
    }
}
