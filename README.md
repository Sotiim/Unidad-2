# 📚 Proyecto de Desarrollo de Software Interactivo - UTNG

## Presentación del Alumno
**Universidad Tecnológica del Norte de Guanajuato (UTNG)**

* **Alumno:** Cesar Alberto Garcia Aranda
* **Materia:** [Creación de Videojuegos]
* **Carrera:** [Ingeniería en Desarrollo y Gestión de Software]

---

## 🎯 Objetivo del Repositorio

Este proyecto académico compila una serie de ejercicios prácticos esenciales en el desarrollo de aplicaciones interactivas, cubriendo aspectos fundamentales como la **visualización 3D**, el diseño de **interfaces de usuario (UI)** y la implementación de sistemas de **gestión de estado (barra de vida)**.

---

## I. 🏞️ Práctica 3D: Manipulación de Entornos

Esta sección se dedica a la exploración de los principios de la **computación gráfica** y el **renderizado tridimensional** dentro del entorno de Unity. El enfoque está en construir y manipular escenas virtuales.

### Aspectos Desarrollados:

* **Implementación de Escena:** Configuración del *Scene* y del *Game View* en Unity.
* **Gestión de Cámaras:** Creación de un sistema de cámara (perspectiva o tercera persona) para la navegación.
* **Modelos y Geometría:** Uso de *GameObjects* y *Meshes* para el entorno.
* **Iluminación Básica:** Uso de luces direccionales y *Light Probes* de Unity.

---

## II. 🖼️ Módulo de Menú y Navegación (UI)

Desarrollo de una interfaz de usuario funcional utilizando el sistema **Unity UI (uGUI)**. Se prioriza la **usabilidad** y la **claridad** de las opciones mediante *Canvases* y *Rect Transforms*.

### Componentes Clave:

| Elemento de UI | Propósito | Estado |
| :--- | :--- | :--- |
| **Menú Principal** | Acceso a funcionalidades primarias (Inicio, Opciones, Créditos). | ✅ Completo |
| **Manejo de Eventos** | Uso de *Event Triggers* y funciones de botones de Unity. | ✅ Completo |
| **Diseño Adaptativo** | Uso de *Canvas Scaler* y *Layout Groups* para diferentes resoluciones. | ⚙️ En Revisión |

---

## III. ❤️ Barra de Vida (Health Management System)

Implementación del sistema de gestión de salud, un elemento de UI crítico para la retroalimentación del estado del usuario o personaje principal.

### Detalles de la Implementación:

1.  **Modelo de Datos:** Definición de variables públicas en C# para `currentHealth` y `maxHealth`.
2.  **Visualización Progresiva:** Uso de un componente `Image` con la propiedad `fillAmount` para representar visualmente la vida.
3.  **Mecanismo de Detección:** Implementación de *scripts* C# para registrar y aplicar daño (`TakeDamage()`) y curación (`Heal()`).

### Fragmento de Código (C# en Unity):

```csharp
// Ejemplo de cómo se actualiza la barra de vida en un script de Unity
public void UpdateHealthBar(float currentHealth, float maxHealth)
{
    // Asegura que la imagen de la barra (fillImage) esté asignada
    if (fillImage != null) 
    {
        // Actualiza el "lleno" de la imagen basado en el porcentaje
        fillImage.fillAmount = currentHealth / maxHealth;
    }
}
