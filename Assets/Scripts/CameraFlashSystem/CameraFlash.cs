using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraFlash : MonoBehaviour
{
    public Image flashImage; // Un UI Image que cubra toda la pantalla
    public float flashDuration = 5f; // Duración de la transición a blanco

    private void Start()
    {
        if (flashImage != null)
        {
            flashImage.gameObject.SetActive(true);
            flashImage.color = new Color(1, 1, 1, 0); // Transparente al inicio
        }
        TriggerFlash();
    }

    public void TriggerFlash()
    {
        if (flashImage != null)
        {
            StartCoroutine(FlashCoroutine());
        }
    }

    private IEnumerator FlashCoroutine()
    {
        float elapsedTime = 0f;
        while (elapsedTime < flashDuration)
        {
            float alpha = elapsedTime / flashDuration;
            flashImage.color = new Color(1, 1, 1, Mathf.Clamp01(alpha));
            elapsedTime += Time.deltaTime * (1f / Time.timeScale); // Ajuste para deltaTime
            Debug.Log($"Elapsed Time: {elapsedTime}, Alpha: {alpha}"); // Debug para verificar el tiempo
            yield return null;
        }
        flashImage.color = new Color(1, 1, 1, 1); // Asegurar que quede completamente blanco
    }
}




