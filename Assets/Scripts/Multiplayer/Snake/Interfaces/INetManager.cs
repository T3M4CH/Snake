using System;

namespace Multiplayer.Snake.Interfaces
{
    public interface INetManager
    {
        event Action OnClientInteract;
        
        int PlayerCount
        {
            get;
        }
    }
}
