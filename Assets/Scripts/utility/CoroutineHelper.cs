using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class CoroutineHelper
{
    public static void RunMethodAfterThisManySeconds(this MonoBehaviour monoBehaviour, Action function, float seconds)
    {
        monoBehaviour.StartCoroutine(GenericEnumeratorWait(function, seconds));
    }
    private static IEnumerator GenericEnumeratorWait(Action function, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        function();
    }
}

