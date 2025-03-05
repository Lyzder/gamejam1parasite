using System.Collections;
using UnityEngine;

public class KittyTrigger : MonoBehaviour
{
    public GameObject flashObject; // UIEndingCutscene
    public GameObject textEndingObject;
    public GameObject victoryScreen; // Pantalla de victoria

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Aseg�rate de que el jugador tiene la tag "Player"
        {
            flashObject.SetActive(true);
            StartCoroutine(ActivateSequence());
        }
    }

    private IEnumerator ActivateSequence()
    {
        yield return new WaitForSeconds(10f); // Espera 10 segundos para mostrar el texto
        textEndingObject.SetActive(true);

        yield return new WaitForSeconds(5f); // Espera 5 segundos m�s
        textEndingObject.SetActive(false); // Oculta el texto primero
        yield return new WaitForSeconds(0.1f); // Peque�a pausa para asegurar que el UI se actualiza
        flashObject.SetActive(false); // Oculta la animaci�n inicial
        victoryScreen.SetActive(true); // Muestra la pantalla de victoria
    }
}
