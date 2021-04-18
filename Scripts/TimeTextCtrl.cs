using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeTextCtrl : MonoBehaviour
{
    public Text timeText;
    private int time;
    private int timedif;
    // Start is called before the first frame update
    void Start()
    {
        timeText.text = 3.ToString();
        time = System.DateTime.Now.Millisecond;
        timedif = System.DateTime.Now.Millisecond;
    }

    void Update()
    {
        timedif = (System.DateTime.Now.Millisecond - time) / 100;

        Debug.Log(int.Parse(timeText.text));
        Debug.Log(timedif);

        string output = ((int)(3 - timedif / 10)).ToString();

        if (int.Parse(output) < 1)
        {
            Time.timeScale = 1;
            Destroy(gameObject);
        }

        timeText.text = output;
    }
}
