using System;
using UnityEngine;

namespace Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private Camera playerCamera;

        [Header("Movement settings")]
        [SerializeField] private float baseMovementSpeed = 4f;

        [Header("Dash settings")]
        [SerializeField] private float dashCooldown = 0.8f;

        [SerializeField] private float dashLength = 2f;
        [SerializeField] private float dashTime = 0.8f;

        [Header("Mouse Settings")]
        [SerializeField] private LayerMask mousePlaneLayerMask;

        [Header("Debug")]
        [SerializeField] private bool drawMousePos = false;

        private float _mouseRaycastMaxDistance = 50f;

        private float _dashTimer = 0f;
        private float _dashCooldownTimer = 0f;
        private Vector3 _dashTarget;
        private Vector3 _dashStartPos;

        private float _inputX = 0f, _inputV = 0f;
        private PlayerState _playerState;

        private Rigidbody _rb;
        private Transform _transform;
        private Vector3 _mouseWorldPos;


        // Start is called before the first frame update
        void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _transform = transform;
        }

        // Update is called once per frame
        void Update()
        {
            switch (_playerState)
            {
                case PlayerState.Default:
                    //TODO: find out why using normal GetAxis here gives floaty movement 
                    _inputX = Input.GetAxisRaw("Horizontal");
                    _inputV = Input.GetAxisRaw("Vertical");

                    RotateTowardsMouse();

                    if (Input.GetKeyDown(KeyCode.Space) && _dashCooldownTimer <= 0)
                    {
                        Dash();
                    }

                    if (_dashCooldownTimer >= 0f)
                    {
                        _dashCooldownTimer -= Time.deltaTime;
                    }

                    break;
                case PlayerState.Dashing:
                    _rb.MovePosition(Vector3.Lerp(_dashStartPos, _dashTarget, _dashTimer / dashTime));
                    if (_dashTimer >= dashTime)
                    {
                        _dashCooldownTimer = dashCooldown;
                        _playerState = PlayerState.Default;
                    }
                    else
                    {
                        _dashTimer += Time.deltaTime;
                    }

                    break;
                case PlayerState.Stunned:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void FixedUpdate()
        {
            switch (_playerState)
            {
                case PlayerState.Default:
                    //this guy (https://youtu.be/ixM2W2tPn6c) says it's a good method and he's right
                    var direction = new Vector3(_inputX, 0f, _inputV).normalized * baseMovementSpeed;
                    _rb.MovePosition(_transform.position + (baseMovementSpeed * Time.deltaTime * direction));
                    break;
                case PlayerState.Dashing:
                    break;
                case PlayerState.Stunned:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Dash()
        {
            var playerPos = _transform.position;
            _dashTarget = playerPos + new Vector3(_inputX, 0f, _inputV).normalized * dashLength;
            _dashStartPos = playerPos;
            _dashTimer = 0f;

            _playerState = PlayerState.Dashing;
        }

        private void RotateTowardsMouse()
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, _mouseRaycastMaxDistance, mousePlaneLayerMask))
            {
                _mouseWorldPos = hit.point;
                _mouseWorldPos.y = _transform.position.y;
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (!drawMousePos || _transform == null )
            {
                return;
            }

            Gizmos.color = Color.red;
            Gizmos.DrawLine(_transform.position, _mouseWorldPos);
            Debug.Log(_mouseWorldPos);
        }

        private enum PlayerState
        {
            Default,
            Dashing,
            Stunned
        }
    }
}