using UnityEngine;
using System.Collections;

public class JumpLoop : MonoBehaviour
{
    public float jumpHeight = 50f; // Altura del salto
    public float jumpDuration = 0.5f; // Duración total del salto

    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position; // Guarda la posición inicial
        StartCoroutine(JumpCoroutine()); // Inicia el salto en bucle
    }

    private IEnumerator JumpCoroutine()
    {
        while (true) // Bucle infinito para que el objeto siempre salte
        {
            float elapsedTime = 0f;
            Vector3 targetPosition = originalPosition + Vector3.up * jumpHeight;

            // Subida
            while (elapsedTime < jumpDuration / 2)
            {
                elapsedTime += Time.deltaTime;
                transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / (jumpDuration / 2));
                yield return null;
            }

            elapsedTime = 0f;

            // Bajada
            while (elapsedTime < jumpDuration / 2)
            {
                elapsedTime += Time.deltaTime;
                transform.position = Vector3.Lerp(targetPosition, originalPosition, elapsedTime / (jumpDuration / 2));
                yield return null;
            }

            transform.position = originalPosition; // Asegura que vuelve a la posición original
        }
    }
}
