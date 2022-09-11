using System;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(Enemy))]
    [RequireComponent(typeof(Rigidbody))]
    public class EnemyMovementController : MonoBehaviour
    {
        [SerializeField] private EnemyManager enemyManager;
        [SerializeField] private EnemyMovementType movementType;
        [SerializeField] private float baseMoveSpeed = 5f;
        private EnemyState _currentState;

        private Vector3 _playerPos;
        private Rigidbody _rb;
        private Enemy _enemy;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _enemy = GetComponent<Enemy>();
            _currentState = _enemy.GetEnemyState;
        }

        private void OnEnable()
        {
            _enemy.EnemyStateChanged += HandleEnemyStateChange;
        }

        private void OnDisable()
        {
            _enemy.EnemyStateChanged -= HandleEnemyStateChange;
        }

        // Update is called once per frame
        void Update()
        {
            switch (_currentState)
            {
                case EnemyState.Default:
                    break;
                case EnemyState.Stunned:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (movementType)
            {
                case  EnemyMovementType.Stop:
                    
                case EnemyMovementType.FollowPlayer:
                    _playerPos = enemyManager.GetPlayerPosition();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void FixedUpdate()
        {
            switch (_currentState)
            {
                case EnemyState.Default:
                    if (movementType == EnemyMovementType.FollowPlayer)
                    {
                        Vector3 enemyPos = transform.position;
                        Vector3 playerDir = _playerPos - enemyPos;
                        playerDir.y = 0f;
                        _rb.MovePosition(enemyPos + playerDir.normalized * (baseMoveSpeed * Time.deltaTime));
                    }
                    break;
                case EnemyState.Stunned:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void HandleEnemyStateChange(EnemyState newState)
        {
            _currentState = newState;
        }
    }

    enum EnemyMovementType
    {
        Stop,
        FollowPlayer
    }
}