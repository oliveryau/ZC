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
    [SerializeField] private GameObject skillChiHud;
    [SerializeField] private GameObject enemyHud;
    [SerializeField] private TextMeshProUGUI turnOrder;

    [Header("Turn Management")]
    public int roundCounter;
    public int currentCharacterIndex;
    public string activePlayer;
    public string activeEnemy;
    [HideInInspector] public List<string> turnOrderList = new List<string>();

    private void Start()
    {
        state = BattleState.NEWROUND;

        CharacterStats[] characters = FindObjectsOfType<CharacterStats>();
        foreach (CharacterStats chara in characters)
        {
            charactersList.Add(chara);
        }

        roundCounter = 0;
        roundInProgress = false;

        playerHud.SetActive(false);
        skillChiHud.SetActive(false);
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

                StartCoroutine(NewRoundDelay(0.5f)); //delay and switch states
            }
        }
        else if (state == BattleState.NEXTTURN)
        {
            Debug.LogWarning("State: Next Turn");
            if (currentCharacter != null)
            {
                turnOrderList.Remove(currentCharacter.gameObject.name);
            }

            //check if battle has ended
            if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 0) //no enemies left
            {
                Debug.LogWarning("State: Win");
                state = BattleState.WIN;

                playerHud.SetActive(false);
                skillChiHud.SetActive(false);
                enemyHud.SetActive(false);
            }
            else if (GameObject.FindGameObjectsWithTag("Player").Length <= 0) //no players left
            {
                Debug.LogWarning("State: Lose");
                state = BattleState.LOSE;

                playerHud.SetActive(false);
                skillChiHud.SetActive(false);
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
                        UpdateTurnOrderUi();

                        state = BattleState.ENEMYTURN;
                    }
                    else if (currentCharacter.tag == "Player")
                    {
                        activePlayer = currentCharacter.gameObject.name;

                        Debug.LogWarning("State: Player Turn");
                        UpdateTurnOrderUi();
                        skillChiHud.SetActive(true);

                        state = BattleState.PLAYERTURN;
                    }
                    currentCharacterIndex++;
                }
                else
                {
                    Debug.LogWarning("State: End Round");
                    state = BattleState.NEWROUND;

                    currentCharacter = null;
                    activeEnemy = null;
                    activePlayer = null;
                    UpdateTurnOrderUi();

                    currentCharacterIndex = 0; //reset the index for the next round
                    roundInProgress = false;
                }
            }
        }
    }
    
    public void DetermineTurnOrder()
    {
        charactersList.Sort((a, b) => b.speed.CompareTo(a.speed));

        turnOrderList.Clear();

        foreach (CharacterStats chara in charactersList)
        {
            turnOrderList.Add(chara.gameObject.name);
        }
    }

    public void UpdateTurnOrderUi()
    {
        turnOrder.text = "TURN:\n";
        turnOrder.color = Color.white;

        if (turnOrderList.Count > 0)
        {
            foreach (string characterName in turnOrderList)
            {
                if (characterName == activePlayer) //player's turn
                {
                    turnOrder.text += "<color=green>" + characterName + "</color>\n";
                }
                else if (characterName == activeEnemy) //enemy's turn
                {
                    turnOrder.text += "<color=red>" + characterName + "</color>\n";
                }
                else
                {
                    turnOrder.text += characterName + "\n"; //other names
                }
            }
        }
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
