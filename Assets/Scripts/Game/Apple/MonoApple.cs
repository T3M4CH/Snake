using System;
using Game.Apple.Interfaces;
using Mirror;

namespace Game.Apple
{
    public class MonoApple : NetworkBehaviour, IPickable
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
