using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfInterestChooser : MonoBehaviour
{
    public PointOfInterest Alien;
    public PointOfInterest Sheep;

    private void Start()
    {
        if (UnityEngine.Random.value > 0.5)
        {
            Alien.gameObject.SetActive(true);
            Sheep.gameObject.SetActive(false);
            Sheep.Deactivate();
        }
        else
        {
            Alien.gameObject.SetActive(false);
            Alien.Deactivate();
            Sheep.gameObject.SetActive(true);
        }
    }
}
