using UnityEngine;
using UnityEngine.SceneManagement; 

public class MenuPrincipal : MonoBehaviour
{
    // 1. REFERENCIA: Asigna el objeto TransitionManager aquí en el Inspector.
    public SceneTransitioner transitionManager; 

    // Usaremos los nombres de escena exactos para el StartTransition
    public void Jugar()
    {
        Debug.Log("Cargando el juego...");
        // 2. LLAMA AL TRANSITIONER
        transitionManager.StartTransition("Juego"); 
    }

    public void Opciones()
    {
        Debug.Log("Cargando menú opciones...");
        // 2. LLAMA AL TRANSITIONER
        transitionManager.StartTransition("Opciones"); 
    }

    // Nota: La función Salir() puede quedarse con Application.Quit() si no necesitas un fade.
    // ...
}