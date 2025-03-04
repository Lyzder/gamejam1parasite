using UnityEngine;

public class RotateY : MonoBehaviour
{
    public float rotationSpeed = 50f; // Velocidad de rotaci�n en grados por segundo

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
