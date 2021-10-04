using UnityEngine;

public class MinMaxAttribute : PropertyAttribute
{
    public float MinLimit = 0f;
    public float MaxLimit = 1f;
    public bool ShowEditRange;
    public bool ShowDebugValues;

    public MinMaxAttribute(float min, float max)
    {
        MinLimit = min;
        MaxLimit = max;
    }
}