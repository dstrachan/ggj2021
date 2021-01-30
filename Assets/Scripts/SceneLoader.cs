using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string _scene;

    private GameObject _ship;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            var shipGrid = FindObjectOfType<ShipGrid>();
            shipGrid.ExportToPlayerPrefs("ship");
            StartCoroutine(LoadYourAsyncScene());
        }
    }

    private IEnumerator LoadYourAsyncScene()
    {
        var asyncLoad = SceneManager.LoadSceneAsync(_scene, LoadSceneMode.Single);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
