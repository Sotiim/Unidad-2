using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // Necesario para IEnumerator

public class SceneTransitioner : MonoBehaviour
{
    public CanvasGroup fadeImage; // Asignar la imagen negra aquí
    public float fadeDuration = 1f;

    // Llama a esta función al iniciar el juego
    void Start()
    {
        StartCoroutine(FadeIn());
    }

    // El fade se ejecuta ANTES de cargar la nueva escena
    public void StartTransition(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }

    IEnumerator FadeIn() // Transición al inicio de la escena (negro -> claro)
    {
        fadeImage.alpha = 1f;
        while (fadeImage.alpha > 0)
        {
            fadeImage.alpha -= Time.deltaTime / fadeDuration;
            yield return null;
        }
        fadeImage.alpha = 0f;
    }

    IEnumerator FadeOut(string sceneName) // Transición antes de salir (claro -> negro)
    {
        fadeImage.alpha = 0f;
        while (fadeImage.alpha < 1)
        {
            fadeImage.alpha += Time.deltaTime / fadeDuration;
            yield return null;
        }
        fadeImage.alpha = 1f;
        SceneManager.LoadScene(sceneName);
    }
}