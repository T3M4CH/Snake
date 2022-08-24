using Game.Input;
using Game.Scene;
using Game.Snake;
using Mirror;
using Multiplayer;
using Multiplayer.Snake;
using Multiplayer.UI;
using UnityEngine;
using Zenject;

namespace Game.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private SetupSelector selector;
        [SerializeField] private SpawnSettings spawnSettings;
        [SerializeField] private GameObject tailPrefab;
        [SerializeField] private NetManager networkManager;
        [SerializeField] private CanvasHUD canvasHUD;
        [SerializeField] private LosePanel losePanel;
        [SerializeField] private Referee referee;
        [SerializeField] private SwipeHandler swipeHandler;
        [SerializeField] private TickController.TimeService timeService;

        public override void InstallBindings()
        {
            Container
                .Bind<CanvasHUD>()
                .FromInstance(canvasHUD)
                .AsSingle();
            
            Container
                .BindInterfacesTo<NetManager>()
                .FromInstance(networkManager)
                .AsSingle();
            
            Container
                .Bind<SetupSelector>()
                .FromInstance(selector)
                .AsSingle();
            
            Container
                .BindInterfacesTo<SpawnSettings>()
                .FromInstance(spawnSettings)
                .AsSingle();
            
            Container
                .BindInterfacesTo<SwipeHandler>()
                .FromInstance(swipeHandler)
                .AsSingle();

            Container
                .BindInterfacesTo<Referee>()
                .FromInstance(referee)
                .AsSingle();
            
            Container
                .BindInterfacesTo<LosePanel>()
                .FromInstance(losePanel)
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