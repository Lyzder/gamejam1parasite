using UnityEngine;

public class LightObjectMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Vector2 moveInput;
    private InputSystem_Actions inputs;

    public float speed = 4f;
    public float jumpForce = 10f; // Salto alto
    public float airControl = 0.5f; // Control en el aire

    public bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true; // Asegura que use gravedad

        inputs = new InputSystem_Actions();
        inputs.Player.Enable();

        inputs.Player.Jump.performed += ctx => Jump();
    }

    private void OnDestroy()
    {
        inputs.Player.Disable();
        inputs.Player.Jump.performed -= ctx => Jump();
    }

    private void Update()
    {
        moveInput = inputs.Player.Move.ReadValue<Vector2>();

        // Movimiento más controlable en el aire
        float controlFactor = isGrounded ? 1f : airControl;
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y) * (speed * controlFactor);

        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // Reinicia velocidad vertical
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
