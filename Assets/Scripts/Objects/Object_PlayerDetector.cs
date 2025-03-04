using UnityEngine;

public class Object_PlayerDetector : MonoBehaviour
{
    private Transform targetParent; // Primer hijo del objeto con este script
    private GameObject player;      // Referencia al jugador
    private bool isAttached = false; // Indica si el jugador está unido al objeto
    private HeavyObjectMovement movementScriptPesado; // Referencia a HeavyObjectMovement en el padre
    private LightObjectMovement movementScriptLiviano; // Referencia a HeavyObjectMovement en el padre
    [SerializeField] int tipo; // 0=liviano , 1=pesado, 2=arma
    private InputSystem_Actions inputs;

    private void Awake()
    {
        inputs = new InputSystem_Actions();
        inputs.Player.Interact.performed += ctx => DesPoseer();
    }

    private void OnDisable()
    {
        inputs.Player.Interact.performed -= ctx => DesPoseer();
    }

    private void Start()
    {
        switch (tipo)
        {
            case 1:
                movementScriptPesado = GetComponent<HeavyObjectMovement>();
                break;
            case 2:
                // TODO
                break;
            default:
                movementScriptLiviano = GetComponent<LightObjectMovement>();
                break;
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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerMovement script = other.GetComponent<playerMovement>();
            Debug.Log("Jugador salió del área.");
            player = null;
        }
    }

    public void Poseer(GameObject player)
    {
        player.transform.SetParent(targetParent);
        player.SetActive(false);
        isAttached = true;
        this.player = player;
        inputs.Enable();
        switch (tipo)
        {
            case 1:
                movementScriptPesado.enabled = true;
                break;
            case 2:
                //TODO
                break;
            default:
                movementScriptLiviano.enabled = true;
                break;
        }
    }

    private void DesPoseer()
    {
        if (isAttached)
        {
            player.SetActive(true);
            player.transform.SetParent(null);
            isAttached = false;
            inputs.Disable();
            switch (tipo)
            {
                case 1:
                    movementScriptPesado.enabled = false;
                    break;
                case 2:
                    //TODO
                    break;
                default:
                    movementScriptLiviano.enabled = false;
                    break;
            }
        }
    }
}
