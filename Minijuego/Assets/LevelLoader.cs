using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para gestionar escenas

public class LevelLoader : MonoBehaviour
{
    // Nombre de la escena del segundo nivel (debe coincidir con el nombre del archivo de escena)
    public string nextLevelName = "Nivel2_Entrega"; 

    // Se llama cuando el jugador toca la "Meta" del Nivel 1
    public void LoadNextLevel()
    {
        Debug.Log("Cargando el siguiente nivel: " + nextLevelName);
        
        // Carga la escena por su nombre
        SceneManager.LoadScene(nextLevelName);
    }
}