using UnityEngine;
using UnityEngine.SceneManagement; 

public class VolverMenu : MonoBehaviour
{
    public SceneTransitioner transitionManager;

    public void Regresar()
    {
        transitionManager.StartTransition("MenuPrincipal"); // Carga la escena principal [cite: 76]
    }
}