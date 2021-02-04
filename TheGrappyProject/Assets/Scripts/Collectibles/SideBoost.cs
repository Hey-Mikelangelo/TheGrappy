using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideBoost : MonoBehaviour
{
    float _maxBoostTime;
    bool _toRight;
    float _force;
    float timeElapsed = 0;
    Coroutine boostCoroutine;
    
    public void SetSideBoost(float maxTime, float force, bool toRight)
    {
        _maxBoostTime = maxTime;
        _force = force;
        _toRight = toRight;
    }
    public void ResetSideBoost()
    {
        timeElapsed = 0;
    }
    public Vector3 SideBoostStep(Transform boostTransform)
    {
        if (_maxBoostTime <= 0)
        {
            _maxBoostTime = 0.05f;
        }
        if (timeElapsed < _maxBoostTime)
        {
            timeElapsed += Time.deltaTime;
            float smoothForce = Mathf.Lerp(0, _force, timeElapsed / _maxBoostTime);
            return ((_toRight ? smoothForce : -smoothForce) * boostTransform.right * Time.deltaTime);
        }
        else
        {
            return Vector3.zero;
        }
    }
    public void StartBoost()
    {
        //boostCoroutine = StartCoroutine(Boost(transform, force, toRight));
    }
    IEnumerator Boost(Transform boostTransform, float force, bool toRight)
    {
        float timeElapsed = 0;

        while (timeElapsed < _maxBoostTime)
        {
            timeElapsed += Time.deltaTime;
            float smoothForce = Mathf.Lerp(0, force, timeElapsed / _maxBoostTime);
            boostTransform.position += (toRight ? smoothForce : -smoothForce) * boostTransform.right * Time.deltaTime;
            yield return null;
        }

    }
}

