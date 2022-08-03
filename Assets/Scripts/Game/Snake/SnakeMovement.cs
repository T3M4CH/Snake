using Game.TimeService.Interfaces;
using Game.Input.Interfaces;
using UnityEngine;
using Zenject;
using System;

namespace Game.Snake
{
    public class SnakeMovement : IDisposable
    {
        public SnakeMovement(Transform transform, ISwipeHandler swipeHandler, ITimeService timeService)
        {
            _transform = transform;
            _swipeHandler = swipeHandler;
            _timeService = timeService;
            _timeService.OnTick += Move;
            _swipeHandler.OnSwipeUp += () => _direction = Vector2Int.up;
            _swipeHandler.OnSwipeLeft += () => _direction = Vector2Int.left;
            _swipeHandler.OnSwipeRight += () => _direction = Vector2Int.right;
            _swipeHandler.OnSwipeDown += () => _direction = Vector2Int.down;
        }

        private Vector2Int _direction = Vector2Int.up;
        private ISwipeHandler _swipeHandler;
        private Transform _transform;
        private readonly ITimeService _timeService;

        private void Move()
        {
            var position = _transform.position;
            _transform.position = new Vector3Int((int)position.x + _direction.x, (int)position.y + _direction.y);
        }

        public void Dispose()
        {
            _timeService.OnTick -= Move;
        }

        public class Factory : PlaceholderFactory<Transform, SnakeMovement>
        {
        }
    }
}