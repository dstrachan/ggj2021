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

    public int score = 0;
    public int scoreSheep = 0;

    public float difficultyModifier = 1;

    private float _timeLeftSeconds;
    private ShipController _player;
    private bool _once;

    public UnityEvent TimerEnd;
    // Start is called before the first frame update
    void Start()
    {
        
        var gameData = JsonUtility.FromJson<GameData>(PlayerPrefs.GetString("ship", string.Empty));
        if(gameData == null)
        {
            gameData = new GameData();
        }

        score = gameData.score;
        scoreSheep = gameData.scoreSheep;
        difficultyModifier = gameData.difficulty;

        if (SceneManager.GetActiveScene().name != "Shop")
        {
            // Some difficulty scaling?
        //    var asteroids = FindObjectOfType<AsteroidController>();

        //    asteroids.maxAsteroids = (int)(asteroids.maxAsteroids * difficultyModifier);

        //    var planets = FindObjectOfType<PlanetController>();
        //    planets.maxSpawnRadius = (int)(planets.maxSpawnRadius * difficultyModifier);
        //    planets.numberOfPlanets = (int)(planets.maxSpawnRadius * difficultyModifier);
        }

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
        if (Input.GetKeyDown(KeyCode.Escape))
        { 
            Application.Quit();
        }

        if(!_once && _player.dead)
        {
            launchButton?.gameObject.SetActive(true);
            gameOver?.gameObject.SetActive(true);
            scoreEnd?.gameObject.SetActive(true);
            scoreSheepEnd?.gameObject.SetActive(true);

            timerText?.gameObject.SetActive(false);
            health?.gameObject.SetActive(false);
            scoreText?.gameObject.SetActive(false);
            scoreSheepText?.gameObject.SetActive(false);


            scoreEnd.text = $"You rescued {score} galactic citizens!";
            scoreSheepEnd.text = $"You rescued {scoreSheep} space sheep!";

            PlayerPrefs.DeleteAll();
        }


        if (SceneManager.GetActiveScene().name != "Shop" && FindObjectsOfType<PointOfInterest>().Length == 0)
        {
            // just end the round if we picked everyone up
            _timeLeftSeconds = 0;
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

        if (_timeLeftSeconds <= 0 && !_player.dead)
        {
            TimerEnd?.Invoke();
        }

    }



}
