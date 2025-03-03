using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    void OnMouseDown()
    {
        Debug.Log("¡Hiciste clic en " + gameObject.name + "!");
        GameManager.Instance.StartGame();
        GameManager.Instance.PauseGame();
        GameManager.Instance.ResumeGame();
        GameManager.Instance.EndGame();
    }
}
