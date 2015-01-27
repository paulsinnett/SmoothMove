using UnityEngine;
using System.Collections;

public class SmoothMove : MonoBehaviour
{
	public AnimationCurve curve;
	public AnimationCurve positionCurve;
	public AnimationCurve velocityCurve;
	float start = 10f;
	float end = 20f;
	float velocity = 0f;

	void Start()
	{
		Invoke ("Test", 1f);
	}

	void Test ()
	{
		StartCoroutine (doTest ());
	}

	IEnumerator doTest ()
	{
		curve = new AnimationCurve ();
		positionCurve = new AnimationCurve ();
		velocityCurve = new AnimationCurve ();
		float value = start;
		for (float time = 0f; time < 1f; time += Time.deltaTime) 
		{
			curve.AddKey(time, value = Smooth (value, ref velocity, end, 0f, 1f - time));
			yield return null;
		}
	}

	float Smooth(float current, ref float currentVelocity, float target, float targetVelocity, float time)
	{
		float value = target;
		if (time > 0f) 
		{
			float positionAcceleration = 2f * (target - current - currentVelocity * time) / (time * time);
			float velocityAcceleration = (targetVelocity - currentVelocity) / time;
			float acceleration = positionAcceleration;
			if (Mathf.Abs (velocityAcceleration) > Mathf.Abs (positionAcceleration))
			{
				acceleration = velocityAcceleration;
			}

		    value = current + currentVelocity * Time.deltaTime + 0.5f * (acceleration * Time.deltaTime * Time.deltaTime);
		    currentVelocity = currentVelocity + acceleration * Time.deltaTime;

			positionCurve.AddKey(1f - time, positionAcceleration);
			velocityCurve.AddKey(1f - time, velocityAcceleration);
		}
		else
		{
			currentVelocity = targetVelocity;
		}

		return value;
	}
}
