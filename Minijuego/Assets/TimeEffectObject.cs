using UnityEngine;

public class TimeEffectObject : MonoBehaviour
{
    [Tooltip("Tiempo añadido o restado al temporizador (ej. 10 para Moneda, -5 para Cono).")]
    public float timeDelta = 10f; 

    // Referencia al Level2Manager para acceder al temporizador
    private Level2Manager levelManager;

    void Start()
    {
        // Encuentra automáticamente la instancia del Level2Manager en la escena
        levelManager = FindObjectOfType<Level2Manager>();
        
        if (levelManager == null)
        {
            Debug.LogError("TimeEffectObject no encontró el Level2Manager en la escena.");
        }
    }

    // Se activa cuando el camión (Player) entra en la zona del Trigger
    private void OnTriggerEnter(Collider other)
    {
        // Asume que el camión tiene el Tag "Player"
        if (other.CompareTag("Player") && levelManager != null) 
        {
            // Llama a la función del manager para aplicar el efecto de tiempo
            levelManager.ApplyTimeEffect(timeDelta);

            // Destruye el objeto para que no pueda ser recogido de nuevo
            Destroy(gameObject); 
        }
    }
}