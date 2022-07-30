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
        [SerializeField] private AnimationCurve dashCurve;
        [SerializeField] private float dashWallCheckDistance;
        [SerializeField] private LayerMask dashCollisionLayerMask;


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

        // private Vector3 _mouseWorldPos;
        private Vector3 _mouseDir;
        private PlayerState _playerState;

        private Rigidbody _rb;
        private Transform _transform;


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
                    //BUG: Gives floaty movement when using Input.GetAxis instead of GetAxisRaw
                    var direction = new Vector3(_inputX, 0f, _inputV).normalized * baseMovementSpeed;
                    _rb.MovePosition(_transform.position + (baseMovementSpeed * Time.deltaTime * direction));
                    break;
                case PlayerState.Dashing:
                    if (_dashTimer >= dashTime)
                    {
                        _dashCooldownTimer = dashCooldown;
                        _playerState = PlayerState.Default;
                    }
                    else
                    {
                        _dashTimer += Time.deltaTime;
                    }

                    if (CheckForWalls((_dashTarget - _dashStartPos).normalized, dashWallCheckDistance, dashCollisionLayerMask))
                    {
                        // _dashCooldownTimer = dashCooldown;
                        // _playerState = PlayerState.Default;
                        break;
                    }

                    _rb.MovePosition(Vector3.Lerp(_dashStartPos, _dashTarget, dashCurve.Evaluate(_dashTimer / dashTime)));


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

        private bool CheckForWalls(Vector3 dir, float distance, LayerMask layerMask)
        {
            Ray ray = new Ray(_transform.position, dir);
            if (Physics.Raycast(ray, distance, layerMask))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void RotateTowardsMouse()
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, _mouseRaycastMaxDistance, mousePlaneLayerMask))
            {
                var playerPos = _transform.position;
                Vector3 mouseWorldPos = hit.point;
                mouseWorldPos.y = playerPos.y;

                _mouseDir = (mouseWorldPos - playerPos).normalized;
                _transform.forward = _mouseDir;
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (!drawMousePos || _transform == null)
            {
                return;
            }

            var playerPos = _transform.position;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(playerPos, playerPos + _mouseDir);
            // Debug.Log();
        }

        private enum PlayerState
        {
            Default,
            Dashing,
            Stunned
        }
    }
}