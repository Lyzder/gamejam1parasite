using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

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


    private void Awake()
    {
        inputs = new InputSystem_Actions();
        inputs.Player.Enable();

        inputs.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
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

        // Calculate the movement direction relative to the camera
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
            // Get the camera's forward direction, ignoring the y-axis
            lookDirection = cameraTransform.forward;
            lookDirection.y = 0f; // Ensure the rotation is only on the horizontal plane

            if (lookDirection != Vector3.zero)
            {
                // Smooth rotation towards the camera direction
                targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 20f);
            }
        }
    }
}
