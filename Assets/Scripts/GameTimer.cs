using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public Text TimerText;
    public float TimeLeftSeconds;

    public UnityEvent TimerEnd;
    // Start is called before the first frame update
    void Start()
    {
        TimerText.text = TimeLeftSeconds.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeLeftSeconds > 0)
        {
            TimeLeftSeconds -= Time.deltaTime;

            TimerText.text = string.Format("{0:F1}", TimeLeftSeconds);

        }

        if(TimeLeftSeconds <= 0 )
        {
            TimerEnd?.Invoke();
        }
    }
}
