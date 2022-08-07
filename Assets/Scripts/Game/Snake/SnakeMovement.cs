using Game.TimeService.Interfaces;
using Game.Input.Interfaces;
using Mirror;
using UnityEngine;
using Zenject;

namespace Game.Snake
{
    public class SnakeMovement
    {
        public SnakeMovement(bool isLocalPlayer, Transform transform, ISwipeHandler swipeHandler)
        {
            _transform = transform;
            if (!isLocalPlayer) return;
            _direction = Vector2Int.up;
            swipeHandler.OnSwipeUp += () => CmdChangeDirection(Vector2Int.up);
            swipeHandler.OnSwipeLeft += () => CmdChangeDirection(Vector2Int.left);
            swipeHandler.OnSwipeRight += () => CmdChangeDirection(Vector2Int.right);
            swipeHandler.OnSwipeDown += () => CmdChangeDirection(Vector2Int.down);
        }

        [SyncVar]
        private Vector2Int _direction;

        private readonly Transform _transform;
        private readonly ITimeService _timeService;

        [Command]
        public void CmdMove()
        {
            var position = _transform.position;
            if (position.x is > 5 or < -5) position.x *= -1;
            if (position.y is > 5 or < -5) position.y *= -1;
            _transform.position = new Vector3Int((int)position.x + _direction.x, (int)position.y + _direction.y);
        }

        [Command]
        private void CmdChangeDirection(Vector2Int direction)
        {
            if (_direction == direction * -1) return;
            _direction = direction;
        }

        private void ChangeDirection(Vector2Int direction)
        {
            if (_direction == direction * -1) return;
            _direction = direction;
        }

        private void SyncDirection(Vector2Int oldValue, Vector2Int newValue)
        {
        }

        public class Factory : PlaceholderFactory<bool, Transform, SnakeMovement>
        {
        }
    }
}