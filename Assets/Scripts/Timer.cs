using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    float time = 0f;
    public TMP_Text timerText;

    // Start is called before the first frame update
    void Start()
    {
        timerText.text = "0:00.000";
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        int minutes = (int) time / 60;
        float seconds = (time - (minutes * 60));
        timerText.text = minutes.ToString() + ":"+ seconds.ToString("00.0");
    }

    public float GetTime()
    {
        return time;
    }
}
