using Mirror;

namespace Multiplayer
{
    public interface IRefereeService
    {
        void Add(NetworkIdentity player);
        
        void Print(uint id);
    }
}
