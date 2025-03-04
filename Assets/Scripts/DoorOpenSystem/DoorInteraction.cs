using System.Collections;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public float openAngle = 90f;
    public float openSpeed = 2f;
    private bool isOpen = false;

    private Quaternion _closedRotation;
    private Quaternion _openRotation;
    private Coroutine _currentCoroutine;

    private void Start()
    {
        _closedRotation = transform.rotation; // Guarda la rotación cerrada
        _openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, openAngle, 0)); // Define la abierta
    }

    public void AbrirPuerta()
    {
        if (!isOpen) // Solo abre si está cerrada
        {
            if (_currentCoroutine != null) StopCoroutine(_currentCoroutine);
            _currentCoroutine = StartCoroutine(MoverPuerta(_openRotation));
            isOpen = true;
        }
    }

    public void CerrarPuerta()
    {
        if (isOpen) // Solo cierra si está abierta
        {
            if (_currentCoroutine != null) StopCoroutine(_currentCoroutine);
            _currentCoroutine = StartCoroutine(MoverPuerta(_closedRotation));
            isOpen = false;
        }
    }

    private IEnumerator MoverPuerta(Quaternion targetRotation)
    {
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.01f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * openSpeed);
            yield return null;
        }
        transform.rotation = targetRotation;
    }
}
