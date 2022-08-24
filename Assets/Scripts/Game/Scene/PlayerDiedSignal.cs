using Game.Scene.Interfaces;
using UnityEngine;

namespace Game.Scene
{
    public struct PlayerDiedSignal : ISignalSoundPlayer
    {
        public string Sound => "Congratulations";
    }
}
