using Game.Input;
using Game.Scene;
using Game.Snake;
using Mirror;
using UnityEngine;
using Zenject;

namespace Game.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GameObject tailPrefab;
        [SerializeField] private SwipeHandler swipeHandler;
        [SerializeField] private TickController.TimeService timeService;

        public override void InstallBindings()
        {
            Container
                .BindInterfacesTo<SwipeHandler>()
                .FromInstance(swipeHandler)
                .AsSingle();

            Container
                .BindInstance(tailPrefab)
                .AsSingle();

            Container
                .Bind<SoundSystem>()
                .AsSingle()
                .NonLazy();

            Container
                .BindInterfacesTo<TickController.TimeService>()
                .FromInstance(timeService)
                .AsSingle();

            Container
                .DeclareSignalWithInterfaces<PlayerDiedSignal>();
        }
    }
}