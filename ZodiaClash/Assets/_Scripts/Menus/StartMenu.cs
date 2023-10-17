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
        Destroy(Instantiate(fadeManager.fadeOutPanel, transform.position, Quaternion.identity, fadeManager.transform), 1f);

        StartCoroutine(DelayStartButton());
    }

    public void QuitButton()
    {
        Destroy(Instantiate(fadeManager.fadeOutPanel, transform.position, Quaternion.identity, fadeManager.transform), 1f);

        StartCoroutine(DelayQuitButton());
    }

    private IEnumerator DelayStartButton()
    {
        scene = SceneManager.GetActiveScene();

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(scene.buildIndex + 1);
    }

    private IEnumerator DelayQuitButton()
    {
        yield return new WaitForSeconds(1f);

        Application.Quit();
    }
}
