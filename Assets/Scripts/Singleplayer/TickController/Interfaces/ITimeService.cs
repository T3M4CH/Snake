using System;

namespace Game.TickController.Interfaces
{
   public interface ITimeService
   {
      event Action OnTick;
      event Action OnRealtimeTick;
      event Action<bool> OnChangeState;

      void ChangeState(bool isStart);

      public bool IsActive
      {
         get;
      }
   }
}
