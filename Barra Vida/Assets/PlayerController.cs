using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Variables públicas para ajustar la velocidad y la vida en el Inspector
    public float runSpeed = 5f; // Aumenté la velocidad para que se note más
    public float vida = 3f; 
    public float jumpSpeed = 7f;
    public bool enSuelo = false; 

    // Referencias privadas
    private Rigidbody rb; 

    void Start()
    {
        // Obtenemos el componente Rigidbody al inicio del juego
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // ===================================
        // CÓDIGO DE MOVIMIENTO (WASD)
        // ===================================

        // 1. Manejar el Input (Entrada de usuario)
        // GetAxis("Horizontal") devuelve -1 (A o Flecha Izquierda) a 1 (D o Flecha Derecha)
        float moveHorizontal = Input.GetAxis("Horizontal"); 
        // GetAxis("Vertical") devuelve -1 (S o Flecha Abajo) a 1 (W o Flecha Arriba)
        float moveVertical = Input.GetAxis("Vertical");   

        // 2. Calcular el vector de movimiento en el plano XZ (horizontal)
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // 3. Aplicar el movimiento al Rigidbody
        // MovePosition mueve el objeto suavemente usando la velocidad y el tiempo entre frames
        rb.MovePosition(transform.position + movement * runSpeed * Time.deltaTime);
        
        // 4. Manejar el Salto
        // GetButtonDown("Jump") es la barra espaciadora por defecto
        if (Input.GetButtonDown("Jump") && enSuelo) 
        {
            // Aplicar una fuerza hacia arriba (impulso)
            rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            //enSuelo = false; // Se recomienda que OnCollisionExit haga esto, pero puedes usarlo aquí.
        }
    }
    
    // --- LÓGICA DE FÍSICAS Y COLISIONES ---

    void OnCollisionEnter(Collision collision)
    {
        // Lógica de Daño
        if (collision.gameObject.CompareTag("Obstaculo")) 
        {
            vida -= 1; 
            if (vida <= 0)
            {
                Destroy(gameObject);
                Debug.Log("Game Over!");
                return;
            }
        }

        // Lógica de Suelo
        // Se recomienda usar el tag "Ground" o "Suelo" en tu plataforma
        if (collision.gameObject.CompareTag("Suelo"))
        {
            enSuelo = true;
        }
    }
    
    void OnCollisionExit(Collision collision)
    {
        // Lógica para dejar de estar en el suelo
        if (collision.gameObject.CompareTag("Suelo"))
        {
            enSuelo = false;
        }
    }
}   