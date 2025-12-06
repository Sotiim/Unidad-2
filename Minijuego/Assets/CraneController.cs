using UnityEngine;
using System.Collections.Generic; // Necesario para la clase List

public class CraneController : MonoBehaviour
{
    // === Variables de Control del Movimiento y Físicas ===
    public Transform ejeX;
    public Transform ejeZ;
    public Transform gancho;

    public float moveSpeed = 8f;

    private FixedJoint joint;
    private Rigidbody ganchoRb;

    public Level3Manager levelManager; // Referencia al LevelManager para actualizar puntuación

    // === Variables de Lógica de Juego y Puntuación ===
    
    [Header("Referencias de Estibas y Prefabs")]
    // ASIGNAR EN EL INSPECTOR: Zona_De_Estiba (0) a (5)
    public List<GameObject> zonasDeEstiba = new List<GameObject>(); 
    // ASIGNAR EN EL INSPECTOR: Tus Collectible_Prefab_Lvl3 (0) a (5)
    // No es estrictamente necesario, pero ayuda a la organización.
    public List<GameObject> prefabsColeccionables = new List<GameObject>(); 

    private GameObject cargaAgarrada = null; // Rastrea la carga actual
    private int score = 0; // Puntuación inicial

    // Lista para rastrear las estibas que ya tienen un objeto (opcional, pero ayuda)
    private List<GameObject> estibasLlenas = new List<GameObject>(); 

    // ----------------------------------------------------------------------------------
    // Ciclo de Vida de Unity
    // ----------------------------------------------------------------------------------

    void Start()
    {
        ganchoRb = gancho.GetComponent<Rigidbody>();
        if (ganchoRb == null)
        {
            Debug.LogError("El Gancho debe tener un Rigidbody adjunto.");
        }
        Debug.Log("Juego iniciado. Puntuación actual: " + score);
    }

    void Update()
    {
        float speed = moveSpeed * Time.deltaTime;

        // Movimiento de los Ejes (X y Z)
        float horizontal = Input.GetAxis("Horizontal"); 
        float vertical = Input.GetAxis("Vertical"); 
        ejeZ.Translate(Vector3.forward * vertical * speed, Space.World); 
        ejeX.Translate(Vector3.right * horizontal * speed, Space.World); 

        // Movimiento del Gancho (Vertical)
        if (Input.GetKey(KeyCode.Space)) 
        {
            gancho.Translate(Vector3.up * speed, Space.World);
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            gancho.Translate(Vector3.down * speed, Space.World);
        }

        // Agarrar/Soltar con la tecla 'E'
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            if (joint == null)
            {
                AgarrarCarga();
            }
            else
            {
                SoltarCarga(); // Llama a la función de soltar
            }
        }
        
