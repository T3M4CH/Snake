using Game.Scene.Interfaces;
using UnityEngine;
using Zenject;

namespace Singleplayer.Scene
{
    public class SoundSystem
    {
        private SignalBus _signalBus;
        
        public SoundSystem(SignalBus signalBus)
        {
            signalBus.Subscribe<ISignalSoundPlayer>(x => Debug.Log(x.Sound));
        }
    }
}
