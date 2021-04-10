using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameTimer : MonoBehaviour
{
    //Pause
    private float pauseTimer = 0;
    private float timer = 0;
    private bool paused = false;

    public bool isPaused()
    {
        return paused;
    }

    void Update()
    {
        if (paused)
        {
            timer += Time.deltaTime;
            if (timer > pauseTimer && pauseTimer > 0)
            {
                timer = 0;
                paused = false;
            }

        }
    }

    public void PauseForSeconds(float s)
    {
        timer = 0;
        pauseTimer = s;
        paused = true;
    }

    public void PauseStart()
    {
        timer = 0;
        pauseTimer = 0;
        paused = true;
    }

    public void PauseStop()
    {
        timer = 0;
        pauseTimer = 0;
        paused = false;
    }
}
