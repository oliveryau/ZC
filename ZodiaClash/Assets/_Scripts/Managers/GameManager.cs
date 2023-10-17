using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    WAIT, PLAY, PAUSE, EFFECTS
}

public class GameManager : MonoBehaviour
{
    public GameState gameState;

    [Header("HUD")]
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject effectsScreen;

    [SerializeField] private bool isPaused;
    [SerializeField] private bool isEffectDisplayed;

    private ScenesManager scenesManager;

    private void Start()
    {
        scenesManager = FindObjectOfType<ScenesManager>();

        gameState = GameState.WAIT;

        StartCoroutine(StartDelay());
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

    private IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(1f);

        gameState = GameState.PLAY;
    }

    #region Pause Menu Buttons
    public void ResumeGame()
    {
        isPaused = false;
    }

    public void RestartBattle()
    {
        Time.timeScale = 1f;
        gameState = GameState.WAIT;

        StartCoroutine(scenesManager.LoadLevel());
    }

    public void ExitBattle()
    {
        Time.timeScale = 1f;
        gameState = GameState.WAIT;

        StartCoroutine(scenesManager.LoadMap());
    }

    public void ExitMenu()
    {
        Time.timeScale = 1f;
        gameState = GameState.WAIT;

        StartCoroutine(scenesManager.LoadMenu());
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
