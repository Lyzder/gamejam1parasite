using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{
    private InputSystem_Actions inputs;
    private Vector2 moveInput;
    public float speed = 2f;
    private Vector2 lookInput;
    private CameraController cameraController;
    private Transform cameraTransform;
    private Vector3 cameraForward;
    private Vector3 cameraRight;
    private Vector3 move;

    private Rigidbody rb;
    public float jumpForce = 3f;
    public AudioClip poseerSfx;

    [SerializeField] Transform pivotePoseer;
    [SerializeField] float radioPoseer;
    [SerializeField] LayerMask capaDeteccion;

    private bool tieneEstrella = false; // Nueva variable

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputs = new InputSystem_Actions();
        inputs.Player.Enable();

        inputs.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        inputs.Player.Jump.performed += ctx => Jump();
        inputs.Player.Interact.performed += ctx => Interactuar();

        cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    private void OnDestroy()
    {
        inputs.Player.Disable();
        inputs.Player.Look.performed -= ctx => lookInput = ctx.ReadValue<Vector2>();
        inputs.Player.Jump.performed -= ctx => Jump();
        inputs.Player.Interact.performed -= ctx => Interactuar();
    }

    private void Update()
    {
        if (inputs.Player.Run.IsPressed())
        {
            speed = 4f;
        }
        else
        {
            speed = 2f;
        }

        moveInput = inputs.Player.Move.ReadValue<Vector2>().normalized;
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);

        move = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        cameraForward = cameraTransform.forward;
        cameraRight = cameraTransform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        moveDirection = (cameraForward * move.z + cameraRight * move.x).normalized;
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

        cameraController.SetLookInput(lookInput);
        lookInput = Vector2.zero;
    }

    private void LateUpdate()
    {
        RotatePlayerModel();
    }

    private void RotatePlayerModel()
    {
        Vector3 lookDirection;
        Quaternion targetRotation;

        if (moveInput.sqrMagnitude > 0.01f)
        {
            lookDirection = cameraTransform.forward;
            lookDirection.y = 0f;

            if (lookDirection != Vector3.zero)
            {
                targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 20f);
            }
        }
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.1f);
    }

    private void Interactuar()
    {
        Collider[] objetos = Physics.OverlapSphere(pivotePoseer.position, radioPoseer, capaDeteccion);

        if (objetos.Length > 0)
        {
            foreach (var obj in objetos)
            {
                if (obj.CompareTag("Star")) // Si es una estrella, la recoge
                {
                    RecogerEstrella(obj.gameObject);
                    return;
                }
               
                else // Si es otro objeto, sigue funcionando Poseer()
                {
                    Object_PlayerDetector detector = obj.GetComponent<Object_PlayerDetector>();
                    if (detector != null)
                    {
						AudioManager.Instance.PlaySFX(poseerSfx);
						detector.Poseer(gameObject);
						return;
                    }
                }
            }
        }
    }

    private void RecogerEstrella(GameObject estrella)
    {
        tieneEstrella = true;
        Destroy(estrella); // Destruye la estrella al recogerla
        Debug.Log("¡Has recogido la estrella!");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pivotePoseer.position, radioPoseer);
    }
}
