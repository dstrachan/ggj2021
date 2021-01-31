using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public Text TimerText;
    public Text GameOver;

    public float TimeLeftSeconds;
    private ShipController _player;
    private bool _once;

    public UnityEvent TimerEnd;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<ShipController>();

        TimerText.text = TimeLeftSeconds.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(!_once && _player.dead)
        {
            TimerText.gameObject.SetActive(false);
        }

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
