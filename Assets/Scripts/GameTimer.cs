using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public Text timerText;
    public Text gameOver;
    public Image health;
    public Text scoreText;
    public Text scoreSheepText;
    public Text scoreEnd;
    public Text scoreSheepEnd;

    public GameObject launchButton;

    public float shopTime = 30;
    public float flightTime = 60;
    public float scoreMultiplier = 15;

    public float score = 0;
    public float scoreSheep = 0;

    private float _timeLeftSeconds;
    private ShipController _player;
    private bool _once;

    public UnityEvent TimerEnd;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<ShipController>();

        if (SceneManager.GetActiveScene().name == "Shop")
        {
            _timeLeftSeconds = shopTime;
        }
        else
        {
            _timeLeftSeconds = flightTime;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(!_once && _player.dead)
        {
            launchButton?.gameObject.SetActive(true);
            gameOver?.gameObject.SetActive(true);
            scoreEnd?.gameObject.SetActive(true);
            scoreSheepEnd?.gameObject.SetActive(true);

            health?.gameObject.SetActive(false);
            scoreText?.gameObject.SetActive(false);
            scoreSheepText?.gameObject.SetActive(false);


            scoreEnd.text = $"You rescued {score} galactic citizens!";
            scoreSheepEnd.text = $"You rescued {score} space sheep!";

            timerText?.gameObject.SetActive(false);
        }

        if (_timeLeftSeconds > 0)
        {
            _timeLeftSeconds -= Time.deltaTime;

            timerText.text = string.Format("{0:F1}", _timeLeftSeconds);

        }

        if (scoreText != null)
        {
            scoreText.text = $"Citizens rescued: {score}";
        }

        if (scoreSheepText != null)
        {
            scoreSheepText.text = $"Sheep rescued: {scoreSheep}";
        }

        if (_timeLeftSeconds <= 0 )
        {
            TimerEnd?.Invoke();
        }
    }


}
