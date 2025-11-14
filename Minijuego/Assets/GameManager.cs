using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

// Clase auxiliar para definir posiciones de muebles desde el Inspector
[System.Serializable]
public class FurnitureSpawn
{
    public int prefabIndex;            // Índice del prefab dentro del array
    public Vector3 position;           // Posición donde se colocará
    public Vector3 rotation;           // Rotación en Euler
}

public class GameManager : MonoBehaviour
{
    // PlayerPrefs: claves usadas para cargar opciones del jugador
    private const string DIFFICULTY_KEY = "GameDifficulty";
    private const string VOLUME_KEY = "MasterVolume";

    // --- Referencias asignadas desde el Inspector ---
    public GameObject collectiblePrefab;          // Ítem a recolectar
    public TextMeshProUGUI timerText;             // Texto del tiempo restante
    public TextMeshProUGUI scoreText;             // Texto de ítems recolectados
    public GameObject gameOverPanel;              // Panel de Game Over
    public TextMeshProUGUI gameOverText;          // Texto de resultado final
    public AudioSource backgroundMusicSource;     // Música del nivel
    public FPSController playerController;        // Referencia al controlador del jugador

    // Límites para spawnear ítems y muebles al azar
    public float spawnRangeX = 7f;
    public float spawnRangeZ = 7f;
    public float spawnHeight = 0.5f;

    // Prefabs de muebles y posiciones configurables
    public GameObject[] furniturePrefabs;
    public FurnitureSpawn[] levelFurnitureLayout;

    // --- Estado del juego ---
    private float timeRemaining;
    private bool isGameOver = false;
    private int itemsCollected = 0;
    private int totalItemsToCollect;

    private const string NEXT_LEVEL_NAME = "Game2";

    void Start()
    {
        Time.timeScale = 1;  // Garantiza que el juego empiece sin pausa

        // Detiene música persistente del menú si existe
        MusicManager persistentManager = FindObjectOfType<MusicManager>();
        if (persistentManager != null)
        {
            AudioSource persistentSource = persistentManager.GetComponent<AudioSource>();
            if (persistentSource != null && persistentSource.isPlaying)
                persistentSource.Stop();
        }

        // Cargar configuraciones guardadas por el jugador
        LoadDifficultySettings();
        LoadVolumeSettings();

        // Elimina muebles del nivel anterior (si quedaron en escena)
        DestroyStaticFurniture();

        // Generar muebles al azar para formar la estructura del nivel
        SpawnRandomFurniture(20);

        // Ocultar panel de Game Over
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // Iniciar música del nivel
        if (backgroundMusicSource != null && !backgroundMusicSource.isPlaying)
            backgroundMusicSource.Play();

        // Crear el primer objeto coleccionable
        SpawnRandomItem();
    }

    void Update()
    {
        if (isGameOver) return;

        // Contador regresivo del tiempo
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            timeRemaining = 0;
            GameOver(false);   // Se termina el juego por tiempo
        }

