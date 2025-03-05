using UnityEngine;
using UnityEngine.InputSystem; // Necesario para InputSystem_Actions

public class HeavyObjectMovement : MonoBehaviour
{
    private Rigidbody rb;
    private InputSystem_Actions inputs;
    private Vector2 moveInput;
    private GameObject camara;
    private Transform cameraTransform;
    private CameraController cameraController;
    private Vector2 lookInput;
    private Vector3 cameraForward;
    private Vector3 cameraRight;
    private Vector3 move;

    [Header("Configuración de Movimiento")]
    public float speed = 2f;
    public float jumpForce = 10f; // Fuerza del salto
    public float airControl = 0.5f; // Control en el aire

    public bool isGrounded;
    public AudioClip jump;
    public AudioClip fall;

    private void Awake()
    {
        camara = transform.GetChild(3).GetChild(0).gameObject;
        cameraTransform = camara.transform;
        cameraController = camara.GetComponent<CameraController>();
        camara.SetActive(false);

        // Obtener el Rigidbody
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;

        // Inicializar el sistema de entrada
        inputs = new InputSystem_Actions();
        inputs.Player.Enable();

        inputs.Player.Jump.performed += OnJump;
        inputs.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
    }

    private void OnDestroy()
    {
        // Deshabilitar entradas y desuscribir eventos correctamente
        inputs.Player.Disable();
        inputs.Player.Jump.performed -= OnJump;
        inputs.Player.Look.performed -= ctx => lookInput = ctx.ReadValue<Vector2>();
    }

    private void OnEnable()
    {
        inputs.Player.Enable();
        camara.SetActive (true);
    }

    private void OnDisable()
    {
        inputs.Player.Disable();
        camara?.SetActive (false);
    }

    private void FixedUpdate()
    {
        CheckFallen();
        Move();
        if (!isGrounded)
            rb.AddForce(Vector3.down * 20f, ForceMode.Acceleration);
        cameraController.SetLookInput(lookInput);
        lookInput = Vector2.zero;

    }
    private void Move()
    {
        moveInput = inputs.Player.Move.ReadValue<Vector2>().normalized;
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);

        move = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        cameraForward = cameraTransform.forward;
        cameraRight = cameraTransform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Movimiento relativo a la c�mara
        moveDirection = (cameraForward * move.z + cameraRight * move.x);

        //transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
        rb.AddForce(moveDirection * speed);
    }

    private void OnJump(InputAction.CallbackContext ctx)
    {
        if (isGrounded)
        {
            // Reiniciar la velocidad vertical para un salto consistente
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            AudioManager.Instance.PlaySFX(jump);
        }
    }

    private void CheckFallen()
    {
        if (!isGrounded)
        {
            isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.4f);
            if(isGrounded)
                AudioManager.Instance.PlaySFX(fall);
        }
        else
            isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.4f);
    }

}
