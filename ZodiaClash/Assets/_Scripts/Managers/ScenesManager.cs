using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public Scene scene;
    private FadeManager fadeManager;
    private BattleManager battleManager;
    private _ExplorationManager explorationManager;
    private PlayerMovement playerMovement;

    [SerializeField] private SceneTransition sceneTransition;

    private void Start()
    {
        scene = SceneManager.GetActiveScene();
        fadeManager = FindObjectOfType<FadeManager>();
        battleManager = FindObjectOfType<BattleManager>();

        switch (scene.name)
        {
            case "0_StartMenu":

                AudioManager.Instance.PlayMusic("Menu BGM");
                AudioManager.Instance.MusicFadeIn(true);

                sceneTransition.ResetSceneData();
                break;
            case "1_Exploration Map":

                AudioManager.Instance.PlayMusic("Exploration BGM");
                AudioManager.Instance.PlayAmbienceMusic("Forest Ambience");
                AudioManager.Instance.MusicFadeIn(true);

                #region Unique Save Loads
                explorationManager = FindObjectOfType<_ExplorationManager>();
                explorationManager.FindAllEnemyNpcs();

                if (sceneTransition.clearedLevel)
                {
                    sceneTransition.enemyNpc = explorationManager.GetSpecificNpc(sceneTransition.levelIndex); //get npc that is just defeated
                    sceneTransition.defeatedEnemyNpcs.Add(sceneTransition.enemyNpc); //add it to the defeated enemies list

                    #region Unique Dialogues
                    switch (sceneTransition.levelIndex)
                    {
                        case 2:
                            //normal guard level
                            playerMovement = FindObjectOfType<PlayerMovement>();
                            playerMovement.DialogueUi.ShowDialogue(explorationManager.defeatedFirstGuardDialogue); 
                            break;
                        case 3:
                            //goat level
                            playerMovement = FindObjectOfType<PlayerMovement>();
                            playerMovement.DialogueUi.ShowDialogue(explorationManager.defeatedGoatDialogue);
                            break;
                        default:
                            break;
                    }
                    #endregion
                }

                if (sceneTransition.defeatedEnemyNpcs != null)
                {
                    foreach (string enemyName in sceneTransition.defeatedEnemyNpcs)
                    {
                        foreach (GameObject enemy in explorationManager.enemyNpcs)
                        {
                            if (enemy.name == enemyName)
                            {
                                enemy.SetActive(false); //hide defeated enemy npcs
                                break;
                            }
                        }
                    }
                }
                #endregion
                break;
            case "2_BattleScene":
            case "3_BattleScene":
            case "4_BattleScene":

                AudioManager.Instance.PlayMusic("Battle BGM");
                AudioManager.Instance.PlayAmbienceMusic("Forest Ambience");
                AudioManager.Instance.MusicFadeIn(true);
                break;
            default:
                break;
        }
        sceneTransition.levelIndex = scene.buildIndex;
        sceneTransition.enemyNpc = null;
    }

    private void Update()
    {
        #region Cheat Codes
        switch (scene.name)
        {
            case "1_Exploration Map":

                if (Input.GetKeyDown(KeyCode.F1))
                {
                    sceneTransition.ResetSceneData();

                    StartCoroutine(LoadMap());
                }
                break;

            case "2_BattleScene":
            case "3_BattleScene":

                if (Input.GetKeyDown(KeyCode.F1))
                {
                    StartCoroutine(LoadMap());
                }
                else if (Input.GetKeyDown(KeyCode.F2))
                {
                    StartCoroutine(LoadLevel());
                }

                if (battleManager.battleState == BattleState.WIN)
                {
                    sceneTransition.clearedLevel = true;
                }
                break;
            default:
                break;
        }
        #endregion
    }

    public IEnumerator LoadMenu()
    {
        fadeManager.SpawnFadeOutPanel();
        AudioManager.Instance.MusicFadeIn(false);

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(0);
    }

    public IEnumerator LoadMap()
    {
        fadeManager.SpawnFadeOutPanel();
        AudioManager.Instance.MusicFadeIn(false);

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(1);
    }

    public IEnumerator LoadLevel()
    {
        fadeManager.SpawnFadeOutPanel();
        AudioManager.Instance.MusicFadeIn(false);

        yield return new WaitForSeconds(1f);

        switch (scene.name)
        {
            case "2_BattleScene":

                SceneManager.LoadScene(2);
                break;
            case "3_BattleScene":

                SceneManager.LoadScene(3);
                break;
            case "4_BattleScene":

                SceneManager.LoadScene(4);
                break;
            default:

                Debug.LogError("No scene found! BUG!");
                break;
        }
    }

    public IEnumerator LoadLevelFromMap(int index)
    {
        fadeManager.SpawnFadeOutPanel();
        AudioManager.Instance.MusicFadeIn(false);

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(index);
    }
}
