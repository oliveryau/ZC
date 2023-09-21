using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BattleState
{
    NEWGAME, NEWROUND, NEXTTURN, PLAYERTURN, ENEMYTURN, WIN, LOSE
}

public class GameManager : MonoBehaviour
{
    public BattleState state;
    
    private CharacterStats currentCharacter;
    private bool roundInProgress;

    [Header("HUD")]
    [SerializeField] private GameObject turnOrder;
    [SerializeField] private GameObject playerHud;
    [SerializeField] private GameObject enemyHud;
    [SerializeField] private GameObject skillChiHud;
    [SerializeField] private GameObject statusEffectIndicator;

    [Header("Turn Management")]
    public List<CharacterStats> charactersList = new List<CharacterStats>();
    public List<CharacterStats> turnOrderList = new List<CharacterStats>();
    public int currentCharacterIndex;
    public string activePlayer;
    public string activeEnemy;

    [Header("Others")]
    public int roundCounter;

    private void Start()
    {
        state = BattleState.NEWGAME;

        roundInProgress = false;

        turnOrder.SetActive(false);
        playerHud.SetActive(false);
        enemyHud.SetActive(false);
        skillChiHud.SetActive(false);
        statusEffectIndicator.SetActive(false);
        
        roundCounter = 0;

        StartCoroutine(NewGameDelay(1f)); //delay at start
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            SceneManager.LoadScene(0);
        }

        if (state == BattleState.NEWROUND)
        {
            if (!roundInProgress)
            {
                //introduce UI for a new round and any other conditions for a new round need to be met
                roundInProgress = true;
                ++roundCounter;

                state = BattleState.NEXTTURN;
            }
        }
        else if (state == BattleState.NEXTTURN)
        {
            Debug.LogWarning("State: Next Turn");
            if (currentCharacter != null)
            {
                turnOrderList.Remove(currentCharacter);
                turnOrderList.Add(currentCharacter);
            }

            //check if battle has ended
            if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 0) //no enemies left
            {
                Debug.LogWarning("State: Win");
                state = BattleState.WIN;

                turnOrder.SetActive(false);
                playerHud.SetActive(false);
                enemyHud.SetActive(false);
                skillChiHud.SetActive(false);
                statusEffectIndicator.SetActive(false);
            }
            else if (GameObject.FindGameObjectsWithTag("Player").Length <= 0) //no players left
            {
                Debug.LogWarning("State: Lose");
                state = BattleState.LOSE;

                turnOrder.SetActive(false);
                playerHud.SetActive(false);
                enemyHud.SetActive(false);
                skillChiHud.SetActive(false);
                statusEffectIndicator.SetActive(false);
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
                        activePlayer = null;

                        Debug.LogWarning("State: Enemy Turn");
                        UpdateTurnOrderUi();

                        state = BattleState.ENEMYTURN;
                    }
                    else if (currentCharacter.tag == "Player")
                    {
                        activePlayer = currentCharacter.gameObject.name;
                        activeEnemy = null;

                        Debug.LogWarning("State: Player Turn");
                        UpdateTurnOrderUi();

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

                    currentCharacterIndex = 0; //reset the index for the next round
                    roundInProgress = false;
                }
            }
        }
    }
    
    public void DetermineTurnOrder()
    {
        //charactersList
        CharacterStats[] characters = FindObjectsOfType<CharacterStats>();

        charactersList.Clear();

        foreach (CharacterStats chara in characters)
        {
            charactersList.Add(chara);
        }

        charactersList.Sort((a, b) => b.speed.CompareTo(a.speed));

        //turnOrderList
        turnOrderList.Clear();

        foreach (CharacterStats chara in charactersList)
        {
            turnOrderList.Add(chara);
        }
    }

    public void UpdateTurnOrderUi()
    {
        TextMeshProUGUI turnOrderText = turnOrder.GetComponentInChildren<TextMeshProUGUI>();

        turnOrderText.text = "TURN:\n\n";

        if (turnOrderList.Count > 0)
        {
            foreach (CharacterStats character in turnOrderList)
            {
                if (character.gameObject.name == activePlayer) //player's turn
                {
                    turnOrderText.text += "<color=green>" + character.gameObject.name + "</color>\n";
                }
                else if (character.gameObject.name == activeEnemy) //enemy's turn
                {
                    turnOrderText.text += "<color=red>" + character.gameObject.name + "</color>\n";
                }
                else
                {
                    turnOrderText.text += "<color=white>" + character.gameObject.name + "</color>\n"; //other characters
                }
            }
        }
    }

    public IEnumerator NewGameDelay(float seconds)
    {
        DetermineTurnOrder();

        yield return new WaitForSeconds(seconds);

        state = BattleState.NEWROUND;

        turnOrder.SetActive(true);
        playerHud.SetActive(true);
        enemyHud.SetActive(true);
        skillChiHud.SetActive(true);
        statusEffectIndicator.SetActive(true);
    }
}
