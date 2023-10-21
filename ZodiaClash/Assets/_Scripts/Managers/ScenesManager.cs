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

    [SerializeField] private SceneTransition sceneTransition;

    private void Start()
    {
        scene = SceneManager.GetActiveScene();
        fadeManager = FindObjectOfType<FadeManager>();
        battleManager = FindObjectOfType<BattleManager>();

        switch (scene.name)
        {
            case "1_Exploration Map":

                explorationManager = FindObjectOfType<_ExplorationManager>();
                explorationManager.FindAllEnemyNpcs();
                AudioManager.Instance.PlayMusic("Exploration BGM");
                AudioManager.Instance.PlayAmbienceMusic("Forest Ambience");

                if (sceneTransition.clearedLevel)
                {
                    sceneTransition.enemyNpc = explorationManager.GetSpecificNpc(sceneTransition.levelIndex);
                    sceneTransition.defeatedEnemyNpcs.Add(sceneTransition.enemyNpc);
                }

                if (sceneTransition.defeatedEnemyNpcs != null)
                {
                    foreach (string enemyName in sceneTransition.defeatedEnemyNpcs)
                    {
                        foreach (GameObject enemy in explorationManager.enemyNpcs)
                        {
                            if (enemy.name == enemyName)
                            {
                                enemy.SetActive(false);
                                break;
                            }
                        }
                    }
                }
                break;
            case "2_BattleScene":

                AudioManager.Instance.PlayMusic("Battle BGM");
                AudioManager.Instance.PlayAmbienceMusic("Forest Ambience");
                break;
            case "3_BattleScene":

                AudioManager.Instance.PlayMusic("Battle BGM");
                AudioManager.Instance.PlayAmbienceMusic("Forest Ambience");
                break;
            default:
                break;
        }
        sceneTransition.levelIndex = scene.buildIndex;
    }

    private void Update()
    {
        #region Cheat Codes
        switch (scene.name)
        {
            case "1_Exploration Map":

                if (Input.GetKeyDown(KeyCode.F1))
                {
                    StartCoroutine(LoadMap());
                }
                break;

            case "2_BattleScene":

                if (Input.GetKeyDown(KeyCode.F1))
                {
                    StartCoroutine(LoadMap());
                }
                else if (Input.GetKeyDown(KeyCode.F2))
                {
                    SceneManager.LoadScene(2);
                }

                if (battleManager.battleState == BattleState.WIN)
                {
                    sceneTransition.clearedLevel = true;
                }
                
                break;

            case "3_BattleScene":

                if (Input.GetKeyDown(KeyCode.F1))
                {
                    StartCoroutine(LoadMap());
                }
                else if (Input.GetKeyDown(KeyCode.F2))
                {
                    SceneManager.LoadScene(3);
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

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(0);
    }

    public IEnumerator LoadMap()
    {
        fadeManager.SpawnFadeOutPanel();
        
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(1);
    }

    public IEnumerator LoadLevel()
    {
        fadeManager.SpawnFadeOutPanel();

        yield return new WaitForSeconds(1f);

        switch (scene.name)
        {
            case "2_BattleScene":

                SceneManager.LoadScene(2);
                break;
            case "3_BattleScene":

                SceneManager.LoadScene(3);
                break;
            default:

                Debug.LogError("No scene found! BUG!");
                break;
        }
    }

    public IEnumerator LoadLevelFromMap(int index)
    {
        fadeManager.SpawnFadeOutPanel();

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(index);
    }
}
