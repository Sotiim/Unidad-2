using UnityEngine;

public class ItemCollectable : MonoBehaviour
{
    // Nombre del trigger usado en el Animator del jugador para reproducir la animación de recoger.
    private const string PICKUP_TRIGGER = "PickUpTrigger"; 
    
    private GameManager gameManager;        // Referencia al GameManager para actualizar puntuación y generar nuevos ítems
    private AudioSource pickupSoundSource;  // Reproducción del sonido de recolección

    void Start()
    {
        // Inicializar referencias necesarias
        gameManager = FindObjectOfType<GameManager>();
        pickupSoundSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificar que el objeto que entra al trigger sea el jugador
        if (other.CompareTag("Player"))
        {
            // Obtener el Animator del jugador para activar la animación de recolección
            Animator playerAnimator = other.GetComponent<Animator>();

            // Activar animación si el jugador tiene Animator asignado
            if (playerAnimator != null)
            {
                playerAnimator.SetTrigger(PICKUP_TRIGGER);
            }
            
            // Actualizar puntaje y generar un nuevo objeto antes de destruir este
            if (gameManager != null)
            {
                gameManager.CollectAndSpawnNewItem(); 
            }

            // --- Reproducción del sonido y eliminación del objeto ---

            if (pickupSoundSource != null && pickupSoundSource.clip != null)
            {
                // Reproducir el sonido de recolección
                pickupSoundSource.Play();

                // Ocultar modelo visual del objeto y desactivar colisión
                Renderer[] renderers = GetComponentsInChildren<Renderer>();
                foreach (Renderer renderer in renderers)
                {
                    renderer.enabled = false;
                }

                GetComponent<Collider>().enabled = false;

                // Destruir el objeto una vez que el sonido haya terminado
                float destroyTime = pickupSoundSource.clip.length;
                Destroy(gameObject, destroyTime);
            }
            else
            {
                // Si no hay sonido configurado, destruir de inmediato
                Destroy(gameObject); 
            }
        }
    }
}
