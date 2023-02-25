using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameEnd : MonoBehaviour
{
    private float bestTime;
    private float time;
    Timer timer;
    


    [SerializeField] GameObject winScreen;
    [SerializeField] TMP_Text bestTimeText;
    [SerializeField] TMP_Text currentTimeText;
    [SerializeField] GameObject newRecordPopUp;


    [SerializeField] GameObject deathScreen;

    // Start is called before the first frame update
    void Start()
    {
        timer = GetComponent<Timer>();
        if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name))
        {
            bestTime = PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name);
        }
        else
        {
            bestTime = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndGame(bool win)  //decided to use the same function for both winning and losing for some reason was just a cool thing to try
    {
        Time.timeScale = 0;
        if (win)
        {
            GetTime();
            int bminutes = (int)bestTime / 60;
            float bseconds = (bestTime - (bminutes * 60));
            bestTimeText.text = bminutes.ToString() + ":" + bseconds.ToString("00.0");

            int minutes = (int)time / 60;
            float seconds = (time - (minutes * 60));
            currentTimeText.text = minutes.ToString() + ":" + seconds.ToString("00.0");

            if (time < bestTime || bestTime == 0)  //if player gets new best time or there was no time before
            {
                newRecordPopUp.SetActive(true);
                PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name,time);
            }
            winScreen.SetActive(true);
        }
        else //if end is caused by death 
        {
            deathScreen.SetActive(true);
        }
    }

    public void ToTitle()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
        Time.timeScale = 1;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    void GetTime()
    {
        time = timer.GetTime();
    }

}
