using UnityEngine;
using UnityEngine.InputSystem; // Necesario para InputSystem_Actions

public class HeavyObjectMovement : MonoBehaviour
{
    private Rigidbody rb;
    private InputSystem_Actions inputs;
    private Vector2 moveInput;

    [Header("Configuración de Movimiento")]
    public float speed = 2f;

    private void Awake()
    {
        // Obtener el Rigidbody
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;

        // Inicializar el sistema de entrada
        inputs = new InputSystem_Actions();
        inputs.Player.Enable();

        // Asignar eventos correctamente
        inputs.Player.Move.performed += OnMove;
        inputs.Player.Move.canceled += OnMoveCancel;
    }

    private void OnDestroy()
    {
        // Deshabilitar entradas y desuscribir eventos correctamente
        inputs.Player.Disable();
        inputs.Player.Move.performed -= OnMove;
        inputs.Player.Move.canceled -= OnMoveCancel;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        // Calcular dirección de movimiento
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        // Usar MovePosition para un movimiento más preciso con físicas
        rb.MovePosition(rb.position + moveDirection * speed * Time.fixedDeltaTime);
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    private void OnMoveCancel(InputAction.CallbackContext ctx)
    {
        moveInput = Vector2.zero;
    }
}
