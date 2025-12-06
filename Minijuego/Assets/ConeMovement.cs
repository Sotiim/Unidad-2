using UnityEngine;

public class ConeMovement : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    // Define la velocidad a la que se mueve el cono
    public float moveSpeed = 1.0f; 
    
    // Define la magnitud del movimiento lateral (solo para movimiento sinusoidal)
    public float amplitudeX = 2.0f; 
    
    // Define la frecuencia del movimiento lateral (solo para movimiento sinusoidal)
    public float frequencyX = 1.0f; 
    
    // Almacena la posición inicial para calcular el desplazamiento
    private Vector3 initialPosition; 

    void Start()
    {
        // Guardamos la posición inicial del cono al ser instanciado
        initialPosition = transform.position;
    }

    void Update()
    {
        // Llama a la función de movimiento que desees
        MoveSideways(); 
    }

    // 1. Movimiento Sinusoidal (de lado a lado)
    void MoveSideways()
    {
        // Calcula un desplazamiento en X basado en una onda sinusoidal
        float offsetX = Mathf.Sin(Time.time * frequencyX) * amplitudeX;
        
        // Aplica el desplazamiento a la posición inicial, manteniendo Y y Z fijos
        transform.position = initialPosition + new Vector3(offsetX, 0, 0);
    }
    
}