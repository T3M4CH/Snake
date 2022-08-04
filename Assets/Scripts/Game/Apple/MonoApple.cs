using System;
using Game.Apple.Interfaces;
using UnityEngine;

namespace Game.Apple
{
    public class MonoApple : MonoBehaviour, IPickable
    {
        private event Action<MonoApple> OnPick = _ => { };
        
        public void Initialize(Action<MonoApple> action)
        {
            OnPick += action;
        }
        
        public void Pick()
        {
            OnPick.Invoke(this);
        }
    }
}
