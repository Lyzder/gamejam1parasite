using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI tiempoTxt;

    float tiempo;
    bool activa;

    [SerializeField]
    private GameObject menuPantalla, tutoPantalla, opcionesPantalla, pausaPantalla, derrotaPantalla, victoriaPantalla, pressEMensaja;
    [SerializeField] Slider musicSlider, sfxSlider;
    [SerializeField] Toggle muteCheck;

    private void Start()
    {
        tiempo = 299f;
        activa = false; // El contador no inicia hasta que se presione Play
        Time.timeScale = 0f; // Asegurar que el tiempo está pausado al inicio
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SaveOptions();
        ActualizarUI();
        AudioManager.Instance.PlaySceneBgm();
    }

    private void Update()
    {
        if (activa)
        {
            Contador();
        }

        // Detectar la tecla P para pausar o reanudar
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (GameManager.Instance.CurrentState == GameManager.GameState.Paused)
            {
                OnResumeGame(); // Si está pausado, reanudar
            }
            else if (GameManager.Instance.CurrentState == GameManager.GameState.Playing)
            {
                OnPauseGame(); // Si está en juego, pausar
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
        if (tiempo > 0)
        {
            tiempo -= Time.deltaTime;
            UpdateTimeUI(tiempo);
        }
        else
        {
            activa = false;
            OnEndGame(); // Si el tiempo llega a 0, el juego termina
        }
    }

    public void OnStartGame()
    {
        if (GameManager.Instance.CurrentState != GameManager.GameState.Idle) return;

        GameManager.Instance.StartGame();
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        activa = true; // Iniciar el contador al comenzar el juego
        menuPantalla.SetActive(false); // Ocultar el menú principal
        ActualizarUI();
    }

    public void OnPauseGame()
    {
        if (GameManager.Instance.CurrentState != GameManager.GameState.Playing) return;

        GameManager.Instance.PauseGame();
        Time.timeScale = 1f; // El juego sigue corriendo
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        activa = false; // Pausar solo el contador
        pausaPantalla.SetActive(true);
        ActualizarUI();
    }

    public void OnResumeGame()
    {
        if (GameManager.Instance.CurrentState != GameManager.GameState.Paused) return;

        GameManager.Instance.ResumeGame();
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        activa = true; // Reanudar el contador cuando el juego se reanude
        pausaPantalla.SetActive(false);
        ActualizarUI();
    }

    public void OnEndGame()
    {
        GameManager.Instance.EndGame();
        Time.timeScale = 1f; // El juego sigue corriendo
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        activa = false; // Detener el contador al finalizar el juego
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
        yield return new WaitForEndOfFrame(); // Esperar un frame para que la escena cargue
        menuPantalla.SetActive(false); // Ocultar el menú después de la recarga
        Time.timeScale = 1f; // Asegurar que el juego siga corriendo
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    public void OnMenu()
    {
        GameManager.Instance.ReturnToMenu(); // Volver al menú
        tiempo = 299f; // Reiniciar el contador de tiempo
        menuPantalla.SetActive(true); // Mostrar el menú principal
        tutoPantalla.SetActive(false);
        opcionesPantalla.SetActive(false);
        pausaPantalla.SetActive(false);
        derrotaPantalla.SetActive(false);
        Time.timeScale = 0f; // Pausar el juego
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        activa = false; // Detener el contador de tiempo
        ActualizarUI();
    }

    public void ActivarOpciones(bool estado)
    {
        opcionesPantalla.SetActive(estado);
        Time.timeScale = 1f; // El juego sigue corriendo
        Cursor.lockState = estado ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = estado;
        activa = !estado; // Pausar el contador si las opciones están activas
        ActualizarUI();
    }

    public void ActivarTutorial(bool estado)
    {
        tutoPantalla.SetActive(estado);
        Time.timeScale = 1f; // El juego sigue corriendo
        Cursor.lockState = estado ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = estado;
        activa = !estado; // Pausar el contador si el tutorial está activo
        ActualizarUI();
    }

    private void ActualizarUI()
    {
        bool enPantalla = menuPantalla.activeSelf || pausaPantalla.activeSelf || opcionesPantalla.activeSelf || tutoPantalla.activeSelf || derrotaPantalla.activeSelf;

        Cursor.lockState = enPantalla ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = enPantalla;
        activa = !enPantalla;
    }

    public void CambiarVolumenMusica()
    {
        AudioManager.Instance.SetMusicVolume(musicSlider.value);
    }

    public void CambiarVolumenSfx()
    {
        AudioManager.Instance.SetSfxVolume(sfxSlider.value);
    }

    public void SaveOptions()
    {
        AudioManager.Instance.SaveSoundPreferences(musicSlider.value, sfxSlider.value, muteCheck.isOn);
    }

    public void MuteAudio()
    {
        AudioManager.Instance.ToggleMute(musicSlider.value, sfxSlider.value);
    }

    public void LoadOptions()
    {
        musicSlider.value = PlayerPrefs.GetFloat(AudioManager.Instance.musicSavedValue); // Actualiza el slider de música
        sfxSlider.value = PlayerPrefs.GetFloat(AudioManager.Instance.sfxSavedValue); // Actualiza el slider de efectos de sonido
        muteCheck.isOn = PlayerPrefs.GetInt(AudioManager.Instance.isMuted) == 1; // Actualiza el toggle de silencio
        AudioManager.Instance.LoadSoundPreferences(); // Carga los valores guardados en AudioManager
    }
}
