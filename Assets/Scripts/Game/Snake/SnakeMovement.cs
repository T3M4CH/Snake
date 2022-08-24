using System;
using Game.Input.Interfaces;
using Mirror;
using UnityEngine;
using Zenject;

namespace Game.Snake
{
    public class SnakeMovement : NetworkBehaviour
    {
        [Inject]
        public void Construct(ISwipeHandler swipeHandler)
        {
            _swipeHandler = swipeHandler;
        }

        [SyncVar] private Vector2Int _direction;

        private ISwipeHandler _swipeHandler;

        public Vector3Int GetMovePoint(Vector2Int position)
        {
            if (position.x is > 5 or < -5) position.x *= -1;
            if (position.y is > 5 or < -5) position.y *= -1;
            return new Vector3Int(position.x + _direction.x, position.y + _direction.y);
        }

        [Command]
        private void CmdChangeDirection(Vector2Int direction)
        {
            Debug.Log("Change");
            if (_direction == direction * -1) return;
            _direction = direction;
        }

        private void Start()
        {
            if (!isLocalPlayer) return;
            _swipeHandler.OnSwipe += CmdChangeDirection;
            _direction = Vector2Int.up;
        }
    }
}