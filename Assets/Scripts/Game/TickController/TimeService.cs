using Game.TimeService.Interfaces;
using UnityEngine;
using System;

namespace Game.TimeService
{
    public class TimeService : MonoBehaviour ,ITimeService
    {
        public event Action OnTick = () => {};

        private float _tickValue = 1f;

        private void Update()
        {
            _tickValue -= Time.deltaTime;
            if (_tickValue > 0) return;
            _tickValue = 1;
            OnTick.Invoke();
        }
    }
}
