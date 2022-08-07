using UnityEngine;
using Zenject;
using Mirror;

namespace Game.Apple
{
    public class AppleSpawner : NetworkBehaviour
    {
        [SerializeField] private MonoApple applePrefab;
        
        public override void OnStartServer()
        {
            var instance = Instantiate(applePrefab);
            NetworkServer.Spawn(instance.gameObject);
            instance.transform.position = RandomPosition;
            instance.Initialize(CmdChangePosition);
        }

        [Command(requiresAuthority = false)]
        private void CmdChangePosition(MonoApple apple)
        {
            RpcChangePosition(apple, RandomPosition);
        }

        [ClientRpc]
        private void RpcChangePosition(MonoApple apple, Vector3 position)
        {
            apple.transform.position = position;
        }
        
        private static Vector3 RandomPosition => new(Random.Range(-5, 5), Random.Range(-5, 5), 0);
    }
}