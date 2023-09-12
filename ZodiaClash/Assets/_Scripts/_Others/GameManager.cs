using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [Header("HUD")]
    [SerializeField] private GameObject playerHud;
    [SerializeField] private GameObject enemyHud;
    [SerializeField] private TextMeshProUGUI turnOrder;

    [Header("Turn Management")]
    public int roundCounter;
    public int currentCharacterIndex;
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

        playerHud.SetActive(false);
        enemyHud.SetActive(false);
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
            Debug.LogWarning("State: Next Turn");

            //ManageTurnOrder();

            //check if battle has ended
            if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 0) //no enemies left
            {
                Debug.LogWarning("State: Win");
                state = BattleState.WIN;

                playerHud.SetActive(false);
                enemyHud.SetActive(false);
            }
            else if (GameObject.FindGameObjectsWithTag("Player").Length <= 0) //no players left
            {
                Debug.LogWarning("State: Lose");
                state = BattleState.LOSE;

                playerHud.SetActive(false);
                enemyHud.SetActive(false);
            }
            else
            {
                //continue battle
                if (currentCharacterIndex < charactersList.Count)
                {
                    currentCharacter = charactersList[currentCharacterIndex];
                    if (currentCharacter.tag == "Enemy")
                    {
                        activeEnemy = currentCharacter.gameObject.name;

                        Debug.LogWarning("State: Enemy Turn");
                        state = BattleState.ENEMYTURN;
                    }
                    else if (currentCharacter.tag == "Player")
                    {
                        activePlayer = currentCharacter.gameObject.name;

                        Debug.LogWarning("State: Player Turn");
                        state = BattleState.PLAYERTURN;
                    }
                    currentCharacterIndex++;
                    ManageTurnOrder();
                }
                else
                {
                    Debug.LogWarning("State: End Round");
                    state = BattleState.NEWROUND;
                    currentCharacterIndex = 0; //reset the index for the next round
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

    public void ManageTurnOrder()
    {
        List<string> turnOrderList = new List<string>();
        foreach (CharacterStats character in charactersList)
        {
            turnOrderList.Add(character.gameObject.name);
        }

        if (currentCharacterIndex > 0)
        {
            turnOrderList.RemoveAt(currentCharacterIndex - 1);
            turnOrderList.Insert(0, currentCharacter.gameObject.name);
            Debug.LogWarning("Inserting " + currentCharacter.gameObject.name);
        }

        string turnOrderListString = "";
        for (int i = 0; i < turnOrderList.Count; i++)
        {
            turnOrderListString += turnOrderList[i] + "\n";
        }

        turnOrder.text = turnOrderListString;
    }

    public IEnumerator NewRoundDelay(float seconds)
    {
        DetermineTurnOrder();

        yield return new WaitForSeconds(seconds);

        state = BattleState.NEXTTURN;

        playerHud.SetActive(true);
        enemyHud.SetActive(true);
    }
}
