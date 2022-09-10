using Multiplayer.Apple.Interfaces;
using System;
using Mirror;

namespace Multiplayer.Apple
{
    public class NetApple : NetworkBehaviour, IConsumable
    {
        private event Action<NetApple> OnPick = _ => { };
        
        public void Initialize(Action<NetApple> action)
        {
            OnPick += action;
        }
        
        public void Pick()
        {
            OnPick.Invoke(this);
        }
    }
}
