using UnityEngine;

public class TriggerBlock : MonoBehaviour
{
    public float pushForce = 10f; // Aumentamos la fuerza de empuje
    public float resetDistance = 0.1f; // Distancia mínima para evitar atravesar

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Poseeible"))
        {
            // Obtener la dirección opuesta al objeto
            Vector3 pushDirection = other.transform.position - transform.position;
            pushDirection.y = 0; // Mantener en el mismo nivel

            // Aplicar una fuerza fuerte en la dirección opuesta
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(pushDirection.normalized * pushForce, ForceMode.VelocityChange);
            }
            else
            {
                // Si el objeto no tiene Rigidbody, lo empujamos manualmente
                other.transform.position += pushDirection.normalized * Time.deltaTime * pushForce;
            }

            // Si el objeto ya cruzó demasiado, lo regresamos
            if (Vector3.Distance(other.transform.position, transform.position) < resetDistance)
            {
                other.transform.position -= pushDirection.normalized * resetDistance * 2;
            }
        }
    }
}
