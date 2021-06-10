using UnityEngine;

public class MinMaxAttribute : PropertyAttribute
{
    public float MinLimit = 0;
    public float MaxLimit = 1;
    public bool ShowEditRange;
    public bool ShowDebugValues;

    public MinMaxAttribute(float min, float max)
    {
        MinLimit = min;
        MaxLimit = max;
    }
}

public class MinMaxValue
{
    public float min;
    public float max;
    public MinMaxValue(float min, float max)
    {
        this.min = min;
        this.max = max;
    }
}
public class MinMaxCurrentValue
{
    private float min;
    public float Min
    {
        get { return min; }
        set
        {
            min = value;
            if (current < min) current = min;
        }
    }
    private float current;
    public float Current
    {
        get { return current; }
        set
        {
            current = value;
            if (current < min) current = min;
            if (max < current) current = max;
        }
    }
    private float max;
    public float Max
    {
        get { return max; }
        set
        {
            max = value;
            if (max < current) current = max;
        }
    }
    public MinMaxCurrentValue(float min, float max)
    {
        this.min = min;
        this.max = max;
        this.current = (min + max) * 0.5f;
    }
    public MinMaxCurrentValue(float min, float current, float max)
    {
        this.min = min;
        this.current = current;
        this.max = max;
    }
}