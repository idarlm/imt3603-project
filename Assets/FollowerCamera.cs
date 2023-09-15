using System;
using UnityEngine;
using UnityEngine.Serialization;


public class FollowerCamera : MonoBehaviour
{
    
    public float distanceToTargetObject = 3;
    public float smoothTime = 0.25f;
    public float sensitivity = 0.25f;
    
    
    [SerializeField] private Transform targetObject;
    [SerializeField] private float targetObjectHeightOffset;
    [SerializeField] private float horizontalDrift = 0.1f;
    [SerializeField] private float lookAheadCoefficient = 1.0f;
    
    private float   _cameraYaw;
    private float   _cameraPitch;
    private Vector3 _front;
    
    private Vector3 _currentVelocity;
    private Vector3 _previousMousePos;
    private Vector3 _previousPlayerPos;
    private Vector3 _lookAtTargetPosition;

    
    private void Start()
    {
        _previousMousePos = Input.mousePosition;
        _previousPlayerPos = targetObject.position;
        _front = new Vector3(
            (float)(Math.Cos(_cameraYaw) * Math.Cos(_cameraPitch)),
            (float)Math.Sin(_cameraPitch),
            (float)(Math.Cos(_cameraPitch) * Math.Sin(-_cameraYaw))
        );
        _lookAtTargetPosition = targetObject.transform.position;
    }

    
    private void LateUpdate()
    {
        // TODO: get Generic XY delta
        var mouseDelta = (_previousMousePos - Input.mousePosition) * (Time.deltaTime * sensitivity);
        var targetPosition = targetObject.position;
        var targetMovement = _previousPlayerPos - targetPosition;
        
        UpdatePitchAndYaw(ref  targetMovement, ref mouseDelta);
        UpdateFront();
        
        var cameraPosition = targetPosition - _front * distanceToTargetObject;
        cameraPosition.y += targetObjectHeightOffset;
        
        AdjustForObstructions(ref cameraPosition);
        SmoothTowardsTargetCameraPosition(ref cameraPosition);
        UpdateLookAtDirection(ref targetMovement);
        
        _previousPlayerPos = targetPosition;
        _previousMousePos = Input.mousePosition;
    }
    
    
    
    
    
    
    
    private void AdjustForObstructions(ref Vector3 cameraTargetPosition)
    {
        var rayOrigin = targetObject.position + new Vector3(0.0f, targetObjectHeightOffset,0.0f);
        var sqrDistanceToCamera = (rayOrigin - cameraTargetPosition).sqrMagnitude;
        
        if (Physics.Raycast(rayOrigin, -_front, out var wallHit) &&
            Math.Pow(wallHit.distance, 2) < sqrDistanceToCamera)
        {
            cameraTargetPosition = wallHit.point + _front * 0.5f;
        }
    }


    private void UpdateFront()
    {
        _front = new Vector3(
            (float)(Math.Cos(_cameraYaw) * Math.Cos(_cameraPitch)),
            (float)Math.Sin(_cameraPitch),
            (float)(Math.Cos(_cameraPitch) * Math.Sin(-_cameraYaw))
        );
    }
    
    
    private void SmoothTowardsTargetCameraPosition(ref Vector3 cameraTargetPosition)
    {
        transform.position = Vector3.SmoothDamp(
            transform.position, 
            cameraTargetPosition, 
            ref _currentVelocity, 
            smoothTime);
    }


    private void UpdateLookAtDirection(ref Vector3 targetMovement)
    {
        var lookAtTargetPosition = targetObject.transform.position - lookAheadCoefficient * targetMovement.normalized;
        lookAtTargetPosition.y += targetObjectHeightOffset;
        
        // TODO. No control of how quickly the camera catches up in real time.
        _lookAtTargetPosition = Vector3.Lerp(
            _lookAtTargetPosition,
            lookAtTargetPosition,
            Time.deltaTime * 5.0f
        );
        transform.LookAt(_lookAtTargetPosition);
    }


    private void UpdatePitchAndYaw(ref Vector3 targetMovement, ref Vector3 controllerXYDelta)
    {
        var targetObjectHorizontalMovement = 
            (targetMovement.x * _front.z - targetMovement.z * _front.x) * horizontalDrift;
        
        _cameraYaw -= controllerXYDelta.x + targetObjectHorizontalMovement;
        _cameraPitch = Math.Max(Math.Min(_cameraPitch -= controllerXYDelta.y, .5f),- 1.25f);
    }
}