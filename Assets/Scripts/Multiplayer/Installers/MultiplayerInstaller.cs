using Game.Input;
using Game.Scene;
using Multiplayer.Snake;
using Multiplayer.TimeService;
using Multiplayer.UI;
using Singleplayer.Scene;
using UnityEngine;
using Zenject;

namespace Multiplayer.Installers
{
    public class MultiplayerInstaller : MonoInstaller
    {
        [SerializeField] private SetupSelector selector;
        [SerializeField] private SerializableSpawnSettings serializableSpawnSettings;
        [SerializeField] private GameObject tailPrefab;
        [SerializeField] private NetManager networkManager;
        [SerializeField] private MonoCanvasHUD monoCanvasHUD;
        [SerializeField] private NetLosePanel netLosePanel;
        [SerializeField] private NetReferee netReferee;
        [SerializeField] private MonoSwipeHandler monoSwipeHandler;
        [SerializeField] private NetTimeService netTimeService;

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
                .BindInterfacesTo<NetTimeService>()
                .FromInstance(netTimeService)
                .AsSingle();
        }
    }
}