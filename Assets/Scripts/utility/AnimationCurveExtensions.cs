using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public static class AnimationCurveExtensions
{
    public static float RandomFromCurve(this AnimationCurve sizeDistribution, float min, float max)
    {
        return (sizeDistribution.Evaluate(Random.value) * max - min) + min;
    }
}

