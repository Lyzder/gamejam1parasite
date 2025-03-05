using UnityEngine;

public class BotonPresion : MonoBehaviour
{
    private bool activado = false;

    [SerializeField] private GameObject puertaObjeto; 
    private DoorInteraction doorInteraction;
    private bool yaEjecutado = false;  
    private void Start()
    {
        doorInteraction = puertaObjeto.GetComponent<DoorInteraction>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HeavyObject") && !activado && !yaEjecutado)
        {
            doorInteraction.AbrirPuerta();
            activado = true;
            yaEjecutado = true;  
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (yaEjecutado) return;
        if (other.CompareTag("HeavyObject") && activado)
        {
            doorInteraction.CerrarPuerta();
            activado = false;
        }
    }
}
