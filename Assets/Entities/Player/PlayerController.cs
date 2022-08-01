using System;
using Player;
using UnityEngine;

namespace Entities.Player
{
    public class PlayerController : MonoBehaviour
    {
        public event Action<PlayerState> PlayerStateChanged;

        private PlayerState _playerState = PlayerState.Default;

        public void SetPlayerState(PlayerState newState)
        {
            PlayerStateChanged?.Invoke(newState);
            _playerState = newState;
        }

        public PlayerState GetPlayerState()
        {
            return _playerState;
        }
    }
}