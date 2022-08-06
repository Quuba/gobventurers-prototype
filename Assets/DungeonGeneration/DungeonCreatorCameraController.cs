using System;
using UnityEngine;

namespace DungeonCreator
{
    public class DungeonCreatorCameraController : MonoBehaviour
    {
        [SerializeField] private float scrollSensitivity = 3f;
        [SerializeField] private bool reverseScroll;
        // private const int ScrollSensitivityMultiplier = 10;

        // [SerializeField] private float mouseSensitivity = 1f;

        private Vector3 _dragOrigin;
        
        private Camera _camera;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        private void Update()
        {
            var scrollDelta = Input.mouseScrollDelta.y;
            if (scrollDelta != 0)
            {
                var scrollAmount = scrollDelta * Time.deltaTime * scrollSensitivity * _camera.orthographicSize/3;
                if (reverseScroll)
                {
                    scrollAmount *= -1;
                }

                _camera.orthographicSize -= scrollAmount;
            }
            
            
            if (Input.GetKeyDown(KeyCode.Mouse2))
            {
                _dragOrigin = _camera.ScreenToWorldPoint(Input.mousePosition);
            }
            
            if (Input.GetKey(KeyCode.Mouse2))
            {
                var difference = _dragOrigin - _camera.ScreenToWorldPoint(Input.mousePosition);

                transform.position += difference;
            }
        }
    }
}