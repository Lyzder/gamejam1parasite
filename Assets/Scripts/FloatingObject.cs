using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    float speed = 2f;  // Velocidad del movimiento
    float height = 0.25f;  // Altura del desplazamiento

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;  // Guarda la posición inicial
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * speed) * height;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
