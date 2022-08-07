using System.Collections.Generic;
using Game.TimeService.Interfaces;
using Mirror;
using UnityEngine;
using Zenject;

namespace Game.Snake
{
    public class TailMovement
    {
        public TailMovement(Transform player, bool isLocalPlayer, SnakeMovement snakeMovement, MemoryPool<CircleCollider2D> tailSegment,
            ITimeService timeService)
        {
            _isLocalPlayer = isLocalPlayer;
            _snakeMovement = snakeMovement;
            _segmentPool = tailSegment;
            _tail.Add(player.GetChild(0));
            _timeService = timeService;
            _timeService.OnTick += CmdMove;
        }

        private ITimeService _timeService;
        private readonly bool _isLocalPlayer;
        private readonly MemoryPool<CircleCollider2D> _segmentPool;
        private readonly SnakeMovement _snakeMovement;
        private List<Transform> _tail = new();

        public void Add()
        {
            var segment = _segmentPool.Spawn();
            segment.transform.position = _tail[^1].position;
            _tail.Add(segment.transform);
        }
        
        [Command]
        private void CmdMove()
        {
            
            for (var i = _tail.Count - 1; i > 0; i--)
            {
                _tail[i].position = _tail[i - 1].position;
                if (i == _tail.Count - 1)
                {
                    _tail[i].gameObject.SetActive(true);
                }
            }
            _snakeMovement.CmdMove();
        }

        public void Dispose()
        {
            _timeService.OnTick -= CmdMove;
        }
    
        public class Factory : PlaceholderFactory<Transform, bool, SnakeMovement, TailMovement>
        {
        }
    }
}