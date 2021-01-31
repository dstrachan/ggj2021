using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string _scene1 = "Shop";
    [SerializeField] private string _scene2 = "SampleScene";

    private ShipLoader _shipLoader;

    private void Start()
    {
        _shipLoader = FindObjectOfType<ShipLoader>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _shipLoader.Save();
            LoadNextScene();
        }
    }

    public void LoadNextScene()
    {
        var nextScene = SceneManager.GetActiveScene().name == _scene1 ? _scene2 : _scene1;
        StartCoroutine(LoadYourAsyncScene(nextScene));
    }

    private IEnumerator LoadYourAsyncScene(string scene)
    {
        var asyncLoad = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
