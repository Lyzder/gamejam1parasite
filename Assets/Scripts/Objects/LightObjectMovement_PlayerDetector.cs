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
        if (targetParent != null)
        {
            targetParent.rotation = Quaternion.identity; // Mantiene el container sin rotación
        }

        if (playerInZone && Input.GetKeyDown(KeyCode.E) && targetParent != null)
        {
            if (!isAttached && targetParent.childCount == 0) // Solo si el contenedor no tiene hijos
            {
                Debug.Log("Jugador ahora es hijo de " + targetParent.name + " y se ha desactivado.");
                player.transform.SetParent(targetParent);
                player.SetActive(false);
                isAttached = true;
                CheckChildren(); // Verifica si el container tiene hijos y activa/desactiva el movimiento
            }
            else if (isAttached && movementScript.isGrounded && Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Se presionó 'E' y se cumplen las condiciones para soltar al jugador.");
                player.SetActive(true);
                player.transform.SetParent(null);
                isAttached = false;
                CheckChildren(); // Verifica si el container tiene hijos y activa/desactiva el movimiento
            }
            else if (targetParent.childCount > 0)
            {
                Debug.Log("El container ya tiene un hijo. No se puede añadir otro.");
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

    private void CheckChildren()
    {
        if (movementScript != null)
        {
            if (targetParent.childCount > 0)
            {
                movementScript.enabled = true;
                Debug.Log("LightObjectMovement activado.");
                if (rb != null) rb.isKinematic = false;
            }
            else
            {
                movementScript.enabled = false;
                Debug.Log("LightObjectMovement desactivado porque el container no tiene hijos.");
                if (rb != null)
                {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    rb.isKinematic = true;
                }
            }
        }
    }
}
