using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public Text TimerText;
    public Text GameOver;
    public Image Health;
    public GameObject LaunchButton;

    public float shopTime = 30;
    public float flightTime = 60;
    public float scoreMultiplier = 15;

    private float _timeLeftSeconds;
    private ShipController _player;
    private bool _once;

    public UnityEvent TimerEnd;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<ShipController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!_once && _player.dead)
        {
            Health?.gameObject.SetActive(false);
            LaunchButton?.gameObject.SetActive(true);
            GameOver?.gameObject.SetActive(true);
            TimerText?.gameObject.SetActive(false);
        }

        if (_timeLeftSeconds > 0)
        {
            _timeLeftSeconds -= Time.deltaTime;

            TimerText.text = string.Format("{0:F1}", _timeLeftSeconds);

        }

        if(_timeLeftSeconds <= 0 )
        {
            TimerEnd?.Invoke();
        }
    }

    public void SetTimer(GameData gameData)
    {
        if (SceneManager.GetActiveScene().name == "Shop")
        {
            FindObjectOfType<GameTimer>()._timeLeftSeconds = shopTime + ((gameData.score + 1) * scoreMultiplier);
        }
        else
        {
            FindObjectOfType<GameTimer>()._timeLeftSeconds = flightTime + ((gameData.score + 1) * scoreMultiplier);
        }
    }
}
