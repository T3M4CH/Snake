using Game.TimeService.Interfaces;
using Game.Input.Interfaces;
using UnityEngine;
using Zenject;

namespace Game.Snake
{
    public class SnakeMovement
    {
        public SnakeMovement(Transform transform, ISwipeHandler swipeHandler)
        {
            _transform = transform;
            swipeHandler.OnSwipeUp += () => ChangeDirection(Vector2Int.up);
            swipeHandler.OnSwipeLeft += () => ChangeDirection(Vector2Int.left);
            swipeHandler.OnSwipeRight += () => ChangeDirection(Vector2Int.right);
            swipeHandler.OnSwipeDown += () => ChangeDirection(Vector2Int.down);
        }

        private Vector2Int _direction = Vector2Int.up;
        private readonly Transform _transform;
        private readonly ITimeService _timeService;

        public void Move()
        {
            var position = _transform.position;
            _transform.position = new Vector3Int((int)position.x + _direction.x, (int)position.y + _direction.y);
        }

        private void ChangeDirection(Vector2Int direction)
        {
            if (_direction == direction * -1) return;
            _direction = direction;
        }
        
        public class Factory : PlaceholderFactory<Transform, SnakeMovement>
        {
        }
    }
}