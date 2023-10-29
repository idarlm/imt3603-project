using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatIKController : MonoBehaviour
{
    private Animator _animator;
    private Vector3 _lookAtTarget;
    private bool _shouldLookAt;
    private float _transitionTime = 1f;
    private float _weight;
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetLookAtTarget(Transform lookAtTarget)
    {
        _lookAtTarget = lookAtTarget.position;
    }

    public void SetLookAtTarget(Vector3 lookAtTarget)
    {
        _lookAtTarget = lookAtTarget;
    }

    public void EnableLookAt()
    {
        _shouldLookAt = true;
    }

    public void DisableLookAt()
    {
        _shouldLookAt = false;
    }
    
    private void OnAnimatorIK(int layerIndex)
    {
        if (_shouldLookAt)
        {
            _animator.SetLookAtPosition(_lookAtTarget);
            _weight += Time.deltaTime;
        }
        else
        {
            _weight -= Time.deltaTime;
        }
        _weight = Math.Clamp(_weight, 0f, 1f);
        _animator.SetLookAtWeight(_weight);
    }
}
