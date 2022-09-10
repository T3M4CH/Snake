using UnityEngine;
using System;

namespace Game.TickController
{
    public class MonoTickService : MonoBehaviour
    {
        public event Action OnTick = () => { };

        private float _currentTime;
        
        private void Update()
        {
            _currentTime -= Time.deltaTime;
            if (_currentTime > 0) return;
            _currentTime = TickValue;
            OnTick.Invoke();
        }

        [field: SerializeField]
        public float TickValue
        {
            get;
            set;
        } = 1;
    }
}
