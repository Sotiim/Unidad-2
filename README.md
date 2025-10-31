# 🎮 Prácticas de Desarrollo de Videojuegos - UTNG

## Información del Proyecto

**Universidad Tecnológica del Norte de Guanajuato (UTNG)**

* **Alumno:** Cesar Alberto Garcia Aranda
* **Materia:** Creación de Videojuegos
* **Carrera:** Ingeniería en Desarrollo y Gestión de Software
* **Motor:** Unity 3D / Unity Hub

---

## 📋 Resumen de Prácticas

Este repositorio contiene el código y los archivos desarrollados para tres prácticas fundamentales de la materia "Creación de Videojuegos", enfocadas en el manejo de entornos 3D, interfaces de usuario y sistemas de estado.

---

## 1. 🏗️ Práctica 3D: Implementación de Assets

Esta práctica se centró en la creación de una escena tridimensional utilizando **assets propios o personalizados**. El objetivo fue entender la importación, el escalado y la colocación de modelos dentro del motor Unity.

### Puntos Clave:

* **Creación de Entorno:** Diseño y montaje de la escena 3D.
* **Gestión de Assets:** Importación correcta de modelos, texturas y materiales.
* **Cámara:** Configuración de la perspectiva visual principal.
* **Iluminación:** Aplicación de luces para dar realismo a la escena.

---

## 2. 🕹️ Práctica de Menú y Navegación

Desarrollo de un sistema de **Menú Completo** que permite la navegación entre diferentes escenas del proyecto.

### Componentes Implementados:

| Elemento | Descripción |
| :--- | :--- |
| **Menú Principal** | Punto de entrada del juego (Botón Jugar, Opciones, Salir). |
| **Transición de Escenas** | Lógica C# para cargar diferentes niveles/escenas. |
| **Menú de Opciones** | Gestión básica de ajustes (ej. volumen, calidad gráfica). |
| **Interfaz UI** | Uso de los componentes *Canvas* y *Buttons* de Unity. |

---

## 3. ❤️ Práctica de Barra de Vida (Health Bar)

Implementación funcional de un sistema de **gestión de vida** y su representación gráfica a través de una Barra de Vida.

### Detalles Técnicos:

* **Lógica C#:** Scripting para manejar la salud actual y máxima.
* **Actualización Visual:** La barra se actualiza dinámicamente utilizando el componente `Image.fillAmount` de Unity UI.
* **Feedback Visual:** La barra de vida cambia su representación (ej. tamaño o color) al recibir daño.

---
Aquí tienes los **detalles técnicos modificados** según la descripción del texto que está arriba (el del minijuego del minisúper):

---

## EXTRA. 🛒 Minijuego que integra las lecciones que se han estado haciendo.

Minijuego el cual hace uso de las prácticas que se han tenido a lo largo de la unidad. En este caso es un minisúper en el cual debes encontrar 5 objetos generados al azar; cuenta con un menú y una barra de vida que funciona como temporizador para completar la búsqueda.

### Detalles Técnicos:

* **Lógica C#:** Scripting para controlar la generación aleatoria de los objetos, el conteo de ítems encontrados y el manejo del tiempo (barra de vida).
* **Interfaz y Retroalimentación:** La barra de vida se actualiza dinámicamente con `Image.fillAmount`, representando el tiempo restante; además, puede cambiar de color conforme el tiempo se agota.
* **Sistemas de Juego:** Se implementa un sistema de detección de objetos recogidos, un menú principal y lógica para finalizar el juego al encontrar todos los productos o al agotarse el tiempo.
* **Optimización:** Uso de listas y funciones aleatorias (`Random.Range`) para la selección de productos y reinicio del escenario.


---

## 🛠️ Requisitos del Sistema

Para abrir este proyecto, es necesario tener instalado:

* **Unity Hub**
* **Unity 3D** (Se recomienda la versión 2022.3.62f1)

---

**Cesar Alberto Garcia Aranda - UTNG**
