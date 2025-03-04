using UnityEngine;

public class PlayerDHeavyObjectMovement_PlayerDetectoretector : MonoBehaviour
{
    private Transform targetParent; // Primer hijo del objeto con este script
    private GameObject player;      // Referencia al jugador
    private bool playerInZone = false; // Indica si el jugador está en el trigger
    private bool isAttached = false; // Indica si el jugador está unido al objeto
    private HeavyObjectMovement movementScript; // Referencia a HeavyObjectMovement en el padre

    private void Start()
    {
        movementScript = GetComponentInParent<HeavyObjectMovement>();

        if (movementScript != null)
        {
            movementScript.enabled = false;
            Debug.Log("HeavyObjectMovement desactivado en el padre.");
        }
        else
        {
            Debug.LogWarning("No se encontró HeavyObjectMovement en el padre.");
        }

        if (transform.childCount > 0)
        {
            targetParent = transform.GetChild(0); // Obtiene el primer hijo
        }
        else
        {
            Debug.LogWarning("El objeto no tiene hijos. No se puede asignar un nuevo padre.");
        }
    }

    private void Update()
    {
        if (playerInZone && Input.GetKeyDown(KeyCode.E) && targetParent != null)
        {
            if (!isAttached)
            {
                // Primera vez presionando E: El jugador se vuelve hijo y se desactiva
                Debug.Log("Jugador ahora es hijo de " + targetParent.name + " y se ha desactivado.");
                player.transform.SetParent(targetParent); // Hace que el jugador sea hijo del targetParent
                player.SetActive(false); // Desactiva al jugador
                movementScript.enabled = true;
                isAttached = true;
            }
            else
            {
                // Segunda vez presionando E: El jugador se reactiva y deja de ser hijo
                Debug.Log("Jugador activado, dejó de ser hijo, y HeavyObjectMovement del padre se desactiva.");
                player.SetActive(true);
                player.transform.SetParent(null); // El jugador deja de ser hijo
                isAttached = false;

                // Desactivar HeavyObjectMovement en el padre si existe
                if (movementScript != null)
                {
                    movementScript.enabled = false;
                    Debug.Log("HeavyObjectMovement desactivado en el padre.");
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jugador detectado. Presiona 'E' para unirte.");
            player = other.gameObject;
            playerInZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jugador salió del área.");
            player = null;
            playerInZone = false;
        }
    }
}
