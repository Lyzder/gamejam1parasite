using UnityEngine;

public class HeavyObjectMovement : MonoBehaviour
{
    private InputSystem_Actions inputs;
    private Vector2 moveInput;
    public float speed = 2f;

    private void Awake()
    {
        inputs = new InputSystem_Actions();
        inputs.Player.Enable();

        // Movimiento
        inputs.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputs.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnDestroy()
    {
        inputs.Player.Disable();
        inputs.Player.Move.performed -= ctx => moveInput = ctx.ReadValue<Vector2>();
        inputs.Player.Move.canceled -= ctx => moveInput = Vector2.zero;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }
}
