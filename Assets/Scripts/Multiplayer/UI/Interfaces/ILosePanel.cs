using Multiplayer.UI.Enums;
using System;

namespace Multiplayer.UI.Interfaces
{
    public interface ILosePanel
    {
        event Action Restart;
        
        void Open(EPlayerStates stateValue);
    }
}