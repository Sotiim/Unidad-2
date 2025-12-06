using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cargar escenas
using UnityEngine.UI;               // Necesario para usar el Slider
using System.Collections;           // Necesario para Coroutines
// Se agrega la librería de Unity Editor para que la funcion de salir funcione en el editor.
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LoadingManager : MonoBehaviour
{
    // Asigna el nombre de la escena que debe cargar (ej. "Nivel2_Entrega")
    public string sceneToLoad;

    // Asigna el nombre de la escena del menu principal
    // Asegúrate de que este nombre coincida exactamente con tu escena de menú.
    public string menuSceneName = "Menu";

    // Arrastra tu Barra_Progreso (Slider) aquí desde el Inspector
    public Slider progressBar;

    // Tiempo mínimo que quieres que dure la pantalla de carga (en segundos)
    public float minimumLoadTime = 10f;

    void Start()
    {
        // Inicia el proceso de carga solo si hay una escena a cargar, 
        // ya que este script ahora también se usará para navegación post-carga.
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            StartCoroutine(LoadAsyncOperation());
        }
    }

    IEnumerator LoadAsyncOperation()
    {
        // 1. INICIAR LA CARGA ASÍNCRONA
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        // Evitamos que la escena se active automáticamente al 90%
        operation.allowSceneActivation = false;

        float timeElapsed = 0f;

        while (!operation.isDone)
        {
            timeElapsed += Time.deltaTime;

            // A. NORMALIZAR PROGRESO DE UNITY (Escala de 0.0 a 0.9 a un rango de 0.0 a 1.0)
            float scaledUnityProgress = Mathf.Clamp01(operation.progress / 0.9f);

            // B. CALCULAR EL PROGRESO BASADO EN EL TIEMPO MÍNIMO (0.0 a 1.0)
            float timeProgress = timeElapsed / minimumLoadTime;

            // C. PROGRESO FINAL: Usamos el más lento de los dos:
            float finalProgress = Mathf.Min(scaledUnityProgress, timeProgress);


            // D. MANEJAR LA TRANSICIÓN VISUAL FINAL
            if (operation.progress >= 0.9f)
            {
                finalProgress = timeProgress;
            }

            // 3. ACTUALIZAR LA BARRA DE PROGRESO
            if (progressBar != null)
            {
                // Mostramos el progreso final
                progressBar.value = finalProgress;
            }

            yield return null; // Espera al siguiente frame

            // 4. VERIFICAR LA TRANSICIÓN A LA ESCENA
            // Solo se activa la escena si ya cargó (0.9f) Y ha pasado el tiempo mínimo.
            if (operation.progress >= 0.9f && timeElapsed >= minimumLoadTime)
            {
                operation.allowSceneActivation = true;
                break;
            }
        }
    }

    public void GoToMenu()
    {
        // Detiene cualquier carga asíncrona que pueda estar en curso.
        StopAllCoroutines();

        // Carga la escena del menú principal (debe estar en Build Settings).
        SceneManager.LoadScene(menuSceneName);
    }

    /// <summary>
        /// Sale de la aplicación.
        /// Se debe asignar al botón "Salir del Juego" de la pantalla final.
        /// </summary>
    public void QuitGame()
    {
        // Detiene cualquier carga asíncrona que pueda estar en curso.
        StopAllCoroutines();

        // La directiva #if UNITY_EDITOR permite que esta línea solo se ejecute
        // cuando se está probando el juego dentro del Editor de Unity.
#if UNITY_EDITOR
        // Esto detiene la reproducción en el Editor.
        EditorApplication.isPlaying = false;
#else
        // Esta es la función para salir en una aplicación compilada (PC, Móvil, etc.).
        Application.Quit();
#endif
        Debug.Log("Saliendo del juego..."); // Mensaje que solo verás en el Editor
    }
}