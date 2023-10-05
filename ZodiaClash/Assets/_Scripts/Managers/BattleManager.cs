using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
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
                    if (activeCharacter.health > 0)
                    {
                        turnOrderList.Remove(activeCharacter);
                        originalTurnOrderList.Remove(activeCharacter);

                        turnOrderList.Add(activeCharacter);
                        originalTurnOrderList.Add(activeCharacter);    
                        
                        UpdateTurnOrderUi();
                    }
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

                        battleState = BattleState.ENEMYTURN;
                    }
                    else if (activeCharacter.tag == "Player")
                    {
                        activePlayer = activeCharacter.gameObject.name;
                        activeEnemy = null;

                        Debug.LogWarning("State: Player Turn");

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

        //turn order ui
        for (int i = 0; i < turnOrderList.Count; i++)
        {
            CharacterStats character = turnOrderList[i];
            GameObject avatarContainer = Instantiate(avatarContainerPrefab, avatarListContainer);

            Transform avatarListChild = avatarListContainer.GetChild(i);
            character.uniqueTurnHud = avatarListChild;

            #region Character Sprite
            Image activeCharacterImg = avatarContainer.transform.Find("Unique Avatar").GetComponent<Image>();
            activeCharacterImg.sprite = character.uniqueCharacterAvatar;
            #endregion

            #region Colour
            Image characterIndicator = avatarContainer.transform.Find("Unique Indicator").GetComponent<Image>();
            if (character.gameObject.CompareTag("Player"))
            {
                characterIndicator.color = Color.green;
            }
            else if (character.gameObject.CompareTag("Enemy"))
            {
                characterIndicator.color = Color.red;
            }
            #endregion

            #region Size
            if (i == 0)
            {
                float scaleFactor = 1.2f;
                character.uniqueTurnHud.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
            }
            #endregion
        }
    }

    public void UpdateTurnOrderUi(string effect = null, CharacterStats target = null)
    {
        if (effect == null && target == null)
        {
            for (int i = 0; i < turnOrderList.Count; i++)
            {
                CharacterStats character = turnOrderList[i];
                Transform currentAvatarTransform = avatarListContainer.GetChild(i);

                if (battleState == BattleState.PLAYERTURN && character.uniqueTurnHud == currentAvatarTransform)
                {
                }
                else if (battleState == BattleState.ENEMYTURN && character.uniqueTurnHud == currentAvatarTransform)
                {
                }
                else
                {
                    MoveChildAvatarTransforms(1, avatarListContainer);

                    //previous character's turn
                    Transform lastAvatarTransform = avatarListContainer.GetChild(turnOrderList.Count - 1);
                    Vector3 currentAvatarTargetPosition = lastAvatarTransform.transform.position;
                    StartCoroutine(LerpTurn(currentAvatarTransform, currentAvatarTargetPosition));

                    currentAvatarTransform.SetAsLastSibling();
                    break;
                }
            }
        }

        if (effect != null && target != null)
        {
            for (int i = 0; i < turnOrderList.Count; i++)
            {
                CharacterStats character = turnOrderList[i];
                Transform currentAvatarTransform = avatarListContainer.GetChild(i);

                switch (effect)
                {
                    case "death":
                        if (target == character)
                        {
                            Transform lastAvatarTransform = avatarListContainer.GetChild(turnOrderList.Count - 1);
                            Vector3 targetAvatarTargetPosition = lastAvatarTransform.transform.position;
                            StartCoroutine(LerpTurn(currentAvatarTransform, targetAvatarTargetPosition, true));

                            StartCoroutine(DeathTurnRemove(target.uniqueTurnHud.gameObject));
                            break;
                        }
                        break;
                    case "switch":
                        if (target == character)
                        {
                            Transform nextAvatarTransform = avatarListContainer.GetChild(1);
                            Vector3 targetAvatarTargetPosition = nextAvatarTransform.transform.position;
                            StartCoroutine(LerpTurn(currentAvatarTransform, targetAvatarTargetPosition));

                            currentAvatarTransform.SetSiblingIndex(1);
                            break;
                        }
                        break;
                    case "revert":
                        break;
                    default:
                        break;
                }
            }
        }

        #region Size
        for (int i = 0;i < turnOrderList.Count; i++)
        {
            CharacterStats character = turnOrderList[i];

            if (i == 0)
            {
                float scaleFactor = 1.2f;
                character.uniqueTurnHud.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
            }
            else
            {
                character.uniqueTurnHud.localScale = Vector3.one;
            }
        }
        #endregion
    }

    private void MoveChildAvatarTransforms(int index, Transform parent, int targetIndex = 0)
    {
        if (index >= parent.childCount)
        {
            return;
        }

        if (targetIndex == index)
        {
            MoveChildAvatarTransforms(index + 1, parent); //recursive method
        }
        else
        {
            Transform currentChild = parent.GetChild(index);
            Transform previousChild = parent.GetChild(index - 1);
            Vector3 currentChildTargetPosition = previousChild.transform.position;
            StartCoroutine(LerpTurn(currentChild, currentChildTargetPosition));

            MoveChildAvatarTransforms(index + 1, parent); //recursive method
        }
    }

    private IEnumerator LerpTurn(Transform characterTransform, Vector3 targetPosition, bool dead = false)
    {
        float duration = 0.15f;
        float elapsedTime = 0f;
        Vector3 startingPosition = characterTransform.position;

        while (elapsedTime < duration && !dead)
        {
            float t = elapsedTime / duration;
            characterTransform.position = Vector3.Lerp(startingPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        characterTransform.position = targetPosition;
    }

    private IEnumerator DeathTurnRemove(GameObject targetAvatar)
    {
        yield return new WaitForSeconds(0.01f);
        Destroy(targetAvatar);
    }

    public void SwitchTurnOrder(CharacterStats target)
    {
        turnOrderList.Remove(target);
        turnOrderList.Insert(1, target);
        UpdateTurnOrderUi("switch", target);
    }

    public void RevertTurnOrder(CharacterStats target)
    {
        int originalIndex = originalTurnOrderList.IndexOf(target);

        turnOrderList.Remove(target);
        turnOrderList.Insert(originalIndex, target);

        UpdateTurnOrderUi("revert");
    }
    #endregion
}
