using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public Scene scene;
    private FadeManager fadeManager;

    private void Start()
    {
        scene = SceneManager.GetActiveScene();
        fadeManager = FindObjectOfType<FadeManager>();

        switch (scene.name)
        {
            case "BattleScene 1":

                AudioManager.Instance.PlayMusic("Battle BGM");
                AudioManager.Instance.PlayAmbienceMusic("Battle Ambience");
                break;
            case "BattleScene 2":

                AudioManager.Instance.PlayMusic("Battle BGM");
                AudioManager.Instance.PlayAmbienceMusic("Battle Ambience");
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        #region Cheat Codes
        switch (scene.name)
        {
            case "Exploration Map":

                if (Input.GetKeyDown(KeyCode.F1))
                {
                    SceneManager.LoadScene(1);
                }
                break;

            case "BattleScene 1":

                if (Input.GetKeyDown(KeyCode.F1))
                {
                    SceneManager.LoadScene(1);
                }
                else if (Input.GetKeyDown(KeyCode.F2))
                {
                    SceneManager.LoadScene(2);
                }
                break;

            case "BattleScene 2":

                if (Input.GetKeyDown(KeyCode.F1))
                {
                    SceneManager.LoadScene(1);
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
            case "BattleScene 1":

                SceneManager.LoadScene(2);
                break;
            case "BattleScene 2":

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