        UpdateUI();  // Refresca textos de tiempo e ítems
    }

    // -------------------------------------
    //      CONFIGURACIONES DEL JUGADOR
    // -------------------------------------

    private void LoadDifficultySettings()
    {
        // Recuperar dificultad guardada (0,Fácil — 1,Medio — 2,Difícil)
        int difficulty = PlayerPrefs.GetInt(DIFFICULTY_KEY, 0);

        // Ajustar tiempo total e ítems según dificultad
        switch (difficulty)
        {
            case 0: totalItemsToCollect = 5; timeRemaining = 90f; break;
            case 1: totalItemsToCollect = 7; timeRemaining = 60f; break;
            case 2: totalItemsToCollect = 10; timeRemaining = 30f; break;
        }
    }

    private void LoadVolumeSettings()
    {
        if (PlayerPrefs.HasKey(VOLUME_KEY))
            AudioListener.volume = PlayerPrefs.GetFloat(VOLUME_KEY);
    }

    // -------------------------------------
    //              SPAWN
    // -------------------------------------

    private void SpawnRandomFurniture(int count)
    {
        if (furniturePrefabs == null || furniturePrefabs.Length == 0)
        {
            Debug.LogWarning("No hay prefabs de muebles asignados.");
            return;
        }

        // Instancia muebles al azar para formar pasillos y obstáculos
        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, furniturePrefabs.Length);
            GameObject prefabToSpawn = furniturePrefabs[randomIndex];

            float randomX = Random.Range(-spawnRangeX, spawnRangeX);
            float randomZ = Random.Range(-spawnRangeZ, spawnRangeZ);

            Vector3 randomPosition = new Vector3(randomX, spawnHeight, randomZ);
            Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

            Instantiate(prefabToSpawn, randomPosition, randomRotation);
        }
    }

    private void SpawnRandomItem()
    {
        if (collectiblePrefab == null) return;

        // Coloca el ítem dentro del rango de juego
        float x = Random.Range(-spawnRangeX, spawnRangeX);
        float z = Random.Range(-spawnRangeZ, spawnRangeZ);

        Vector3 pos = new Vector3(x, spawnHeight, z);

        Instantiate(collectiblePrefab, pos, collectiblePrefab.transform.rotation);
    }

    // -------------------------------------
    //          ACTUALIZACIÓN DE UI
    // -------------------------------------

    private void UpdateUI()
    {
        timerText.text = "Tiempo: " + Mathf.Ceil(timeRemaining).ToString("F0");
        scoreText.text = "Ítems: " + itemsCollected + "/" + totalItemsToCollect;
    }

    // -------------------------------------
    //            LÓGICA DE ÍTEMS
    // -------------------------------------

    public void CollectAndSpawnNewItem()
    {
        itemsCollected++;

        // Si aún faltan ítems, generar otro
        if (itemsCollected < totalItemsToCollect)
        {
            SpawnRandomItem();
        }
        else
        {
            // Si ya juntó todos, avanzar al siguiente nivel
            NextLevel();
        }

        UpdateUI();
    }

    public void ApplyTimePenalty(float seconds)
    {
        // Resta tiempo cuando el jugador choca con un obstáculo
        timeRemaining -= seconds;

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            GameOver(false);
        }
    }

    // -------------------------------------
    //              GAME OVER
    // -------------------------------------

    private void GameOver(bool win)
    {
     if (playerController != null)
    {
        playerController.ReleaseCursorAndDisable(); 
    }
        isGameOver = true;
        Time.timeScale = 0;  // Pausa el juego

        if (backgroundMusicSource != null)
            backgroundMusicSource.Stop();

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            if (gameOverText != null)
            {
                gameOverText.text = win ?
                    "¡Misión Cumplida!" :
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

        // Si existe el siguiente nivel en Build Settings…
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            // Si ya no hay más niveles → victoria final
            GameOver(true);
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
    if (playerController != null)
    {
        // 1. Liberar y deshabilitar el controlador inmediatamente
        playerController.ReleaseCursorAndDisable();
    }

    Debug.Log("Cargando Menú...");
    
    // 2. Restaurar el tiempo
    Time.timeScale = 1; 
    
    // 3. ¡Iniciar la carga de escena de forma diferida!
    StartCoroutine(LoadMenuSceneDelayed());
}
private IEnumerator LoadMenuSceneDelayed()
{
    // Esperar un frame. Esto permite que el sistema de Input de Unity 
    // complete el desbloqueo del cursor y la UI tome el foco.
    yield return null; 
    
    // Cargar la escena "Menu"
    SceneManager.LoadScene("Menu");
}

    // -------------------------------------
    //        LIMPIEZA DE NIVEL ANTERIOR
    // -------------------------------------

    private void DestroyStaticFurniture()
    {
        // Limpia muebles con tag especial (ej.: los del menú anterior)
        GameObject[] staticFurniture = GameObject.FindGameObjectsWithTag("StaticFurniture");

        foreach (GameObject furniture in staticFurniture)
            Destroy(furniture);

        if (staticFurniture.Length == 0)
            Debug.LogWarning("No se encontraron objetos con el tag 'StaticFurniture'.");
    }
}
