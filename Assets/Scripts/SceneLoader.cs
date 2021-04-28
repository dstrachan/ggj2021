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
        //if (Input.GetMouseButtonDown(1))
        //{       
        //    LoadNextScene();
        //}
    }

    public void LoadNextScene()
    {
        _shipLoader.Save();
        var nextScene = SceneManager.GetActiveScene().name == _scene1 ? _scene2 : _scene1;

        if(nextScene == _scene2)
        {
            Physics.gravity = new Vector3(0, 0, 0);
        }
        else
        {
            // gravity in the shop for components to fall onto conveyor
            Physics.gravity = new Vector3(0, -9.1f, 0);
        }

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
