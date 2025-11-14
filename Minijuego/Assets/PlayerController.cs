using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5.0f;      // Velocidad normal del jugador
    public float sprintSpeed = 10.0f;   // Velocidad al correr

    private Rigidbody rb;               // Referencia al Rigidbody para manejar la física
    private Animator animator;          // Referencia al Animator para controlar animaciones
    private GameManager gameManager;    // Referencia al GameManager para aplicar penalizaciones

    void Start()
    {
        // Obtengo componentes necesarios al iniciar la escena
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        //gameManager = FindObjectOfType<GameManager>();

        // Aseguro que el jugador comience sin velocidad acumulada
        rb.velocity = Vector3.zero;
    }

    void FixedUpdate()
    {
        // Leo el input del jugador (movimiento horizontal y vertical)
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Verifico si el jugador está corriendo
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        float currentMaxSpeed = isSprinting ? sprintSpeed : walkSpeed;

        // Genero la dirección de movimiento basada en el input
        Vector3 direction = transform.forward * verticalInput + transform.right * horizontalInput;
        Vector3 movement = direction.normalized * currentMaxSpeed;

        // Aplico movimiento mediante la velocidad del Rigidbody
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

        // Magnitud de la velocidad horizontal para usar en la animación
        float currentMagnitude = new Vector2(rb.velocity.x, rb.velocity.z).magnitude;

        Debug.Log("Velocidad: " + currentMagnitude);

        // Normalizo la velocidad para que la animación vaya de 0 a 1
        float normalizedSpeed = Mathf.Clamp(currentMagnitude / sprintSpeed, 0f, 1f);

        // Actualizo el valor de "Speed" en el Animator
        if (animator != null)
        {
            animator.SetFloat("Speed", normalizedSpeed);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Verifico si el choque es contra un objeto con la etiqueta "Obstacle"
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Rigidbody rb = GetComponent<Rigidbody>();

            // Evito que el Rigidbody genere rotaciones indeseadas al chocar
            rb.angularVelocity = Vector3.zero;

            // Detengo la velocidad horizontal para evitar rebotes
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);

            // Aplico penalización si existe un GameManager cargado
            if (gameManager != null)
            {
                gameManager.ApplyTimePenalty(3.0f);
            }
        }
    }
}
