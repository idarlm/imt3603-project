using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR;

namespace CameraSystem
{
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
    private bool _cameraIsColliding;

    
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
        var mouseDelta = new Vector3(Input.GetAxis("Horizontal Right")*-1, 
            Input.GetAxis("Vertical Right")*-1, 0.0f)  * (Time.deltaTime * sensitivity);
        mouseDelta += (_previousMousePos - Input.mousePosition) * (Time.deltaTime * sensitivity);
        var targetPosition = targetObject.position;
        var targetMovement = _previousPlayerPos - targetPosition;
        
        UpdatePitchAndYaw(ref  targetMovement, ref mouseDelta);
        UpdateFront();
        
        var cameraPosition = targetPosition - _front * distanceToTargetObject;
        cameraPosition.y += targetObjectHeightOffset;
        
        AdjustForObstructions(ref cameraPosition);
        LerpTowardsTargetPosition(ref cameraPosition);
        UpdateLookAtDirection(ref targetMovement);
        
        _previousPlayerPos = targetPosition;
        _previousMousePos = Input.mousePosition;
    }
    
    
    /// <summary>
    /// AdjustForObstructions moves the camera closer to the target object
    /// if there is no clear line of sight from the target object to the camera position
    /// </summary>
    /// <param name="cameraTargetPosition"></param>
    private void AdjustForObstructions(ref Vector3 cameraTargetPosition)
    {
        var rayOrigin = targetObject.position + new Vector3(0.0f, targetObjectHeightOffset,0.0f);
        var sqrDistanceToCamera = (rayOrigin - cameraTargetPosition).sqrMagnitude;
        
        if (Physics.Raycast(rayOrigin, -_front, out var wallHit) &&
            Math.Pow(wallHit.distance, 2) < sqrDistanceToCamera)
        {
            _cameraIsColliding = true;
            cameraTargetPosition = wallHit.point + _front * 0.5f;
        }

        _cameraIsColliding = false;
    }


    /// <summary>
    /// UpdateFront updates the front facing vector of the camera based on
    /// _cameraYaw and _cameraPitch, the horizontal and vertical rotation in radians.
    /// </summary>
    private void UpdateFront()
    {
        _front = new Vector3(
            (float)(Math.Cos(_cameraYaw) * Math.Cos(_cameraPitch)),
            (float)Math.Sin(_cameraPitch),
            (float)(Math.Cos(_cameraPitch) * Math.Sin(-_cameraYaw))
        );
    }
    
    
    /// <summary>
    /// LerpTowardsTargetPosition interpolates from the current camera position
    /// to a provided target camera position. 
    /// </summary>
    /// <param name="cameraTargetPosition">Destination camera position</param>
    private void LerpTowardsTargetPosition(ref Vector3 cameraTargetPosition)
    {
        transform.position = Vector3.SmoothDamp(
            transform.position, 
            cameraTargetPosition, 
            ref _currentVelocity, 
            smoothTime);
    }

    /// <summary>
    /// UpdateLookAtDirection updates the lookAt direction of the camera. lookAt drifts
    /// towards the target objects actual position, but may deviate depending on _lookAheadCoefficient 
    /// </summary>
    /// <param name="targetMovement">The movement of the target object since the last frame</param>
    private void UpdateLookAtDirection(ref Vector3 targetMovement)
    {
        var lookAtTargetPosition = targetObject.transform.position 
                                   - lookAheadCoefficient * targetMovement.normalized;
        lookAtTargetPosition.y += targetObjectHeightOffset;
        
        // TODO. No control of how quickly the camera catches up in real time.
        _lookAtTargetPosition = Vector3.Lerp(
            _lookAtTargetPosition,
            lookAtTargetPosition,
            Time.deltaTime * (_cameraIsColliding ? 15.0f : 5.0f)
        );
        transform.LookAt(_lookAtTargetPosition);
    }


    /// <summary>
    /// UpdatePitchAndYaw updates the horizontal and lateral rotation of the camera anchor
    /// position around the target object. A horizontalDrift > 0 will cause the camera to track
    /// target movement by rotating.
    /// </summary>
    /// <param name="targetMovement"></param>
    /// <param name="controllerXYDelta"></param>
    private void UpdatePitchAndYaw(ref Vector3 targetMovement, ref Vector3 controllerXYDelta)
    {
        var targetObjectHorizontalMovement = 
            (targetMovement.x * _front.z - targetMovement.z * _front.x) * horizontalDrift;
        
        _cameraYaw -= controllerXYDelta.x + targetObjectHorizontalMovement;
        _cameraPitch = Math.Max(Math.Min(_cameraPitch -= controllerXYDelta.y, .5f),- 1.25f);
    }
}
}
