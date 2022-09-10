using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

namespace Menu
{
    public class MenuButtons : MonoBehaviour
    {
        [SerializeField] private Button singlePlayer;
        [SerializeField] private Button multiPlayer;
        [SerializeField] private Button settings;

        private void Start()
        {
            singlePlayer.onClick.AddListener(() => SceneManager.LoadScene(1));
            multiPlayer.onClick.AddListener(() => SceneManager.LoadScene(2));
        }
    }
}
