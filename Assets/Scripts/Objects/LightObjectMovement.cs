using UnityEngine;
using UnityEngine.InputSystem; // Necesario para InputSystem_Actions

public class LightObjectMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Vector2 moveInput;
    private InputSystem_Actions inputs;

    [Header("Movimiento y Salto")]
    public float speed = 4f;
    public float jumpForce = 10f; // Fuerza del salto
    public float airControl = 0.5f; // Control en el aire

    public bool isGrounded;

    private void Awake()
    {
        // Obtener el Rigidbody y asegurar que la gravedad esté activada
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;

        // Inicializar el sistema de entrada
        inputs = new InputSystem_Actions();
        inputs.Player.Enable();

        // Asignar eventos de entrada correctamente
        inputs.Player.Jump.performed += OnJump;
    }

    private void OnDestroy()
    {
        // Deshabilitar entradas y desuscribir eventos correctamente
        inputs.Player.Disable();
        inputs.Player.Jump.performed -= OnJump;
    }

    private void Update()
    {
        // Leer la entrada de movimiento
        moveInput = inputs.Player.Move.ReadValue<Vector2>();

        // Aplicar control en el aire
        float controlFactor = isGrounded ? 1f : airControl;
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y) * (speed * controlFactor);

        // Aplicar el movimiento manteniendo la velocidad vertical
        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);
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

    private void FixedUpdate()
    {
        // Mejor detección del suelo con un Raycast
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.5f);
    }

    private void OnDrawGizmos()
    {
        // Color del Gizmo (verde si está en el suelo, rojo si no)
        Gizmos.color = isGrounded ? Color.green : Color.red;

        // Dirección y longitud del Raycast
        Vector3 origin = transform.position;
        Vector3 direction = Vector3.down * 0.1f;

        // Dibujar el Raycast como una línea
        Gizmos.DrawLine(origin, origin + direction);

        // Dibujar un punto al final del Raycast
        Gizmos.DrawSphere(origin + direction, 0.02f);
    }


}
