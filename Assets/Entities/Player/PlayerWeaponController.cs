using System;
using Items.Weapons;
using UnityEditor;
using UnityEngine;

namespace Entities.Player
{
    public class PlayerWeaponController : MonoBehaviour
    {
        [SerializeField] private Weapon currentWeapon;
        private PlayerState _playerState;

        private PlayerController _playerController;

        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();
        }

        private void OnEnable()
        {
            _playerController.PlayerStateChanged += HandlePlayerStateChanged;
        }

        private void OnDisable()
        {
            _playerController.PlayerStateChanged -= HandlePlayerStateChanged;
        }

        private void Update()
        {
            switch (_playerState)
            {
                case PlayerState.Default:
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        
                        PrimaryAttack();
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

        private void PrimaryAttack()
        {
            Debug.Log("test attack");
            currentWeapon.UsePrimaryAction();
        }

        private void HandlePlayerStateChanged(PlayerState newState)
        {
            _playerState = newState;
        }
    }
}