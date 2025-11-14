using UnityEngine;

public class FPSController : MonoBehaviour
{
    // --- Parámetros de movimiento ---
    public float walkSpeed = 5.0f;          // Velocidad base del jugador
    public float sprintSpeed = 10.0f;       // Velocidad al correr
    public float maxBounds = 14f;           // Límite para evitar salir del área jugable
    
    // --- Parámetros de cámara ---
    public Transform playerCamera;          // Cámara principal asignada desde el Inspector
    public float mouseSensitivity = 100f;   // Sensibilidad del movimiento del ratón
    public float minYAngle = -60f;          // Límite inferior al mirar hacia abajo
    public float maxYAngle = 60f;           // Límite superior al mirar hacia arriba
    
    // --- Componentes del objeto ---
    private Rigidbody rb;                   // Rigidbody para manejar física
    private Animator animator;              // Controlador de animaciones
    private float xRotation = 0f;           // Rotación acumulada en el eje X para el control vertical
    private GameManager gameManager;        // Referencia al GameManager para penalizaciones

    void Start()
    {
        // Inicializar referencias
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();

        // Bloquear cursor al centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
        
        // Asegurar que el jugador no inicie con velocidad residual
        rb.velocity = Vector3.zero;
    }

    void Update()
{
    // 1. CHEQUEO PRINCIPAL: Si el juego está pausado o el script está desactivado, ¡sal inmediatamente!
    // Nota: 'this.enabled' ya debería ser falso si llamaste a ReleaseCursorAndDisable().
    if (Time.timeScale == 0 || !this.enabled) 
    {
        return;
    }

    // 2. Lógica de Bloqueo de Cursor
    // Si el cursor está visible, bloquéalo de nuevo (para el modo FPS)
    if (Cursor.lockState != CursorLockMode.Locked) 
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // 3. Lógica de Rotación de Cámara (Mouse Look)
    
    // Rotación horizontal del cuerpo del jugador
    float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
    transform.Rotate(Vector3.up * mouseX);

    // Rotación vertical de la cámara
    float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
    xRotation -= mouseY;

    // Limitar la rotación vertical dentro de los ángulos permitidos
    xRotation = Mathf.Clamp(xRotation, minYAngle, maxYAngle);

    // Aplicar la rotación calculada a la cámara
    if (playerCamera != null)
    {
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}

    void FixedUpdate()
    {
        // --- Movimiento físico del jugador ---

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        bool isMoving = Mathf.Abs(horizontalInput) > 0.01f || Mathf.Abs(verticalInput) > 0.01f;
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);

        // Determinar velocidad máxima según si el jugador está corriendo
        float currentMaxSpeed = isSprinting ? sprintSpeed : walkSpeed;

        if (isMoving)
        {
            // Determinar dirección en función de hacia dónde mira el jugador
            Vector3 direction = transform.forward * verticalInput + transform.right * horizontalInput;
            Vector3 movement = direction.normalized * currentMaxSpeed;

            // Aplicar movimiento al Rigidbody
            rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
        }
        else
        {
            // Detener movimiento horizontal cuando no hay entrada del usuario
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
        }

        // --- Reforzar límites del escenario ---
        // Previene que el jugador salga del área jugable incluso si la colisión falla.
        Vector3 currentPos = transform.position;
        currentPos.x = Mathf.Clamp(currentPos.x, -maxBounds, maxBounds);
        currentPos.z = Mathf.Clamp(currentPos.z, -maxBounds, maxBounds);
        transform.position = currentPos;

        // --- Actualización de animaciones ---
        // Normalizar velocidad para usar en el Blend Tree del Animator
        float currentMagnitude = new Vector2(rb.velocity.x, rb.velocity.z).magnitude;
        float normalizedSpeed = Mathf.Clamp(currentMagnitude / sprintSpeed, 0f, 1f);

        if (animator != null)
        {
            animator.SetFloat("Speed", normalizedSpeed);
        }
    }

    public void ReleaseCursorAndDisable()
{
    // 1. Desbloquear el cursor y hacerlo visible para interactuar con la UI
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;

    // 2. Desactivar este script para detener el movimiento del jugador y la cámara
    this.enabled = false;
    
    // Opcional: Asegurar que el Rigidbody se detenga
    if (rb != null)
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}

    void OnCollisionEnter(Collision collision)
    {
        // --- Anular rotación producida por colisiones ---
        // Esto mantiene estable la cámara evitando giros involuntarios.
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.angularVelocity = Vector3.zero;

        // --- Comportamiento específico con obstáculos ---
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            // Detener movimiento horizontal al chocar contra un muro
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);

            // Aplicar penalización desde el GameManager
            if (gameManager != null)
            {
                gameManager.ApplyTimePenalty(3.0f);
            }
        }
    }
}
