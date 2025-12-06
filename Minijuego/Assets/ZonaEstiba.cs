using UnityEngine;

public class ZonaEstiba : MonoBehaviour
{
    // El Level Manager (Asignar en el Inspector)
    public Level3Manager levelManager;

    public int itemsRequired = 0; 

    private int itemsStacked = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Carga"))
        {
            itemsStacked++;
            if (itemsStacked >= itemsRequired && levelManager != null)
            {
                levelManager.WinLevel();
            }
        }
    }

    // Si la carga cae del área, se resta del contador
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Carga") && itemsStacked > 0)
        {
            itemsStacked--;
        }
    }
    public void SetRequiredItems(int requiredCount)
    {
        itemsRequired = requiredCount;
    }
}