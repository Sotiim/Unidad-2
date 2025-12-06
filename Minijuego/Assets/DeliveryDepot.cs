using UnityEngine;

public class DeliveryDepot : MonoBehaviour
{
    [Tooltip("Referencia al Level2Manager en la escena.")]
    public Level2Manager levelManager;

    // Asegúrate de que el Collider de este objeto esté marcado como Is Trigger.
private void OnTriggerEnter(Collider other)
{
    
    // Opción rápida: Si tiene un Rigidbody (asumiendo que solo el camión lo tiene)
    if (other.GetComponent<Rigidbody>() != null) 
    {
        Debug.Log("Si entra.");
        if (levelManager != null)
        {
            levelManager.WinLevel();
            gameObject.SetActive(false);
        }
        else
        {
             Debug.LogError("DeliveryDepot no tiene asignado el Level2Manager.");
        }
    }
}
}