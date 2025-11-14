// MenuManager.cs
using UnityEngine;
using UnityEngine.SceneManagement; 

public class MenuManager : MonoBehaviour
{
    // Función para iniciar el juego (desde el Menu Principal)
    public void StartGame()
    {
        // Carga la escena de juego que nombraste "Game"
        SceneManager.LoadScene("Game"); 
    }

    // Función para ir al menú de Opciones (desde Menu o Game)
    public void OpenOptions()
    {
        // Carga la escena de Opciones
        // NOTA: Debes crear una escena llamada 'Opciones' si aún no existe.
        SceneManager.LoadScene("Opciones");
    }
    
    // Función para volver al Menú Principal (desde Opciones o Game Over)
    public void GoToMainMenu()
    {
        // Carga la escena del Menú Principal
        SceneManager.LoadScene("Menu"); 
    }

    // Función para salir de la aplicación (usado en el Menu Principal)
    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}