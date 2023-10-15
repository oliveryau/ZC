using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    PLAY, PAUSE, EFFECTS
}

public class GameManager : MonoBehaviour
{
    public GameState gameState;

    [Header("HUD")]
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject effectsScreen;

    [SerializeField] private bool isPaused;
    [SerializeField] private bool isEffectDisplayed;

    private void Start()
    {
        gameState = GameState.PLAY;
    }

    private void Update()
    {
        if (gameState == GameState.PLAY) //normal play state
        {
            Time.timeScale = 1f;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isPaused = true;
            }

            if (isPaused)
            {
                pauseScreen.SetActive(true);

                gameState = GameState.PAUSE;
            }

            if (isEffectDisplayed)
            {
                effectsScreen.SetActive(true);

                gameState = GameState.EFFECTS;
            }
        }

        else if (gameState == GameState.PAUSE) //pause screen
        {
            Time.timeScale = 0f;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isPaused = false;
            }

            if (!isPaused)
            {
                pauseScreen.SetActive(false);

                gameState = GameState.PLAY;
            }
        }

        else if (gameState == GameState.EFFECTS) //effects list screen
        {
            Time.timeScale = 0f;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isEffectDisplayed = false;
            }

            if (!isEffectDisplayed)
            {
                effectsScreen.SetActive(false);

                gameState = GameState.PLAY;
            }
        }
    }

    #region Pause
    public void ResumeGame()
    {
        isPaused = false;
    }

    public void RestartBattle()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitBattle()
    {
        isPaused = false;

        //load previous scene
    }
    #endregion

    #region Status Effects Buttons
    public void ShowEffectsInfo()
    {
        isEffectDisplayed = true;
    }

    public void HideEffectsInfo()
    {
        isEffectDisplayed = false;
    }
    #endregion
}
