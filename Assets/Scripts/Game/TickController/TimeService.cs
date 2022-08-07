using System;
using Game.TimeService.Interfaces;
using Mirror;
using UnityEngine;

namespace Game.TickController
{
    public class TimeService : NetworkBehaviour, ITimeService
    {
        public event Action OnTick = () => { };

        [SerializeField] private float tickValue = 1f;
        
        private float _currentValue;

        private void Update()
        {
            if (!isServer) return;
            _currentValue -= Time.deltaTime;
            if (_currentValue > 0) return;
            _currentValue = tickValue;
            RpcTick();
        }

        [ClientRpc]
        private void RpcTick()
        {
            OnTick.Invoke();
        }
    }
}