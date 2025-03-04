using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    static UIManager current;

    public TextMeshProUGUI tiempoTxt;

    float tiempo;
    bool activa;

    
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.K)) {
            PauseTime();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            ResumeTime();
        }

        if (activa)
        {
            Contador();
        }
    }
    private void Start()
    {
        InicioPartida();
    }

    public void UpdateTimeUI(float time)
    {

        int minutes = (int)time / 60;
        float seconds = (float)time % 60f;

        tiempoTxt.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

   
    private void Contador()
    {
        UpdateTimeUI(tiempo);

        if(tiempo > 0)
        {
            tiempo -= Time.deltaTime;
            //Invoke("Contador", 1f);
        } else
        {
            activa = false;
        }
    }
    void InicioPartida()
    {
        tiempo = 299f;

        activa = true;
    }
    void PauseTime()
    {
        activa = false;
    }

    void ResumeTime()
    {
        activa = true;
    }
}
