// PlayerController.cs - Adjuntar al objeto "Player"
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 15.0f; 
    public float rotationSpeed = 360.0f; 

    private Rigidbody rb;
    private GameManager gameManager;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        // Rigidbody debe tener Freeze Rotation X y Z marcados en el Inspector.
        
        gameManager = FindObjectOfType<GameManager>();
    }

    void FixedUpdate() 
    {
        float horizontalInput = Input.GetAxis("Horizontal"); // Para girar
        float verticalInput = Input.GetAxis("Vertical");   // Para adelante/atrás

        // 1. APLICAR ROTACIÓN MANUAL (Giro con A/D)
        float turn = horizontalInput * rotationSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
        
        // 2. APLICAR TRASLACIÓN (Adelante/Atrás) - En el eje local del personaje
        Vector3 localMovement = transform.forward * verticalInput * moveSpeed;
        
        // Aplica velocidad de traslación
        rb.velocity = new Vector3(localMovement.x, rb.velocity.y, localMovement.z);
    }
    
    // 3. LÍNEA DE VIDA/PENALIZACIÓN (Al chocar con límites/estantes)
    private void OnCollisionEnter(Collision collision)
    {
        // Usa Tag "Obstacle" (Muros, Borders, Estantes)
        if (collision.gameObject.CompareTag("Obstacle")) 
        {
            if (gameManager != null)
            {
                gameManager.ApplyTimePenalty(3.0f); // Penaliza con 3 segundos
            }
            // Agrega un pequeño empujón de feedback
            Vector3 pushBack = (transform.position - collision.contacts[0].point).normalized * 0.2f;
            rb.position += pushBack; 
        }
    }
}