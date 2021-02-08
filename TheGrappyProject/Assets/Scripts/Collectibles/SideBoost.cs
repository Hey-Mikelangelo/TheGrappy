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
    PlayerVarsSO _playerVars;
    public void SetSideBoost(float maxTime, float force, bool toRight, PlayerVarsSO playerVars)
    {
        _maxBoostTime = maxTime;
        _force = force;
        _toRight = toRight;
        _playerVars = playerVars;
    }
    public void ResetSideBoost()
    {
        timeElapsed = 0;
        if (_maxBoostTime <= 0)
        {
            _maxBoostTime = 0.05f;
        }
    }
    public Vector3 SideBoostStep(Transform boostTransform)
    {
        if (timeElapsed < _maxBoostTime)
        {
            _playerVars.isDestroyer = true;

            timeElapsed += Time.deltaTime;
            float smoothForce = Mathf.Lerp(0, _force, timeElapsed / _maxBoostTime);
            return ((_toRight ? smoothForce : -smoothForce) * boostTransform.right * Time.deltaTime);
        }
        else
        {
            _playerVars.isDestroyer = false;
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

