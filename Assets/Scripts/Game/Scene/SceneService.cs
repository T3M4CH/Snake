using UnityEngine.SceneManagement;
using Game.Scene.Interfaces;

namespace Game.Scene
{
    public class SceneService : ISceneService
    {
        public void LoadScene(int id)
        {
            SceneManager.LoadScene(id);
        }
    }
}
