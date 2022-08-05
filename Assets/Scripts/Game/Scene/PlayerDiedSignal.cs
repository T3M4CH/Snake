using Game.Scene.Interfaces;
using Game.TimeService.Interfaces;
using UnityEngine;

namespace Game.Scene
{
    public struct PlayerDiedSignal : ISignalSoundPlayer
    {
        public string Sound => "Congratulations";
    }
}
