using UnityEngine;
using System.Collections;

// This class smooths a change in value over time like SmoothDamp, but it uses an AnimationCurve
// to hit the target value at the desired time.
// An instance of SmoothMove can also tell you when it's finished its move.
public class SmoothMove
{
    AnimationCurve curve;
    float derivative;
    float current;
    float time;

    public SmoothMove()
    {
        curve = new AnimationCurve();
        curve.AddKey(0f, 0f);
        curve.AddKey(1f, 0f);
    }

    public void Target(float currentValue, float targetValue, float duration)
    {
        derivative = 0f;
        Retarget(currentValue, targetValue, duration);
    }

    public void Retarget(float currentValue, float targetValue, float duration)
    {
        Keyframe[] keys = curve.keys;
        keys[0].time = 0f;
        keys[0].value = currentValue;
        keys[0].inTangent = derivative;
        keys[0].outTangent = derivative;
        keys[1].time = duration;
        keys[1].value = targetValue;
        keys[1].inTangent = 0f;
        keys[1].outTangent = 0f;

        curve.keys = keys;
        time = 0f;
        current = currentValue;
    }

    public float Update()
    {
        time += Time.deltaTime;
        float newValue = curve.Evaluate(time);
        float acceleration = (newValue - current - (derivative * Time.deltaTime)) * (2f / (Time.deltaTime * Time.deltaTime));
        derivative += acceleration * Time.deltaTime;
        current = newValue;
        return current;
    }

    public bool Finished()
    {
        return time >= curve.keys[1].time;
    }
}
