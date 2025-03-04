using UnityEngine;

public class HeavyObjectMovement_PlayerDetector : MonoBehaviour
{
    private Transform targetParent; // Primer hijo del objeto con este script
    private GameObject player;      // Referencia al jugador
    private bool playerInZone = false; // Indica si el jugador está en el trigger
    private bool isAttached = false; // Indica si el jugador está unido al objeto
    private HeavyObjectMovement movementScript; // Referencia a HeavyObjectMovement en el padre
    [SerializeField] int tipo; // 0=liviano , 1=pesado, 2=arma

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
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerMovement script = other.GetComponent<playerMovement>();
            Debug.Log("Jugador detectado. Presiona 'E' para unirte.");
            player = other.gameObject;
            playerInZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerMovement script = other.GetComponent<playerMovement>();
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
                Debug.Log("HeavyObjectMovement activado.");
            }
            else
            {
                movementScript.enabled = false;
                Debug.Log("HeavyObjectMovement desactivado porque el container no tiene hijos.");
            }
        }
    }

    public void Poseer(GameObject player)
    {
        player.transform.SetParent(targetParent);
        player.SetActive(false);
        isAttached = true;
        this.player = player;
    }

    public void DesPoseer()
    {
        if (isAttached)
        {
            player.SetActive(true);
            player.transform.SetParent(null);
            isAttached = false;
        }
    }
}
