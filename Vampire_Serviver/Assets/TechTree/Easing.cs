using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Easing
{
    public static float easeInSine(float x)
    {
        return 1 - Mathf.Cos((x * Mathf.PI) / 2);
    }
    public static float easeOutSine(float x)
    {
        return Mathf.Sin((x * Mathf.PI) / 2);
    }
}
