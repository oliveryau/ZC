using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState
{
    NEWGAME, NEWROUND, NEXTTURN, PLAYERTURN, ENEMYTURN, WIN, LOSE
}

public class BattleManager : MonoBehaviour
{
    public BattleState battleState;
    
    private CharacterStats activeCharacter;
    private bool roundInProgress;

    [Header("HUD")]
    [SerializeField] private GameObject turnOrder;
    [SerializeField] private GameObject avatarContainerPrefab;
    [SerializeField] private Transform avatarListContainer;
    [SerializeField] private GameObject playerHud;
    [SerializeField] private GameObject enemyHud;
    [SerializeField] private GameObject skillChiHud;
    [SerializeField] private GameObject statusEffectIndicator;

    [Header("Round Management")]
    public int characterCount;
    public int roundCounter;

    [Header("Turn Management")]
    public string activePlayer;
    public string activeEnemy;
    public List<CharacterStats> charactersList = new List<CharacterStats>();
    public List<CharacterStats> turnOrderList = new List<CharacterStats>();
    public List<CharacterStats> originalTurnOrderList = new List<CharacterStats>();
    [HideInInspector] public bool revertingTurn;

    private void Start()
    {
        battleState = BattleState.NEWGAME;

        roundInProgress = false;

        turnOrder.SetActive(false);
        playerHud.SetActive(false);
        enemyHud.SetActive(false);
        skillChiHud.SetActive(false);
        statusEffectIndicator.SetActive(false);

        characterCount = 0;
        roundCounter = 0;

        StartCoroutine(NewGameDelay(1f)); //delay at start
    }

    private void Update()
    {
        if (battleState == BattleState.NEWROUND)
        {
            if (!roundInProgress)
            {
                //introduce UI for a new round and any other conditions for a new round need to be met
                roundInProgress = true;
                ++roundCounter;

                battleState = BattleState.NEXTTURN;
            }
        }
        else if (battleState == BattleState.NEXTTURN)
        {
            Debug.LogWarning("State: Next Turn");
            //first shift the currentCharacter in turnOrderList to the last
            if (activeCharacter != null)
            {
                if (revertingTurn)
                {
                    RevertTurnOrder(activeCharacter);
                    revertingTurn = false;

                    --characterCount; //minus for roundCounter
                }
                else if (!revertingTurn)
                {
                    turnOrderList.Remove(activeCharacter);
                    originalTurnOrderList.Remove(activeCharacter);

                    turnOrderList.Add(activeCharacter);
                    originalTurnOrderList.Add(activeCharacter);
                }
            }

            //then check if player has won
            if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 0) //no enemies left
            {
                Debug.LogWarning("State: Win");
                battleState = BattleState.WIN;

                turnOrder.SetActive(false);
                playerHud.SetActive(false);
                enemyHud.SetActive(false);
                skillChiHud.SetActive(false);
                statusEffectIndicator.SetActive(false);
            }
            //then check if enemy has won
            else if (GameObject.FindGameObjectsWithTag("Player").Length <= 0) //no players left
            {
                Debug.LogWarning("State: Lose");
                battleState = BattleState.LOSE;

                turnOrder.SetActive(false);
                playerHud.SetActive(false);
                enemyHud.SetActive(false);
                skillChiHud.SetActive(false);
                statusEffectIndicator.SetActive(false);
            }
            //continue battle
            else
            {
                if (characterCount < charactersList.Count)
                {
                    activeCharacter = turnOrderList[0];
                    if (activeCharacter.tag == "Enemy")
                    {
                        activeEnemy = activeCharacter.gameObject.name;
                        activePlayer = null;

                        Debug.LogWarning("State: Enemy Turn");
                        UpdateTurnOrderUi();

                        battleState = BattleState.ENEMYTURN;
                    }
                    else if (activeCharacter.tag == "Player")
                    {
                        activePlayer = activeCharacter.gameObject.name;
                        activeEnemy = null;

                        Debug.LogWarning("State: Player Turn");
                        UpdateTurnOrderUi();

                        battleState = BattleState.PLAYERTURN;
                    }
                    ++characterCount;
                }
                else
                {
                    Debug.LogWarning("State: End Round");
                    battleState = BattleState.NEWROUND;

                    activeCharacter = null;
                    activeEnemy = null;
                    activePlayer = null;

                    characterCount = 0;
                    roundInProgress = false;
                }
            }
        }
    }

    public IEnumerator NewGameDelay(float seconds)
    {
        DetermineTurnOrder();

        yield return new WaitForSeconds(seconds);

        battleState = BattleState.NEWROUND;

        turnOrder.SetActive(true);
        playerHud.SetActive(true);
        enemyHud.SetActive(true);
        skillChiHud.SetActive(true);
        statusEffectIndicator.SetActive(true);
    }

    #region Turn Management
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

        foreach (CharacterStats chara in charactersList) //add the sorted charactersList to turnOrderList
        {
            turnOrderList.Add(chara);
        }

        //originalTurnOrderList
        originalTurnOrderList.Clear();

        foreach (CharacterStats chara in turnOrderList) //add turnOrderList to original turn order list
        {
            originalTurnOrderList.Add(chara);
        }
    }

    public void UpdateTurnOrderUi()
    {
        foreach (Transform child in avatarListContainer)
        {
            Destroy(child.gameObject);
        }

        if (turnOrderList.Count > 0)
        {
            for (int i = 0; i < turnOrderList.Count; i++)
            {
                CharacterStats character = turnOrderList[i];

                GameObject avatarContainer = Instantiate(avatarContainerPrefab, avatarListContainer);

                #region Indicator Colours
                Image characterIndicator = avatarContainer.transform.Find("Unique Indicator").GetComponent<Image>();
                if (character.gameObject.name == activePlayer)
                {
                    characterIndicator.color = Color.green;
                }
                else if (character.gameObject.name == activeEnemy)
                {
                    characterIndicator.color = Color.red;
                }
                else
                {
                    characterIndicator.color = Color.white;
                }
                #endregion

                #region Character Sprite
                Image activeCharacterImg = avatarContainer.transform.Find("Unique Avatar").GetComponent<Image>();
                activeCharacterImg.sprite = character.uniqueCharacterAvatar;
                #endregion

                #region Size
                if (i == 0)
                {
                    float scaleFactor = 1.2f;
                    avatarContainer.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
                }
                #endregion
            }
        }
    }

    public void SwitchTurnOrder(CharacterStats target)
    {
        turnOrderList.Remove(target);
        turnOrderList.Insert(1, target);
        UpdateTurnOrderUi();
    }

    public void RevertTurnOrder(CharacterStats target)
    {
        int originalIndex = originalTurnOrderList.IndexOf(target);

        turnOrderList.Remove(target);
        turnOrderList.Insert(originalIndex, target);

        UpdateTurnOrderUi();
    }
    #endregion
}
