using UnityEngine;

public class RoomBoundary : MonoBehaviour
{
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Aseg�rate de que el personaje tenga el tag "Player"
        {
            gameObject.SetActive(false); // Desactiva el muro
        }

    }
}