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
        if (UnityEngine.Random.value > 0.7)
        {
            Alien.gameObject.SetActive(true);
            Sheep.gameObject.SetActive(false);
        }
        else
        {
            Alien.gameObject.SetActive(false);
            Sheep.gameObject.SetActive(true);
        }
    }
}
