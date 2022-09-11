using UnityEngine;
using Mirror;
using Multiplayer.Apple;

namespace Game.Apple
{
    public class NetAppleSpawner : NetworkBehaviour
    {
        [SerializeField] private NetApple applePrefab;
        
        public override void OnStartServer()
        {
            var instance = Instantiate(applePrefab);
            NetworkServer.Spawn(instance.gameObject);
            instance.transform.position = RandomPosition;
            instance.Initialize(ChangePosition);
        }

        [Server]
        private void ChangePosition(NetApple apple)
        {
            var position = RandomPosition;
            apple.transform.position = position;
            RpcChangePosition(apple, position);
        }

        [ClientRpc]
        private void RpcChangePosition(NetApple apple, Vector3 position)
        {
            apple.transform.position = position;
        }
        
        private static Vector3 RandomPosition => new(Random.Range(-4, 4), Random.Range(-4, 4), 0);
            //new(Random.Range(-8, 8), Random.Range(-4, 4), 0);
    }
}