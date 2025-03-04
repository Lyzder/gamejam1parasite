using UnityEngine;

public class LightObjectMovement_PlayerDetector : MonoBehaviour
{
    private Transform targetParent; // Primer hijo del objeto con este script
    private GameObject player;      // Referencia al jugador
    private bool playerInZone = false; // Indica si el jugador está en el trigger
    private bool isAttached = false; // Indica si el jugador está unido al objeto
    private LightObjectMovement movementScript; // Referencia a LightObjectMovement en el padre
    private Rigidbody rb; // Referencia al Rigidbody del objeto liviano

    private void Start()
    {
        movementScript = GetComponentInParent<LightObjectMovement>();
        rb = GetComponentInParent<Rigidbody>();

        if (movementScript != null)
        {
            movementScript.enabled = false;
            Debug.Log("LightObjectMovement desactivado en el padre.");
        }
        else
        {
            Debug.LogWarning("No se encontró LightObjectMovement en el padre.");
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
                player.transform.SetParent(targetParent);
                player.SetActive(false);
                movementScript.enabled = true;

                // Reactivar física al tomar el objeto
                if (rb != null)
                {
                    rb.isKinematic = false;
                }

                isAttached = true;
            }
            else if (isAttached && movementScript.isGrounded) // Solo se suelta si está en el suelo
            {
                // Segunda vez presionando E: El jugador se reactiva y deja de ser hijo
                Debug.Log("Jugador activado, dejó de ser hijo, y LightObjectMovement del padre se desactiva.");
                player.SetActive(true);
                player.transform.SetParent(null);
                isAttached = false;

                // Desactivar movimiento y frenar el objeto
                if (movementScript != null)
                {
                    movementScript.enabled = false;
                }

                if (rb != null)
                {
                    rb.velocity = Vector3.zero; // Detiene el movimiento
                    rb.angularVelocity = Vector3.zero; // Detiene la rotación
                    rb.isKinematic = true; // Desactiva físicas temporalmente
                }

                Debug.Log("LightObjectMovement desactivado en el padre.");
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
