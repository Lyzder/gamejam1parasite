using UnityEngine;
using System.Collections;

public class SideToSideMovement : MonoBehaviour
{
    public float moveDistance = 50f; // Distancia que recorrerá a cada lado
    public float moveDuration = 1f; // Duración del movimiento de un lado a otro

    private Vector3 leftPosition;
    private Vector3 rightPosition;

    void Start()
    {
        // Guarda la posición inicial y calcula los extremos
        leftPosition = transform.position - Vector3.right * moveDistance;
        rightPosition = transform.position + Vector3.right * moveDistance;

        // Inicia el movimiento en bucle
        StartCoroutine(MoveSideToSide());
    }

    private IEnumerator MoveSideToSide()
    {
        while (true) // Bucle infinito para movimiento continuo
        {
            yield return MoveToPosition(rightPosition); // Mueve a la derecha
            yield return MoveToPosition(leftPosition);  // Mueve a la izquierda
        }
    }

    private IEnumerator MoveToPosition(Vector3 target)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, target, elapsedTime / moveDuration);
            yield return null;
        }

        transform.position = target; // Asegura que llega a la posición final
    }
}
