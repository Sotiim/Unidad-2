using UnityEngine;
using UnityEngine.UI; 

public class BarraVida : MonoBehaviour
{
    private PlayerController playerController; 
    public Image barraVida;
    private float vidaMaxima; 

    void Start()
    {
        // 1. Encuentra el objeto Player y obtén el script PlayerController
        // Si el objeto Player no está activo, GameObject.Find fallará.
        GameObject playerObject = GameObject.Find("Player");

        if (playerObject != null)
        {
            playerController = playerObject.GetComponent<PlayerController>(); 

            // 2. Si se encuentra el script, inicializa la vida máxima
            if (playerController != null)
            {
                vidaMaxima = playerController.vida;
            }
            else
            {
                Debug.LogError("ERROR: No se encontró el componente PlayerController en el objeto 'Player'.");
            }
        }
        else
        {
            Debug.LogError("ERROR: No se encontró ningún objeto llamado 'Player' en la escena.");
        }
    }

    void Update()
    {
        // Solo actualiza si la referencia no es nula
        if (playerController != null && vidaMaxima > 0)
        {
            barraVida.fillAmount = playerController.vida / vidaMaxima; 
        }
    }
}