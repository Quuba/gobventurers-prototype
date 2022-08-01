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
        private EnemyState _currentState;

        private Vector3 _playerPos;
        private Rigidbody rb;
        private Enemy _enemy;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
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
                    Vector3 enemyPos = transform.position;
                    Vector3 playerDir = _playerPos - enemyPos;
                    playerDir.y = 0f;
                    rb.MovePosition(enemyPos + playerDir.normalized * Time.deltaTime);
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
        FollowPlayer
    }
}