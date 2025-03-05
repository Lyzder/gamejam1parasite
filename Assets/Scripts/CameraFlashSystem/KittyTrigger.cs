using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class KittyTrigger : MonoBehaviour
{
    public GameObject flashObject; // Asigna UIEndingCutscene desde el inspector
    public GameObject textEndingObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Asegúrate de que el jugador tiene la tag "Player"
        {
            flashObject.SetActive(true);
            StartCoroutine(ActivateTextAfterDelay(10f)); // Espera 10 segundos antes de activar el texto
        }
    }

    private IEnumerator ActivateTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        textEndingObject.SetActive(true); // Activa el texto después de 10 segundos
    }
}
