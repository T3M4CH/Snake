using System;
using System.Collections.Generic;
using Game.Input.Interfaces;
using Game.TickController.Interfaces;
using Mirror;
using UnityEngine;
using Zenject;

namespace Multiplayer.Snake
{
    public class NetTailMovement : NetworkBehaviour
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
        private readonly List<Transform> _tail = new();

        public void RemoveTail()
        {
            CmdDestroyTail(_tail);
            _tail.RemoveRange(1, _tail.Count - 1);
        }

        [Server]
        public void Add(GameObject sender)
        {
            if (sender != gameObject) return;

            var segment = Instantiate(_segmentPrefab, Vector2.one * 10, Quaternion.identity);
            NetworkServer.Spawn(segment, connectionToClient);
            segment.transform.position = _tail[^1].position;
            _tail.Add(segment.transform);
            segment.SetActive(false);
            
            _onChangeActive = () => SetActive(segment, true);
            _timeService.OnTick += _onChangeActive;
        }
        
        [ClientRpc]
        public void RpcStopMovement()
        {
            _direction = Vector2Int.zero;
        }

        private void SetActive(GameObject segment, bool value)
        {
            segment.SetActive(value);
            RpcSetActive(segment, value);
        }

        [ClientRpc]
        private void RpcSetActive(GameObject segment, bool value)
        {
            segment.SetActive(value);
            if (value)
            {
                _timeService.OnTick -= _onChangeActive;
            }
        }

        [Server]
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

        [Command]
        private void CmdDestroyTail(List<Transform> tailTransforms)
        {
            for (int i = 1; i < tailTransforms.Count; i++)
            {
                NetworkServer.Destroy(tailTransforms[i].gameObject);
            }
        }

        private void ChangeMoveAccess(bool value)
        {
            if (value)
            {
                _timeService.OnTick += Move;
            }
            else
            {
                _timeService.OnTick -= Move;
            }
        }

        private void Start()
        {
            if (isLocalPlayer)
            {
                _swipeHandler.OnSwipe += CmdChangeDirection;
            }

            if (isServer)
            {
                ChangeMoveAccess(_timeService.IsActive);
                _timeService.OnChangeState += ChangeMoveAccess;
            }

            _tail.Add(transform);
        }

        private void ValidatePosition(ref Vector3 position)
        {
            if (position.x is > 8 or < -8) position.x *= -1;
            if (position.y is > 4 or < -4) position.y *= -1;
        }
    }
}