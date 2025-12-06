using UnityEngine;

public class TruckController : MonoBehaviour
{
    // --- Parámetros de Movimiento ---
    [Header("Parámetros del Vehículo")]
    public float maxSpeed = 10.0f;           // Velocidad máxima al caminar (en Unity Units/s)
    public float turnSpeed = 70.0f;          // Velocidad de giro (grados por segundo)
    
    // El límite se puede dejar, pero es más limpio usar Colliders en los bordes.
    public float maxBoundsX = 14f;           // Límite lateral del camino (Eje X)
    
    // --- Referencias ---
    private Rigidbody rb;
    private Level2Manager levelManager;      // Usaremos este para las penalizaciones

    void Start()
    {
        // Obtener el Rigidbody
        rb = GetComponent<Rigidbody>();
        
        // Buscar el GameManager del nivel 2 para la interacción (Conos/Monedas)
        levelManager = FindObjectOfType<Level2Manager>();

        if (rb == null)
        {
            Debug.LogError("El TruckController requiere un Rigidbody.");
            enabled = false;
        }
    }

    void FixedUpdate()
    {
        // 1. Obtener Entradas
        float verticalInput = Input.GetAxis("Vertical");      // W/S o Flechas Arriba/Abajo
        float horizontalInput = Input.GetAxis("Horizontal");  // A/D o Flechas Izquierda/Derecha

        // ********* MODIFICACIÓN CLAVE PARA QUITAR LA REVERSA *********
        // Aseguramos que la entrada vertical sea siempre cero o positiva.
        // Si el jugador presiona S o Flecha Abajo (verticalInput < 0), se convierte en 0.
        float forwardInput = Mathf.Max(0f, verticalInput); 
        // *************************************************************

        // 2. Control de Rotación (Giro)
        // La rotación se aplica sobre el eje Y (vertical)
        // Nota: La capacidad de girar no se ve afectada por el movimiento
        float turn = horizontalInput * turnSpeed * Time.fixedDeltaTime;
        transform.Rotate(0, turn, 0);

        // 3. Control de Movimiento (Solo Avanzar)
        // Usamos transform.forward para movernos en la dirección a la que apunta el camión.
        // Usamos 'forwardInput' que es siempre positivo o cero.
        Vector3 movement = transform.forward * forwardInput * maxSpeed;

        // Aplicar la velocidad. Mantenemos rb.velocity.y (gravedad)
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
        
        // 4. Reforzar Límites del Escenario (Método de Clamp)
        // Aunque es mejor usar colliders, este es tu método de seguridad.
        Vector3 currentPos = transform.position;
        currentPos.x = Mathf.Clamp(currentPos.x, -maxBoundsX, maxBoundsX);
        transform.position = currentPos;
    }

    // --- Lógica de Colisión (Para penalizaciones) ---
    void OnCollisionEnter(Collision collision)
    {
        // Anular rotación/estabilidad. Evita que el camión vuelque fácilmente
        if (rb != null)
        {
            rb.angularVelocity = Vector3.zero;
        }

        // Comportamiento al chocar contra un obstáculo rígido
        if (collision.gameObject.CompareTag("Obstacle") && levelManager != null)
        {
            // Opcional: Aplicar un pequeño rebote o detención
            rb.velocity = Vector3.zero; 
            
            // Aplicar penalización desde el Level2Manager (asumiendo que tiene la función)
            levelManager.ApplyTimeEffect(-3.0f); 
            Debug.Log("Choque con obstáculo. Penalización de tiempo aplicada.");
        }
    }
}