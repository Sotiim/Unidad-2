using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic; // Necesario para usar la lista de posiciones

public class Level2Manager : MonoBehaviour
{
    private const string DIFFICULTY_KEY = "GameDifficulty";

    // --- Configuración de Nivel ---
    [Header("Configuración del Nivel 2")]
    public float timeLimit = 60f; // Tiempo inicial del desafío de entrega
    private float timeRemaining;
    private bool isLevelActive = true;

    // --- Configuración de Spawn ---
    [Header("Configuración de Spawn")]
    public GameObject conePrefab; // ¡ASIGNAR! Prefab del Cono (resta tiempo)
    public int numberOfConesToSpawn = 10;

    [Header("Configuración de Monedas")]
    public GameObject coinPrefab; // ¡ASIGNAR! Prefab de la Moneda (suma tiempo)
    public int numberOfCoinsToSpawn = 5;

    [Header("Parámetros de Área")]
    public float spawnRangeX = 3f; // Ancho máximo del área de spawn (lateral)
    public float spawnRangeZ = 15f; // Largo máximo del área de spawn (hacia adelante)
    public float spawnHeight = 0.5f; // Altura sobre el piso

    [Header("Control de Espaciado")]
    public float minimumSpacing = 4.0f; // Distancia mínima requerida entre los objetos

    // --- Referencias UI ---
    [Header("Referencias UI")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public GameObject endLevelPanel;
    public TextMeshProUGUI endLevelText;

    // --- Referencias de Componentes ---
    [Header("Referencias de Componentes")]
    public AudioSource backgroundMusicSource;
    public TruckController truckController;

    void Start()
    {
        Time.timeScale = 1;
        timeRemaining = timeLimit;

        // Buscar el controlador del camión si no está asignado
        if (truckController == null)
        {
            truckController = FindObjectOfType<TruckController>();
        }

        // --- Lógica de Inicio ---
        LoadVolumeSettings();

        // ¡LLAMADAS A LAS FUNCIONES DE SPAWN!
        SpawnItemsWithSpacing();

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
            EndLevel(false); // Se termina el nivel por tiempo agotado
        }

        UpdateUI();
    }

    // -------------------------------------
    //       ACTUALIZACIÓN DE UI
    // -------------------------------------

    private void UpdateUI()
    {
        if (timerText != null)
        {
            timerText.text = "Tiempo: " + Mathf.Ceil(timeRemaining).ToString("F0");
        }

        if (scoreText != null)
        {
            scoreText.text = "Objetivo: Entregar la carga";
        }
    }

    // -------------------------------------
    //          LÓGICA DE SPAWN CON ESPACIADO
    // -------------------------------------

    private void SpawnItemsWithSpacing()
    {
        // Generamos todos los objetos (conos y monedas) en una sola pasada 
        // para asegurarnos de que el espaciado se aplique entre TODOS ellos.

        List<Vector3> spawnedPositions = new List<Vector3>();

        // 1. Generar Conos
        GenerateItemSet(conePrefab, numberOfConesToSpawn, spawnedPositions);

        // 2. Generar Monedas
        GenerateItemSet(coinPrefab, numberOfCoinsToSpawn, spawnedPositions);
    }

    private void GenerateItemSet(GameObject prefab, int count, List<Vector3> existingPositions)
    {
        if (prefab == null)
        {
            Debug.LogError($"No se asignó el Prefab para {prefab.name} en el Level2Manager.");
            return;
        }

        int itemsSpawned = 0;
        int attempts = 0;
        const int maxAttemptsPerItem = 50;

        while (itemsSpawned < count && attempts < count * maxAttemptsPerItem)
        {
            attempts++;

            float randomX = Random.Range(-spawnRangeX, spawnRangeX);
            float randomZ = Random.Range(0f, spawnRangeZ);
            Vector3 spawnPosition = new Vector3(randomX, spawnHeight, randomZ);

            bool positionIsValid = true;

            // Verificar espaciado contra los objetos ya generados
            foreach (Vector3 existingPos in existingPositions)
            {
                float distance = Vector2.Distance(
                    new Vector2(existingPos.x, existingPos.z),
                    new Vector2(spawnPosition.x, spawnPosition.z)
                );

                if (distance < minimumSpacing)
                {
                    positionIsValid = false;
                    break;
                }
            }

            // Si la posición es válida, instanciar el objeto
            if (positionIsValid)
            {
                Quaternion spawnRotation = prefab.transform.rotation;
                Instantiate(prefab, spawnPosition, spawnRotation);

                existingPositions.Add(spawnPosition);
                itemsSpawned++;
                attempts = 0;
            }
        }

        if (itemsSpawned < count)
        {
            Debug.LogWarning($"Solo se pudieron generar {itemsSpawned} objetos de {count} ({prefab.name}) debido al espaciado estricto.");
        }
    }


    // -------------------------------------
    //            LÓGICA DE TIEMPO
    // -------------------------------------

    public void ApplyTimeEffect(float timeChange)
    {
        if (!isLevelActive) return;

        timeRemaining += timeChange;

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            EndLevel(false);
            return;
        }

        UpdateUI();
    }

    // -------------------------------------
    //          FIN DE NIVEL
    // -------------------------------------

    public void WinLevel()
    {
        if (isLevelActive)
        {
            NextLevel();
        }
    }

    private void EndLevel(bool win)
    {
        if (truckController != null)
        {
            truckController.enabled = false;
        }

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
                    "¡Entrega Exitosa! Tiempo restante: " + Mathf.Ceil(timeRemaining).ToString("F0") + "s" :
                    "¡Tiempo Agotado! Game Over";
            }
        }
    }
    // -------------------------------------
    //        PROGRESIÓN ENTRE NIVELES
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
        if (truckController != null)
        {
            truckController.enabled = false;
        }
        Time.timeScale = 1;
        StartCoroutine(LoadMenuSceneDelayed());
    }

    private IEnumerator LoadMenuSceneDelayed()
    {
        yield return null;
        SceneManager.LoadScene("Menu");
    }

    // -------------------------------------
    //       LÓGICA DE CARGA/LIMPIEZA
    // -------------------------------------

    private void LoadVolumeSettings()
    {
        const string VOLUME_KEY = "MasterVolume";
        if (PlayerPrefs.HasKey(VOLUME_KEY))
            AudioListener.volume = PlayerPrefs.GetFloat(VOLUME_KEY);
    }

}