using UnityEngine;

public class PressButton : MonoBehaviour
{
    public bool isPressed;
    GameObject button;
    private void Update()
    {
        if (isPressed)
        {
            button.GetComponent<Renderer>().material.color = Color.green;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("HeavyObject"))
        {
            Debug.Log("colisiono con " + collision.gameObject.name);
            isPressed = true;
            button = collision.gameObject;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        isPressed = false;
        button = null;
    }


}
