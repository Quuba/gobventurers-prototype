using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    

    [Space(10)] [SerializeField] private Optional<FloatBounds> distanceBounds;
    [SerializeField] private float playerDistance = 5f;

    [Space(10)] [SerializeField] private Optional<FloatBounds> angleXBounds;
    [SerializeField] private float angleX = 60f;
    [SerializeField] private float angleY = -90f;

    [SerializeField] private bool useSmoothing = true;
    [SerializeField] private float smoothTime = 0.25f;
    
    private Transform _transform;

    private Vector3 _velocity = Vector3.zero;
    private Vector3 _targetPos;

    // move curve
    private void Start()
    {
        // _lastTargetPos = playerTransform.position;
    }

    // private void Update()
    // {
    //     _transform = transform;
    //     // var camPos = _transform.position;
    //
    //     var playerPos = playerTransform.position;
    //
    //     var dirVec = new Vector3(playerPos.x - _lastTargetPos.x, 0f, playerPos.z - _lastTargetPos.z).normalized;
    //     dirVec = moveSpeed * Time.deltaTime * dirVec;
    // }

    private void FixedUpdate()
    {
        _targetPos = playerTransform.position;

        SetTransformRelativeToPoint(_targetPos);

        // _lastTargetPos = _targetPos;
    }

    private void OnValidate()
    {
        SetTransformRelativeToPoint(playerTransform.position);
    }

    private void SetTransformRelativeToPoint(Vector3 targetPoint)
    {
        _transform = transform;

        if (angleXBounds.Enabled)
        {
            angleX = Mathf.Clamp(angleX, angleXBounds.Value.lower, angleXBounds.Value.upper);
        }

        if (distanceBounds.Enabled)
        {
            playerDistance = Mathf.Clamp(playerDistance, distanceBounds.Value.lower, distanceBounds.Value.upper);
        }

        Vector3 targetPos = targetPoint;
        Vector3 newCamPos = new Vector3();
        newCamPos.x = Mathf.Cos(angleY * Mathf.Deg2Rad) * Mathf.Cos(angleX * Mathf.Deg2Rad);
        newCamPos.z = Mathf.Sin(angleY * Mathf.Deg2Rad) * Mathf.Cos(angleX * Mathf.Deg2Rad);

        newCamPos.y = Mathf.Sin(angleX * Mathf.Deg2Rad);

        // _transform.position = targetPos + newCamPos * playerDistance;

        //this guy (https://youtu.be/ZBj3LBA2vUY) says it's good
        if (useSmoothing)
        {
            _transform.position = Vector3.SmoothDamp(_transform.position, targetPos + newCamPos * playerDistance,
                ref _velocity, smoothTime);
        }
        else
        {
            _transform.position = targetPos + newCamPos * playerDistance;
        }
        

        _transform.localEulerAngles = new Vector3(angleX, -angleY - 90f, 0f);
    }

    [Serializable]
    private struct FloatBounds
    {
        public float upper;
        public float lower;

        public FloatBounds(float upper, float lower)
        {
            this.upper = upper;
            this.lower = lower;
        }
    }
}