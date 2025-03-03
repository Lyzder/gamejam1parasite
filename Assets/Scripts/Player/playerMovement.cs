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


    private void Awake()
    {
        inputs = new InputSystem_Actions();
        inputs.Player.Enable();

        inputs.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
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

        if (moveDirection.sqrMagnitude > 0.01f) // Evita rotar si no hay movimiento
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);


        cameraController.SetLookInput(lookInput);
        lookInput = Vector2.zero;

    }
}
