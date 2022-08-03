using System;

namespace Game.TimeService.Interfaces
{
   public interface ITimeService
   {
      public event Action OnTick;
   }
}
