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
            case "BattleScene 2":
                AudioManager.Instance.PlayMusic("Battle BGM");
                break;
            default:
                break;
        }
    }
}
