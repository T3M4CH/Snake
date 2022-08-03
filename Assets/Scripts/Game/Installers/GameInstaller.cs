using Game.Input;
using Game.Snake;
using UnityEngine;
using Zenject;

namespace Game.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private SwipeHandler swipeHandler;
        [SerializeField] private TimeService.TimeService timeService;
        
        public override void InstallBindings()
        {
            Container
                .BindInterfacesTo<SwipeHandler>()
                .FromInstance(swipeHandler)
                .AsSingle();

            Container
                .BindInterfacesTo<TimeService.TimeService>()
                .FromInstance(timeService)
                .AsSingle();
            
            Container
                .BindFactory<Transform, SnakeMovement, SnakeMovement.Factory>();
        }
    }
}