using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideBoost : MonoBehaviour
{
    public float maxBoostTime;
    public float force;
    public float destroyerOffDelay;
    public LinkerSO linker;
    bool _toRight;
    float _timeElapsed = 0;
    PlayerVarsSO _playerVars;
    private void Awake()
    {
        _playerVars = linker.playerVars;
    }
    public void SetDirection(bool toRight)
    {
        _toRight = toRight;
    }
    public void ResetTime()
    {
        _timeElapsed = 0;
        if (maxBoostTime <= 0)
        {
            maxBoostTime = 0.05f;
        }
    }
    public bool HasLeftTime()
    {
        return _timeElapsed >= maxBoostTime ? false : true;
    }
    public Vector3 Step(Transform boostTransform)
    {
        if (_timeElapsed < maxBoostTime)
        {
            _playerVars.SetIsDestroyer(true);

            _timeElapsed += Time.fixedDeltaTime;
            float smoothForce = Mathf.Lerp(0, force, _timeElapsed / maxBoostTime);
            return ((_toRight ? smoothForce : -smoothForce) * boostTransform.right * Time.fixedDeltaTime);
        }
        else
        {
            _playerVars.SetIsDestroyer(false);
            _playerVars.sideBoost.DecreaseCount();
            _playerVars.sideBoost.EndUse();
            _playerVars.SetCurrentActiveAbility(Collectible.none);
            return Vector3.zero;
        }
    }


}

