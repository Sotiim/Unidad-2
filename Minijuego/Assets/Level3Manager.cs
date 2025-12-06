using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class Level3Manager : MonoBehaviour
{
    // Clave para cargar la dificultad, debe coincidir con el menú
    private const string DIFFICULTY_KEY = "GameDifficulty";
    private const string VOLUME_KEY = "MasterVolume";

    // --- Configuración de Nivel ---
    [Header("Configuración de Nivel 3")]
    public float timeLimit = 60f;
    private float timeRemaining;
    private bool isLevelActive = true;

    // --- Referencias UI ---
    [Header("Referencias UI")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public GameObject endLevelPanel;
    public TextMeshProUGUI endLevelText;

    // --- Referencias de Componentes ---
    [Header("Referencias de Componentes")]
    public AudioSource backgroundMusicSource;
    public CraneController craneController; // Se usa para aplicar la velocidad de la grúa
    public ZonaEstiba zonaEstiba; // Se usa para decirle a la meta cuántos ítems se necesitan

    void Start()
    {
        Time.timeScale = 1;

        // LLAMADAS CLAVE DE CONFIGURACIÓN
        LoadDifficultySettings();
        LoadVolumeSettings();

        // Asignar la referencia del LevelManager a la Grúa, si no está hecha en el Inspector
        if (craneController != null && craneController.levelManager == null)
        {
            craneController.levelManager = this;
        }

        // Ocultar panel de Fin de Nivel
        if (endLevelPanel != null)
            endLevelPanel.SetActive(false);

        // Iniciar música del nivel
        if (backgroundMusicSource != null && !backgroundMusicSource.isPlaying)
            backgroundMusicSource.Play();

        UpdateUI();
    }

    void Update()
    {
        if (!isLevelActive) return;

        // Contador regresivo del tiempo
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            timeRemaining = 0;
            EndLevel(false); // Game Over por tiempo
        }

        UpdateUI();
    }

    public void ApplyTimeBonus(float bonusAmount)
    {
        if (isLevelActive)
        {
            timeRemaining += bonusAmount;
            // Asegura que el tiempo no se vuelva negativo si ya está cerca de cero.
            timeRemaining = Mathf.Max(0, timeRemaining);
            Debug.Log($"[Level3Manager] Tiempo Bonificado: +{bonusAmount}s. Nuevo tiempo: {Mathf.Ceil(timeRemaining)}s");
        }
    }

    // NUEVA FUNCIÓN: Permite a la grúa saber si puede moverse o no.
    public bool IsLevelActive()
    {
        return isLevelActive;
    }

    // -------------------------------------
    //      CONFIGURACIONES DEL JUGADOR
    // -------------------------------------

    private void LoadDifficultySettings()
    {
        // 1. Obtener el valor de dificultad (3, 5, o 7). El default es 5 (Intermedio)
        int difficulty = PlayerPrefs.GetInt(DIFFICULTY_KEY, 5);

        // --- BASE DE PARÁMETROS DEL NIVEL 3 ---
        float baseTime = 90f; // Tiempo base máximo
        int itemsToStack = 0;

        // --- PARÁMETROS ESPECÍFICOS DE DIFICULTAD ---
        float speedLevel3 = 8f;
        float speedLevel5 = 12f;
        float speedLevel7 = 18f;

        switch (difficulty)
        {
            case 3: // FÁCIL
                baseTime = 90f; itemsToStack = 5;
                ApplyCraneSpeed(speedLevel3);
                break;

            case 5: // INTERMEDIO
                baseTime = 60f; itemsToStack = 7;
                ApplyCraneSpeed(speedLevel5);
                break;

            case 7: // DIFÍCIL
                baseTime = 30f; itemsToStack = 10;
                ApplyCraneSpeed(speedLevel7);
                break;

            default: // Valor por defecto
                baseTime = 60f; itemsToStack = 7;
                ApplyCraneSpeed(speedLevel5);
                break;
        }

        // 2. APLICAR TIEMPO
        timeLimit = baseTime;
        timeRemaining = timeLimit;

        // 3. APLICAR REQUISITO DE ÍTEMS A LA META (ZonaEstiba)
        if (zonaEstiba != null)
        {
            zonaEstiba.SetRequiredItems(itemsToStack);
        }
    }

    // Función auxiliar para aplicar la velocidad a la grúa
    public void ApplyCraneSpeed(float speed)
    {
        if (craneController != null)
        {
            craneController.moveSpeed = speed;
        }
    }

    private void LoadVolumeSettings()
    {
        if (PlayerPrefs.HasKey(VOLUME_KEY))
            AudioListener.volume = PlayerPrefs.GetFloat(VOLUME_KEY);
    }

    // -------------------------------------
    //       ACTUALIZACIÓN DE UI
    // -------------------------------------

    private void UpdateUI()
    {
        if (timerText != null)
        {
            timerText.text = "Tiempo: " + Mathf.Ceil(timeRemaining).ToString("F0");
        }

        // Mantenemos el scoreText simple ya que el conteo está en ZonaEstiba.cs
        if (scoreText != null)
        {
            scoreText.text = "Objetivo: Apilar la carga";
        }
    }

    // -------------------------------------
    //          FIN DE NIVEL (WinLevel)
    // -------------------------------------

    public void WinLevel()
    {
        if (isLevelActive)
        {
            EndLevel(true);
        }
    }

    private void EndLevel(bool win)
    {
        isLevelActive = false;
        Time.timeScale = 0; // Pausa el juego

        if (backgroundMusicSource != null)
            backgroundMusicSource.Stop();

        if (endLevelPanel != null)
        {
            endLevelPanel.SetActive(true);

            if (endLevelText != null)
            {
                endLevelText.text = win ?
                    "Entrega Exitosa! Tiempo restante: " + Mathf.Ceil(timeRemaining).ToString("F0") + "s" :
                    "Tiempo Agotado! Game Over";
            }
        }
    }

    // -------------------------------------
    //        PROGRESIÓN ENTRE NIVELES
    // -------------------------------------

    public void NextLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            EndLevel(true);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadMenu()
    {
        Time.timeScale = 1;
        StartCoroutine(LoadMenuSceneDelayed());
    }

    private IEnumerator LoadMenuSceneDelayed()
    {
        yield return null;
        SceneManager.LoadScene("Menu");
    }
}