        // Muestra la puntuación en el editor
        // NOTA: Para un juego real, usarías Unity UI para mostrar esto en pantalla.
        Debug.Log("Score: " + score); 
    }

    
    void AgarrarCarga(){
        // Usamos un radio de 1.5f para ser generosos con la detección
        Collider[] hitColliders = Physics.OverlapSphere(gancho.position, 1.5f);

        foreach (var hitCollider in hitColliders)
        {
            // 1. Verificar el Tag de la carga (ASUMIMOS que tus prefabs tienen el Tag "Carga")
            if (hitCollider.CompareTag("Carga"))
            {
                Rigidbody rb = hitCollider.GetComponent<Rigidbody>();

                // 2. Asegurar que la carga sea válida y no esté ya siendo agarrada
                if (rb != null && ganchoRb != null && rb.gameObject.GetComponent<FixedJoint>() == null)
                {
                    // A. CREAR LA UNIÓN (PEGA LA FÍSICA DE LA CAJA AL GANCHO)
                    joint = rb.gameObject.AddComponent<FixedJoint>();
                    joint.connectedBody = ganchoRb;
                    joint.anchor = new Vector3(0, -0.5f, 0);
                    joint.breakForce = Mathf.Infinity; // Unión irrompible

                    // B. DESACTIVAR LA GRAVEDAD Y CONGELAR ROTACIÓN
                    rb.useGravity = false;
                    rb.freezeRotation = true;

                    // C. Centrar la carga en el gancho
                    rb.transform.position = gancho.position;

                    // D. Guardar la referencia de la carga que acabamos de agarrar
                    cargaAgarrada = rb.gameObject; 

                    Debug.Log("¡AGARRE ÉXITOSO! Item: " + hitCollider.name);
                    return; // Agarra solo la primera carga encontrada
                }
            }
        }
    }

    void SoltarCarga()
    {
        if (joint != null)
        {
            // OBTENER la carga ANTES de destruir el joint
            Rigidbody cargoRb = joint.connectedBody.GetComponent<Rigidbody>(); 

            // 1. Destruir el componente FixedJoint
            Destroy(joint);
            joint = null;

            if (cargoRb != null)
            {
                // 2. RESTAURAR LA FÍSICA
                cargoRb.useGravity = true;
                cargoRb.freezeRotation = false;
                cargoRb.AddForce(Vector3.down * 1f, ForceMode.VelocityChange);
            }

            // 3. Verificar si la carga soltada hace match con la estiba
            VerificarMatchYPuntuacion(cargaAgarrada);
            
            // 4. Limpiar la referencia de la carga agarrada
            cargaAgarrada = null; 
            
            Debug.Log("Carga soltada y física restaurada.");
        }
    }

    void VerificarMatchYPuntuacion(GameObject carga)
    {
        if (carga == null) return;

        // 1. Buscar la estiba más cercana que esté disponible (no llena)
        GameObject estibaCercana = BuscarEstibaCercana(carga.transform.position);

        if (estibaCercana != null)
        {
            // 2. Obtener el Material de la Carga y de la Estiba
            Material materialCarga = GetMaterial(carga);
            Material materialEstiba = GetMaterial(estibaCercana);

            // 3. Comparar los Materiales
            if (materialCarga != null && materialEstiba != null && materialCarga == materialEstiba)
            {
                // ¡MATCH DE MATERIALES EXITOSO!
                score++;
                estibasLlenas.Add(estibaCercana); // Marcar esta estiba como llena
                
                if (levelManager != null)
                {
                    levelManager.ApplyTimeBonus(10f); // Añade 10 segundos al tiempo restante
                }

                Debug.Log($"✅ ¡PUNTO GANADO! Carga '{carga.name}' ({materialCarga.name}) colocada correctamente en Estiba '{estibaCercana.name}'. Nuevo Score: {score}");
                
                // Opcional: Desactivar/Destruir la carga para que no se pueda volver a agarrar
                carga.SetActive(false); 
            }
            else
            {
                Debug.Log($"❌ FALLO DE MATCH. Carga: {(materialCarga != null ? materialCarga.name : "NULO")}, Estiba Cercana: {(materialEstiba != null ? materialEstiba.name : "NULO")}.");
            }
        }
        else
        {
            Debug.Log("Carga soltada lejos de una estiba válida.");
        }
    }
    
    GameObject BuscarEstibaCercana(Vector3 position)
    {
        GameObject bestEstiba = null;
        float minDistance = float.MaxValue;
        float maxSearchDistance = 2.0f; // Radio máximo para considerar una estiba

        foreach (var estiba in zonasDeEstiba)
        {
            // Solo considera estibas que no estén ya "llenas"
            if (estiba != null && !estibasLlenas.Contains(estiba)) 
            {
                float distance = Vector3.Distance(position, estiba.transform.position);
                
                if (distance < minDistance && distance < maxSearchDistance)
                {
                    minDistance = distance;
                    bestEstiba = estiba;
                }
            }
        }
        return bestEstiba;
    }
    
    Material GetMaterial(GameObject obj)
    {
        MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
        
        // Si no está en el padre, busca en los hijos (común en prefabs anidados)
        if (renderer == null)
        {
            renderer = obj.GetComponentInChildren<MeshRenderer>();
        }

        // Usamos sharedMaterial para comparar la referencia real del material
        return renderer != null ? renderer.sharedMaterial : null;
    }
}