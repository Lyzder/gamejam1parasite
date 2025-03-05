using TMPro.Examples;
using UnityEngine;
using UnityEngine.InputSystem; // Necesario para InputSystem_Actions

public class LightObjectMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Vector2 moveInput;
    private InputSystem_Actions inputs;
    private GameObject camara;
    private Transform cameraTransform;
    private CameraController cameraController;
    private Vector2 lookInput;
    private Vector3 cameraForward;
    private Vector3 cameraRight;
    private Vector3 move;

    [Header("Movimiento y Salto")]
    public float speed = 4f;
    public float jumpForce = 10f; // Fuerza del salto
    public float airControl = 0.5f; // Control en el aire

    public bool isGrounded;

    private void Awake()
    {
        camara = transform.GetChild(3).GetChild(0).gameObject;
        cameraTransform = camara.transform;
        cameraController = camara.GetComponent<CameraController>();
        camara.SetActive(false);

        // Obtener el Rigidbody y asegurar que la gravedad esté activada
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;

        // Inicializar el sistema de entrada
        inputs = new InputSystem_Actions();
        inputs.Player.Enable();

        // Asignar eventos de entrada correctamente
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
        camara.SetActive(true);
    }

    private void OnDisable()
    {
        inputs.Player.Disable();
        camara?.SetActive(false);
    }

    private void FixedUpdate()
    {
        // Mejor detección del suelo con un Raycast
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.7f);

        Move();

        cameraController.SetLookInput(lookInput);
        lookInput = Vector2.zero;
    }

    private void OnJump(InputAction.CallbackContext ctx)
    {
        if (isGrounded)
        {
            // Reiniciar la velocidad vertical para un salto consistente
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
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

    private void OnDrawGizmos()
    {
        // Color del Gizmo (verde si está en el suelo, rojo si no)
        Gizmos.color = isGrounded ? Color.green : Color.red;

        // Dirección y longitud del Raycast
        Vector3 origin = transform.position;
        Vector3 direction = Vector3.down * 0.5f;

        // Dibujar el Raycast como una línea
        Gizmos.DrawLine(origin, origin + direction);

        // Dibujar un punto al final del Raycast
        Gizmos.DrawSphere(origin + direction, 0.02f);
    }


}
