using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{
    private PlayerInputs inputs;
    private Vector2 moveInput;
    public float speed = 2f;

    private void Awake()
    {
        inputs = new PlayerInputs();
        inputs.Movement.Enable();
    }

    private void Update()
    {
        if (inputs.Movement.Run.IsPressed())
        {
            speed = 4f;
        }
        else
        {
            speed = 2f;
        }

        moveInput = inputs.Movement.Walk.ReadValue<Vector2>().normalized;
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);

        if (moveDirection.sqrMagnitude > 0.01f) // Evita rotar si no hay movimiento
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }
}
