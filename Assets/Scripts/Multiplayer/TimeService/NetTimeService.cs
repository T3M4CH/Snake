using System;
using Game.TickController.Interfaces;
using Mirror;
using UnityEngine;

namespace Multiplayer.TimeService
{
    public class NetTimeService : NetworkBehaviour, ITimeService
    {
        public event Action OnTick = () => { };
        public event Action OnRealtimeTick = () => { };
        public event Action<bool> OnChangeState = _ => { };

        [SerializeField] private float tickValue = 1f;

        [SyncVar] private bool _isActive;
        private float _currentValue;

        [Server]
        public void ChangeState(bool isStart)
        {
            _isActive = isStart;
            OnChangeState.Invoke(isStart);
        }

        private void Update()
        {
            if (!isServer) return;
            _currentValue -= Time.deltaTime;
            if (_currentValue > 0) return;
            _currentValue = tickValue;
            Tick();
        }

        [Server]
        private void Tick()
        {
            OnRealtimeTick.Invoke();
            if (!_isActive) return;
            OnTick.Invoke();
            RpcTick();
        }

        [ClientRpc]
        private void RpcTick()
        {
            if (isClientOnly)
            {
                OnRealtimeTick.Invoke();
                OnTick.Invoke();
            }
        }

        public bool IsActive => _isActive;
    }
}