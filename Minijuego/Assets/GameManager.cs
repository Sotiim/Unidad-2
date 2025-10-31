// GameManager.cs - Adjuntar al objeto "GameManager"
using UnityEngine;
using TMPro; 
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour
{
    // Conectar en el Inspector
    public GameObject collectiblePrefab; 
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;

    // Límites de la caja de juego (10x10)
    public float spawnRangeX = 4.5f; 
    public float spawnRangeZ = 4.5f; 
    public float spawnHeight = 0.5f; // Altura del suelo (ajusta si es necesario)

    // Variables de Estado y Score
    private float timeRemaining = 60.0f; 
    private bool isGameOver = false;
    private int itemsCollected = 0; 
    public int totalItemsToCollect = 5; 

    void Update()
    {
        if (isGameOver) return;

        // MANEJO DEL TIEMPO (Línea de Vida)
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            timeRemaining = 0;
            GameOver(false); 
        }

        UpdateUI();
    }

    void Start()
    {
        // Generar el primer ítem al inicio del juego
        SpawnRandomItem(); 
    }

    private void UpdateUI()
    {
        timerText.text = "Tiempo: " + Mathf.Ceil(timeRemaining).ToString("F0");
        scoreText.text = "Ítems: " + itemsCollected + "/" + totalItemsToCollect;
    }

    // LLAMADA DESDE ItemCollectable.cs (al chocar)
    public void CollectAndSpawnNewItem()
    {
        itemsCollected++;
        
        if (itemsCollected < totalItemsToCollect)
        {
            SpawnRandomItem(); 
        }
        else
        {
            GameOver(true); // Gana al llegar al total
        }

        UpdateUI();
    }

    // LLAMADA DESDE PlayerController.cs (al chocar con Obstáculo)
    public void ApplyTimePenalty(float seconds)
    {
        timeRemaining -= seconds;
        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            GameOver(false); 
        }
    }

    // FUNCIÓN PARA GENERAR EL ÍTEM AL AZAR
    private void SpawnRandomItem()
    {
        if (collectiblePrefab == null) return;
        
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        float randomZ = Random.Range(-spawnRangeZ, spawnRangeZ);
        
        Vector3 randomPosition = new Vector3(randomX, spawnHeight, randomZ);

        // Instancia el nuevo ítem
        Instantiate(collectiblePrefab, randomPosition, collectiblePrefab.transform.rotation);
    }
    
    // FUNCIÓN DE FIN DE JUEGO
    private void GameOver(bool win)
    {
        isGameOver = true;
        Time.timeScale = 0; // Detiene el juego
        
        if (win)
        {
            Debug.Log("¡Juego Ganado!");
        }
        else
        {
            Debug.Log("¡Game Over!");
        }
    }
}