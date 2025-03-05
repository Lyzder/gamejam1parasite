using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponObjectMovement : MonoBehaviour
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
    private bool aiming;

    public bool isGrounded;
    public Quaternion rotationOffset;

    [Header("Configuración de Movimiento")]
    public float speed = 2f;
    public float jumpForce = 10f; // Fuerza del salto
    public float airControl = 0.5f; // Control en el aire

    // Shooting variables
    [Header("Shooting Settings")]
    [SerializeField] GameObject bullet;
    public float recarga = 3f;
    private float timer;
    private Transform bulletSpawn;
    public AudioClip shootSfx;

    private void Awake()
    {
        camara = transform.GetChild(3).GetChild(0).gameObject;
        cameraTransform = camara.transform;
        cameraController = camara.GetComponent<CameraController>();
        camara.SetActive(false);

        aiming = false;

        // Obtener el Rigidbody
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;

        // Obtener el punto de spawn de balas
        bulletSpawn = transform.GetChild(4);

        // Inicializar el sistema de entrada
        inputs = new InputSystem_Actions();
        inputs.Player.Enable();

        // Asignar eventos de entrada correctamente
        inputs.Player.Jump.performed += OnJump;

        inputs.Player.Aim.performed += ctx => SetAiming(true);
        inputs.Player.Aim.canceled += ctx => SetAiming(false);
        inputs.Player.Attack.performed += ctx => Shoot();

        // Lee input para la camara
        inputs.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
    }

    private void OnDestroy()
    {
        // Deshabilitar entradas y desuscribir eventos correctamente
        inputs.Player.Disable();
        inputs.Player.Jump.performed -= OnJump;
        inputs.Player.Aim.performed -= ctx => SetAiming(true);
        inputs.Player.Aim.canceled -= ctx => SetAiming(false);
        inputs.Player.Attack.performed -= ctx => Shoot();
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

    // Start is called before the first frame update
    void Start()
    {
        timer = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < recarga)
            timer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        // Mejor detección del suelo con un Raycast
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.5f);

        Move();

        cameraController.SetLookInput(lookInput);
        lookInput = Vector2.zero;
    }

    private void LateUpdate()
    {
        RotateObjectModel();
    }

    private void RotateObjectModel()
    {
        Vector3 lookDirection;
        Quaternion targetRotation;

        if (moveInput.sqrMagnitude > 0.01f || aiming)
        {
            lookDirection = cameraTransform.forward;
            if (!aiming)
                lookDirection.y = 0f;

            if (lookDirection != Vector3.zero)
            {
                targetRotation = Quaternion.LookRotation(lookDirection);
                
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation * rotationOffset, Time.deltaTime * 20f);
            }
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

    private void OnJump(InputAction.CallbackContext ctx)
    {
        if (isGrounded)
        {
            // Reiniciar la velocidad vertical para un salto consistente
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void SetAiming(bool toggle)
    {
        aiming = toggle;
        cameraController.SetAimingState(toggle);
    }

    private void Shoot()
    {
        if (aiming && timer >= recarga)
        {
            Instantiate(bullet, bulletSpawn.position, Quaternion.LookRotation(cameraTransform.forward));
            timer = 0;
            AudioManager.Instance.PlaySFX(shootSfx);
        }
    }
}
