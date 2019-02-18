using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenController : MonoBehaviour
{
    public Image fadeToBlackImage;
    public TMPro.TextMeshProUGUI loadingText;
    public GameObject titleScreenTextObject;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForInputThenLoadLevel());
    }

    private IEnumerator WaitForInputThenLoadLevel()
    {
        while (Input.anyKey == false)
        {
            yield return null;
        }

        yield return FadeThenLoad(.005f);
    }

    /// <summary>
    /// Fades for a given amount of time then loads the selected scene.
    /// </summary>
    /// <returns></returns>
    public IEnumerator FadeThenLoad(float fadeTime)
    {
        Debug.Log($"<color=white>{nameof(TitleScreenController)}</color>: Starting loading/unloading coroutine.");

        loadingText.gameObject.SetActive(true);

        var color = fadeToBlackImage.color;
        color.a = 0;
        fadeToBlackImage.color = color;

        // Wait for the screen to fade to black
        while (color.a < 1)
        {
            yield return null;
            color.a += fadeTime;
            fadeToBlackImage.color = color;
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("LevelOne", LoadSceneMode.Additive);

        Debug.Log($"<color=white>{nameof(TitleScreenController)}</color>: Started loading LevelOne scene.");

        while (asyncLoad.isDone == false)
        {
            yield return null;
        }

        Debug.Log($"<color=white>{nameof(TitleScreenController)}</color>: Finished loading LevelOne scene.");

        titleScreenTextObject.SetActive(false);
        loadingText.gameObject.SetActive(false);

        // Wait for the screen to fade to clear
        while (color.a > 0)
        {
            yield return null;
            color.a -= fadeTime;
            fadeToBlackImage.color = color;
        }

        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync("TitleScreen", UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

        Debug.Log($"<color=white>{nameof(TitleScreenController)}</color>: Started unloading TitleScreen scene.");

        while (asyncUnload.isDone == false)
        {
            yield return null;
        }

        Debug.Log($"<color=white>{nameof(TitleScreenController)}</color>: Finished loading TitleScreen scene.");

        Debug.Log($"<color=white>{nameof(TitleScreenController)}</color>: Finished loading/unloading coroutine.");
    }
}
