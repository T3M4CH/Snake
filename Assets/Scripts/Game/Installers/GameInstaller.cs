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
        [SerializeField] private SwipeHandler swipeHandler;
        [SerializeField] private TickController.TimeService timeService;

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
                .BindInterfacesTo<TickController.TimeService>()
                .FromInstance(timeService)
                .AsSingle();

            Container
                .BindMemoryPool<CircleCollider2D, MemoryPool<CircleCollider2D>>()
                .WithInitialSize(5)
                .FromComponentInNewPrefab(tailPrefab)
                .UnderTransformGroup("TailPrefabs")
                .AsSingle();

            Container
                .BindFactory<bool ,Transform, SnakeMovement, SnakeMovement.Factory>();

            Container
                .BindFactory<Transform, bool,SnakeMovement, TailMovement, TailMovement.Factory>();

            Container
                .DeclareSignalWithInterfaces<PlayerDiedSignal>();
        }
    }
}