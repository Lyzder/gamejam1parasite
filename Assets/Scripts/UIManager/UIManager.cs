using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI tiempoTxt;

    [SerializeField] private float tiempo = 299f; // Ahora visible en el Inspector
    private bool activa;

    [SerializeField]
    GameObject menuPantalla, tutoPantalla, opcionesPantalla,
               pausaPantalla, derrotaPantalla, victoriaPantalla, pressEMensaja;

    private void Start()
    {
        activa = false; // El contador no inicia hasta que se presione Play
        Time.timeScale = 1f; // Mantener Time.timeScale en 1 para RawImages
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        ActualizarUI();
    }

    private void Update()
    {
        if (activa)
        {
            Contador();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (GameManager.Instance.CurrentState == GameManager.GameState.Paused)
            {
                OnResumeGame();
            }
            else if (GameManager.Instance.CurrentState == GameManager.GameState.Playing)
            {
                OnPauseGame();
            }
        }
    }

    public void UpdateTimeUI(float time)
    {
        int minutes = (int)time / 60;
        float seconds = (float)time % 60f;
        tiempoTxt.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    private void Contador()
    {
        if (activa && tiempo > 0)
        {
            tiempo -= Time.unscaledDeltaTime; // Sigue corriendo aunque Time.timeScale = 0
            UpdateTimeUI(tiempo);
        }
        else if (tiempo <= 0)
        {
            activa = false;
            OnEndGame();
        }
    }

    public void OnStartGame()
    {
        if (GameManager.Instance.CurrentState != GameManager.GameState.Idle) return;

        GameManager.Instance.StartGame();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        activa = true;
        menuPantalla.SetActive(false);
        ActualizarUI();
    }

    public void OnPauseGame()
    {
        if (GameManager.Instance.CurrentState != GameManager.GameState.Playing) return;

        GameManager.Instance.PauseGame();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        activa = false;
        pausaPantalla.SetActive(true);
        ActualizarUI();
    }

    public void OnResumeGame()
    {
        if (GameManager.Instance.CurrentState != GameManager.GameState.Paused) return;

        GameManager.Instance.ResumeGame();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        activa = true;
        pausaPantalla.SetActive(false);
        ActualizarUI();
    }

    public void OnEndGame()
    {
        GameManager.Instance.EndGame();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        activa = false;
        derrotaPantalla.SetActive(true);
        ActualizarUI();
    }

    public void OnRestartGame()
    {
        StartCoroutine(RestartGameRoutine());
    }

    private IEnumerator RestartGameRoutine()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        yield return new WaitForEndOfFrame();
        menuPantalla.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnMenu()
    {
        GameManager.Instance.ReturnToMenu();
        tiempo = 299f;
        menuPantalla.SetActive(true);
        tutoPantalla.SetActive(false);
        opcionesPantalla.SetActive(false);
        pausaPantalla.SetActive(false);
        derrotaPantalla.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        activa = false;
        ActualizarUI();
    }

    public void ActivarOpciones(bool estado)
    {
        opcionesPantalla.SetActive(estado);
        Cursor.lockState = estado ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = estado;
        activa = !estado;
        ActualizarUI();
    }

    public void ActivarTutorial(bool estado)
    {
        tutoPantalla.SetActive(estado);
        Cursor.lockState = estado ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = estado;
        activa = !estado;
        ActualizarUI();
    }

    private void ActualizarUI()
    {
        bool enPantalla = menuPantalla.activeSelf || pausaPantalla.activeSelf || opcionesPantalla.activeSelf || tutoPantalla.activeSelf || derrotaPantalla.activeSelf;
        Cursor.lockState = enPantalla ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = enPantalla;
        activa = !enPantalla;
    }
}