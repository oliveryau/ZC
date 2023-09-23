using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    private Scene scene;
    private FadeManager fadeManager;

    private void Start()
    {
        fadeManager = FindObjectOfType<FadeManager>();
    }

    public void StartButton()
    {
        fadeManager.fadeOutPanel.SetActive(true);
        StartCoroutine(DelayStartButton());
    }

    public void QuitButton()
    {
        fadeManager.fadeOutPanel.SetActive(true);
        StartCoroutine(DelayQuitButton());
    }

    private IEnumerator DelayStartButton()
    {
        yield return new WaitForSeconds(0.5f);

        scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex + 1);
    }

    private IEnumerator DelayQuitButton()
    {
        yield return new WaitForSeconds(0.5f);

        Application.Quit();
    }
}
