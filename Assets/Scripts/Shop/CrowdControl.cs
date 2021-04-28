using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Extension
{
    public static IEnumerable<U> Map<T, U>(this IEnumerable<T> s, Func<T, U> f)
    {
        foreach (var item in s)
            yield return f(item);
    }
}
public class CrowdControl : MonoBehaviour
{

    private Folk[] _folk;
    // Start is called before the first frame update
    void Start()
    {
        _folk = GetComponentsInChildren<Folk>();

        var scoresThing = FindObjectOfType<GameTimer>();

        _folk.ToList().ForEach(s=> s.gameObject.SetActive(false));
        _folk.Where(w => w.type == PoiType.Sheep).Take((int)scoresThing.scoreSheep).ToList().ForEach(s=> s.gameObject.SetActive(true));
        _folk.Where(w => w.type == PoiType.Alien).Take((int)scoresThing.score).ToList().ForEach(s=> s.gameObject.SetActive(true));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
