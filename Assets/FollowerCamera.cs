using System;
using UnityEngine;
using UnityEngine.Serialization;


public class FollowerCamera : MonoBehaviour
{
    [SerializeField] public Transform target;
    public float distance = 3;
    public float smoothTime = 0.25f;
    public float sensitivity = 0.25f;
    [SerializeField] private float targetHeightOffset = 3.0f;
    
    private Vector3 _currentVelocity= Vector3.zero;
    private float _cameraYaw;
    private float _cameraPitch;
    private Vector3 _previousMousePos;
    private Vector3 _previousPlayerPos;
    private Vector3 _front;
    
    private void Start()
    {
        _previousMousePos = Input.mousePosition;
        _previousPlayerPos = target.position;
        _front = new Vector3(
            (float)(Math.Cos(_cameraYaw) * Math.Cos(_cameraPitch)),
            (float)Math.Sin(_cameraPitch),
            (float)(Math.Cos(_cameraPitch) * Math.Sin(-_cameraYaw))
        );
    }
    private void LateUpdate()
    {
        var mouseDelta = (_previousMousePos - Input.mousePosition) * (Time.deltaTime * sensitivity);
        var playerPosition = this.target.position;
        _front.y = 0;
        _front.Normalize();
        var playerDelta = _previousPlayerPos - playerPosition;
        var playerSidewaysMovement = playerDelta.x * _front.z - playerDelta.z * _front.x;
        playerSidewaysMovement *= 0.1f;
        
        
        _previousPlayerPos = playerPosition;
        _previousMousePos = Input.mousePosition;
        _cameraYaw -= mouseDelta.x + playerSidewaysMovement;
        _cameraPitch = Math.Max(Math.Min(_cameraPitch -= mouseDelta.y, -0.25f),- 1.25f);

        _front = new Vector3(
            (float)(Math.Cos(_cameraYaw) * Math.Cos(_cameraPitch)),
            (float)Math.Sin(_cameraPitch),
            (float)(Math.Cos(_cameraPitch) * Math.Sin(-_cameraYaw))
        );
        
        var target = playerPosition - _front * distance;
        transform.position = Vector3.SmoothDamp(transform.position, target, ref _currentVelocity, smoothTime);
        var playerTransform = this.target.transform;
        transform.LookAt(playerTransform);
    }
}