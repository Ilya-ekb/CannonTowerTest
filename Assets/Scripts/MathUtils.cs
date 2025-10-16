using UnityEngine;

public static class MathUtils
{
    public static bool ApproximatelyEqual(float a, float b, float tolerance = 0.001f)
        => Mathf.Abs(a - b) <= tolerance;
}