using System;
using System.Collections.Generic;
using Game.Input.Interfaces;
using Game.TimeService.Interfaces;
using Mirror;
using UnityEngine;
using Zenject;

namespace Game.Snake
{
    public class TailMovement : NetworkBehaviour
    {
        [Inject]
        public void Construct
        (
            GameObject tailSegment,
            ITimeService timeService,
            ISwipeHandler swipeHandler
        )
        {
            _swipeHandler = swipeHandler;
            _segmentPrefab = tailSegment;
            _timeService = timeService;
        }

        private ITimeService _timeService;
        private Action _onChangeActive;
        [SyncVar] private Vector2Int _direction;
        private ISwipeHandler _swipeHandler;
        private GameObject _segmentPrefab;
        private NetworkConnection _client;
        private List<Transform> _tail = new();

        [Server]
        public void CmdAdd(GameObject sender)
        {
            if (sender != gameObject) return;
            var segment = Instantiate(_segmentPrefab);
            NetworkServer.Spawn(segment, connectionToClient);
            segment.SetActive(false);
            
            RpcAdd(segment);
        }
        
        [Command]
        private void CmdSetActive(GameObject segment)
        {
            segment.SetActive(true);
            RpcSetActive(segment);
        }

        [ClientRpc]
        private void RpcSetActive(GameObject segment)
        {
            segment.SetActive(true);
            _timeService.OnTick -= _onChangeActive;
        }

        [ClientRpc]
        private void RpcAdd(GameObject segment)
        {
            segment.transform.position = _tail[^1].position;
            _tail.Add(segment.transform);
            segment.SetActive(false);
            
            _onChangeActive = () => CmdSetActive(segment);
            _timeService.OnTick += _onChangeActive;
        }

        private void Move()
        {
            for (var i = _tail.Count - 1; i > 0; i--)
            {
                _tail[i].position = _tail[i - 1].position;
            }

            var position = transform.position;
            ValidatePosition(ref position);
            position = new Vector3Int((int)position.x + _direction.x, (int)position.y + _direction.y);
            transform.position = position;
        }

        [Command]
        private void CmdChangeDirection(Vector2Int direction)
        {
            if (_direction == direction * -1) return;
            _direction = direction;
        }

        private void Start()
        {
            if (isLocalPlayer)
            {
                _swipeHandler.OnSwipe += CmdChangeDirection;
                _timeService.OnTick += Move;
            }
            
            _tail.Add(transform);
        }

        private void ValidatePosition(ref Vector3 position)
        {
            if (position.x is > 5 or < -5) position.x *= -1;
            if (position.y is > 5 or < -5) position.y *= -1;
        }
    }
}