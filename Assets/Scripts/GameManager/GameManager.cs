using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { Idle, Playing, Paused, GameOver }
    public GameState CurrentState { get; private set; } = GameState.Idle;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame()
    {
        if (CurrentState == GameState.Idle || CurrentState == GameState.GameOver)
        {
            CurrentState = GameState.Playing;
            Debug.Log("Game Started!");
            // Lógica adicional, como reiniciar variables o cargar la escena
           
        }
    }

    public void PauseGame()
    {
        if (CurrentState == GameState.Playing)
        {
            CurrentState = GameState.Paused;
            Time.timeScale = 0f;
            Debug.Log("Game Paused!");
        }
    }

    public void ResumeGame()
    {
        if (CurrentState == GameState.Paused)
        {
            CurrentState = GameState.Playing;
            Time.timeScale = 1f;
            Debug.Log("Game Resumed!");
        }
    }

    public void EndGame()
    {
        if (CurrentState == GameState.Playing || CurrentState == GameState.Paused)
        {
            CurrentState = GameState.GameOver;
            Time.timeScale = 1f; // Asegurar que el tiempo se restablece
            Debug.Log("Game Over!");
            // Aquí puedes agregar lógica para mostrar una pantalla de fin de juego
        }
    }
   
   

  

}

