// ItemCollectable.cs - USA EL MÉTODO DE DETECCIÓN IS TRIGGER
using UnityEngine;

public class ItemCollectable : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // *** ESTA FUNCIÓN SE DISPARA CUANDO UN OBJETO MARCA 'IS TRIGGER' ES ATRAVESADO ***
    private void OnTriggerEnter(Collider other)
    {
        // 1. Verificar el Tag del Jugador
        if (other.CompareTag("Player"))
        {
            if (gameManager != null)
            {
                // Llama a la función que suma score, hace el spawn y destruye este ítem
                gameManager.CollectAndSpawnNewItem(); 
            }

            // 2. Destruir el ítem que fue recolectado
            Destroy(gameObject); 
        }
    }
    
    // NOTA: La función OnCollisionEnter debe ser ELIMINADA o COMENTADA en este script.
}