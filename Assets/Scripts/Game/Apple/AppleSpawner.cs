using UnityEngine;
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
            instance.Initialize(ChangePosition);
        }

        [Server]
        private void ChangePosition(MonoApple apple)
        {
            var position = RandomPosition;
            apple.transform.position = position;
            RpcChangePosition(apple, position);
        }

        [ClientRpc]
        private void RpcChangePosition(MonoApple apple, Vector3 position)
        {
            apple.transform.position = position;
        }
        
        private static Vector3 RandomPosition => new(Random.Range(-8, 8), Random.Range(-4, 4), 0);
    }
}