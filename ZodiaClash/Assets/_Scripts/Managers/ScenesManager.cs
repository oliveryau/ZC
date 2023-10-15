using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public Scene scene;

    private void Start()
    {
        scene = SceneManager.GetActiveScene();

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
            case "BattleScene 1":

                if (Input.GetKeyDown(KeyCode.F1))
                {
                    SceneManager.LoadScene(0);
                }
                else if (Input.GetKeyDown(KeyCode.F2))
                {
                    SceneManager.LoadScene(1);
                }
                break;

            case "BattleScene 2":

                if (Input.GetKeyDown(KeyCode.F1))
                {
                    SceneManager.LoadScene(0);
                }
                else if (Input.GetKeyDown(KeyCode.F2))
                {
                    SceneManager.LoadScene(2);
                }
                break;
            default:
                break;
        }
        #endregion
    }
}
