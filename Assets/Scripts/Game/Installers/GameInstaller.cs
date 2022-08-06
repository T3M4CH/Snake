using Game.Apple;
using Game.Input;
using Game.Scene;
using Game.Snake;
using UnityEngine;
using Zenject;

namespace Game.Installers
{
    public class GameInstaller : MonoInstaller
    {
        #region MyRegion

        
        [SerializeField] private CircleCollider2D tailPrefab;
        [SerializeField] private MonoApple applePrefab;
        [SerializeField] private SwipeHandler swipeHandler;
        [SerializeField] private TimeService.TimeService timeService;

        #endregion
        
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
                .BindInterfacesTo<TimeService.TimeService>()
                .FromInstance(timeService)
                .AsSingle();

            Container
                .Bind<AppleSpawner>()
                .AsSingle()
                .WithArguments(applePrefab)
                .NonLazy();

            Container
                .BindMemoryPool<CircleCollider2D, MemoryPool<CircleCollider2D>>()
                .WithInitialSize(5)
                .FromComponentInNewPrefab(tailPrefab)
                .UnderTransformGroup("TailPrefabs")
                .AsSingle();

            Container
                .BindFactory<Transform, SnakeMovement, SnakeMovement.Factory>();

            Container
                .BindFactory<Transform, SnakeMovement, TailMovement, TailMovement.Factory>();

            Container
                .DeclareSignalWithInterfaces<PlayerDiedSignal>();
        }
    }
}