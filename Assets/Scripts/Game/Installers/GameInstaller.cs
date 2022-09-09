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
        [SerializeField] private SerializableSpawnSettings serializableSpawnSettings;
        [SerializeField] private GameObject tailPrefab;
        [SerializeField] private NetManager networkManager;
        [SerializeField] private MonoCanvasHUD monoCanvasHUD;
        [SerializeField] private NetLosePanel netLosePanel;
        [SerializeField] private NetReferee netReferee;
        [SerializeField] private MonoSwipeHandler monoSwipeHandler;
        [SerializeField] private TickController.TimeService timeService;

        public override void InstallBindings()
        {
            Container
                .Bind<MonoCanvasHUD>()
                .FromInstance(monoCanvasHUD)
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
                .BindInterfacesTo<SerializableSpawnSettings>()
                .FromInstance(serializableSpawnSettings)
                .AsSingle();
            
            Container
                .BindInterfacesTo<MonoSwipeHandler>()
                .FromInstance(monoSwipeHandler)
                .AsSingle();

            Container
                .BindInterfacesTo<NetReferee>()
                .FromInstance(netReferee)
                .AsSingle();
            
            Container
                .BindInterfacesTo<NetLosePanel>()
                .FromInstance(netLosePanel)
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