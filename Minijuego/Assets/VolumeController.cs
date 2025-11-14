using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    // Claves para PlayerPrefs (deben coincidir con GameManager)
    private const string DIFFICULTY_KEY = "GameDifficulty";
    private const string VOLUME_KEY = "MasterVolume";

    public Slider volumeSlider;

    void Start()
    {
        // Asegura que el slider muestre el valor guardado
        if (volumeSlider != null)
        {
            float savedVolume = PlayerPrefs.GetFloat(VOLUME_KEY, 1.0f);
            volumeSlider.value = savedVolume;

            // Aplica inmediatamente el volumen guardado
            AudioListener.volume = savedVolume;
        }
    }

    // Llamado automáticamente por el slider
    public void SetMasterVolume(float volume)
    {
        // 1. Aplicar volumen global
        AudioListener.volume = volume;

        // 2. Guardar valor
        PlayerPrefs.SetFloat(VOLUME_KEY, volume);
        PlayerPrefs.Save();
    }

    // Para botones de dificultad
    public void SetDifficulty(int difficultyLevel)
    {
        // 0 = Fácil, 1 = Medio, 2 = Difícil
        PlayerPrefs.SetInt(DIFFICULTY_KEY, difficultyLevel);
        PlayerPrefs.Save();
    }

    // Botón de regresar
    public void BackToMenu(string sceneName)
    {
        Time.timeScale = 1; 
        SceneManager.LoadScene(sceneName);
    }
}
