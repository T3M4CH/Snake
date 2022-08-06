using Game.Scene;
using Game.Scene.Interfaces;
using UnityEngine;
using Zenject;

namespace Game.Apple
{
    public class AppleSpawner
    {
        public AppleSpawner(MonoApple apple, DiContainer container, SignalBus signalBus)
        {
            signalBus.Subscribe<ISignalSoundPlayer>(x => Debug.Log("ya prav!"));
            Spawn(apple, container);
        }

        private void Spawn(MonoApple applePrefab, DiContainer container)
        {
            var instance = container.InstantiatePrefab(applePrefab).GetComponent<MonoApple>();
            instance.transform.position = RandomPosition;
            instance.Initialize(ChangePosition);
        }

        private static void ChangePosition(MonoApple apple)
        {
            apple.transform.position = RandomPosition;
        }

        private static Vector2 RandomPosition => new(Random.Range(-8, 9), Random.Range(-4, 5));
    }
}