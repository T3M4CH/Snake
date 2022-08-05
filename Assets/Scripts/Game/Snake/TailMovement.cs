using System.Collections.Generic;
using Game.TimeService.Interfaces;
using UnityEngine;
using Zenject;

namespace Game.Snake
{
    public class TailMovement
    {
        public TailMovement(Transform player, SnakeMovement snakeMovement, MemoryPool<CircleCollider2D> tailSegment,
            ITimeService timeService)
        {
            _snakeMovement = snakeMovement;
            _segmentPool = tailSegment;
            _tail.Add(player.GetChild(0));
            _timeService = timeService;
            _timeService.OnTick += Move;
        }

        private ITimeService _timeService;
        private readonly MemoryPool<CircleCollider2D> _segmentPool;
        private readonly SnakeMovement _snakeMovement;
        private List<Transform> _tail = new();

        public void Add()
        {
            var segment = _segmentPool.Spawn();
            segment.transform.position = _tail[^1].position;
            _tail.Add(segment.transform);
        }

        private void Move()
        {
            for (var i = _tail.Count - 1; i > 0; i--)
            {
                _tail[i].position = _tail[i - 1].position;
                if (i == _tail.Count - 1)
                {
                    _tail[i].gameObject.SetActive(true);
                }
            }

            _snakeMovement.Move();
        }

        public void Dispose()
        {
            _timeService.OnTick -= Move;
        }
    
        public class Factory : PlaceholderFactory<Transform, SnakeMovement, TailMovement>
        {
        }
    }
}