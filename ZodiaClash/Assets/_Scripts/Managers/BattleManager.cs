using System;
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
    
    private ScenesManager scenesManager;
    private GameManager gameManager;
    private CharacterStats activeCharacter;
    private bool roundInProgress;

    [Header("HUD")]
    [SerializeField] private GameObject battleStateHud;
    [SerializeField] private GameObject turnOrder;
    [SerializeField] private GameObject avatarContainerPrefab;
    [SerializeField] private Transform avatarListContainer;
    [SerializeField] private GameObject playerHud;
    [SerializeField] private GameObject enemyHud;
    [SerializeField] private GameObject statusEffectIndicator;
    public GameObject skillChiHud;

    [Header("Round Management")]
    public int characterCount;
    public int roundCounter;

    [Header("Turn Management")]
    public string activePlayer;
    public string activeEnemy;
    public List<CharacterStats> charactersList = new List<CharacterStats>();
    public List<CharacterStats> turnOrderList = new List<CharacterStats>();
    public List<CharacterStats> originalTurnOrderList = new List<CharacterStats>();
    [SerializeField] private Sprite playerIndicator;
    [SerializeField] private Sprite enemyIndicator;
    [HideInInspector] public bool revertingTurn;

    [Header("Others")]
    [SerializeField] private SceneTransition sceneTransition;

    private void Start()
    {
        battleState = BattleState.NEWGAME;

        scenesManager = FindObjectOfType<ScenesManager>();
        gameManager = FindObjectOfType<GameManager>();
        sceneTransition.clearedLevel = false;

        roundInProgress = false;
        characterCount = 0;
        roundCounter = 0;

        StartCoroutine(NewGameDelay(0.5f, 1f)); //delay at start
    }

    private void Update()
    {
        if (battleState == BattleState.NEWROUND)
        {
            if (!roundInProgress)
            {
                roundInProgress = true;
                ++roundCounter;

                battleState = BattleState.NEXTTURN;
            }
        }
        else if (battleState == BattleState.NEXTTURN)
        {
            Debug.LogWarning("State: Next Turn");
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

                battleStateHud.GetComponentInChildren<TextMeshProUGUI>().text = "Battle Over";
                battleStateHud.gameObject.SetActive(true);

                sceneTransition.clearedLevel = true;
                StartCoroutine(scenesManager.LoadMap());
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

                gameManager.lostBattle = true;
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

    public IEnumerator NewGameDelay(float startDelay, float nextDelay)
    {
        yield return new WaitForSeconds(startDelay);

        battleStateHud.gameObject.SetActive(true);

        yield return new WaitForSeconds(nextDelay);

        DetermineTurnOrder();

        playerHud.SetActive(true);
        enemyHud.SetActive(true);
        statusEffectIndicator.SetActive(true);
        
        battleStateHud.gameObject.SetActive(false);

        yield return new WaitForSeconds(nextDelay);

        turnOrder.SetActive(true);

        yield return new WaitForSeconds(startDelay);

        battleState = BattleState.NEWROUND;
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

            #region Indicator Colour
            Image characterIndicator = avatarContainer.transform.Find("Unique Indicator").GetComponent<Image>();
            if (character.gameObject.CompareTag("Player"))
            {
                //characterIndicator.color = Color.green;
                characterIndicator.sprite = playerIndicator;
            }
            else if (character.gameObject.CompareTag("Enemy"))
            {
                //characterIndicator.color = Color.red;
                characterIndicator.sprite = enemyIndicator;
            }
            #endregion

            #region Animations
            if (i == 0)
            {
                float scaleFactor = 1.2f;
                character.uniqueTurnHud.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
                character.uniqueTurnHud.transform.Find("Arrow").gameObject.SetActive(true);
            }
            #endregion
        }
    }

    public void UpdateTurnOrderUi(string effect = null, CharacterStats target = null, int additionalIndex = 0)
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
                    ////first move the other avatars up
                    //MoveAvatarNormal(1, avatarListContainer);

                    ////then move the current avatar to the back
                    //Transform lastAvatarTransform = avatarListContainer.GetChild(turnOrderList.Count - 1);
                    //Vector3 currentAvatarTargetPosition = lastAvatarTransform.transform.localPosition;
                    //StartCoroutine(LerpTurn(currentAvatarTransform, currentAvatarTargetPosition));

                    //finally set the current avatar as the last one
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
                            currentAvatarTransform = target.uniqueTurnHud;

                            currentAvatarTransform.GetComponent<Animator>().SetTrigger("death");
                            StartCoroutine(DeathRemoveAvatar(currentAvatarTransform));
                            break;
                        }
                        break;
                    case "switch":
                        if (target == character)
                        {
                            //first set the currentAvatar to the target's turn avatar
                            currentAvatarTransform = target.uniqueTurnHud;

                            ////move other avatars that are not target
                            //MoveAvatarSpeed(1, avatarListContainer, i);

                            ////move target avatar to first child
                            //Transform nextAvatarTransform = avatarListContainer.GetChild(1);
                            //Vector3 speedupAvatarTargetPosition = nextAvatarTransform.transform.position;
                            //StartCoroutine(LerpTurn(currentAvatarTransform, speedupAvatarTargetPosition));

                            //set index of avatar to next one
                            currentAvatarTransform.Find("SpeedUp").gameObject.SetActive(true);
                            currentAvatarTransform.SetSiblingIndex(1);
                            break;
                        }

                        break;
                    case "revert":
                        currentAvatarTransform = target.uniqueTurnHud;

                        currentAvatarTransform.Find("SpeedUp").gameObject.SetActive(false);
                        currentAvatarTransform.SetSiblingIndex(additionalIndex);
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
                character.uniqueTurnHud.transform.Find("Arrow").gameObject.SetActive(true);
            }
            else
            {
                character.uniqueTurnHud.localScale = Vector3.one;
                character.uniqueTurnHud.transform.Find("Arrow").gameObject.SetActive(false);
            }
        }
        #endregion
    }

    private void MoveAvatarNormal(int index, Transform parent)
    {
        if (index >= parent.childCount)
        {
            return;
        }
        else
        {
            Transform currentChild = parent.GetChild(index);
            Transform previousChild = parent.GetChild(index - 1);
            Vector3 currentChildTargetPosition = previousChild.transform.position;
            StartCoroutine(LerpTurn(currentChild, currentChildTargetPosition));

            MoveAvatarNormal(index + 1, parent);
        }
    }

    private void MoveAvatarSpeed(int index, Transform parent, int uniqueIndex = 0)
    {
        if (index >= parent.childCount)
        {
            return;
        }
        else if (uniqueIndex == index)
        {
            MoveAvatarSpeed(index + 1, parent);
        }
        else
        {
            Transform currentChild = parent.GetChild(index);

            if (index + 1 < parent.childCount)
            {
                Transform nextChild = parent.GetChild(index + 1);
                Vector3 currentChildTargetPosition = nextChild.transform.position;
                StartCoroutine(LerpTurn(currentChild, currentChildTargetPosition));
            }

            MoveAvatarSpeed(index + 1, parent);
        }
    }

    private IEnumerator LerpTurn(Transform characterTransform, Vector3 targetPosition, bool dead = false)
    {
        float duration = 0.2f;
        float elapsedTime = 0f;
        Vector3 startingPosition = characterTransform.localPosition;

        while (elapsedTime < duration && !dead)
        {
            float t = elapsedTime / duration;
            characterTransform.localPosition = Vector3.Lerp(startingPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        characterTransform.localPosition = targetPosition;
    }

    private IEnumerator DeathRemoveAvatar(Transform deadTarget)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(deadTarget.gameObject);
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

        UpdateTurnOrderUi("revert", target, originalIndex);
    }
    #endregion
}
