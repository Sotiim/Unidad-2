using UnityEngine;
using UnityEngine.Audio; 
using UnityEngine.UI; 

public class VolumeController : MonoBehaviour
{
    public AudioMixer masterMixer; 
    public Slider volumeSlider;
    
    private const string MixerParam = "MasterVolume"; 
    private const string PlayerPrefsKey = "MasterVolumeLevel"; 

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat(PlayerPrefsKey, 1f); 
        volumeSlider.value = savedVolume;

        // 2. Ejecutar la función para asegurar que el Mixer se inicialice correctamente.
        SetMasterVolume(savedVolume); 
        }
    
    // Función llamada por el Slider
    public void SetMasterVolume(float volume)
    {
        // El script sí funciona, lo dejamos como estaba.
        float volumeDB = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20f;
        masterMixer.SetFloat(MixerParam, volumeDB);
        
        // Guardar el valor del slider inmediatamente después de moverlo.
        PlayerPrefs.SetFloat(PlayerPrefsKey, volume);
        PlayerPrefs.Save();
    }
}