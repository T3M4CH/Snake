using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu
{
    public class MenuButtons : MonoBehaviour
    {
        [SerializeField] private Button singlePlayer;
        [SerializeField] private Button multiPlayer;
        [SerializeField] private Button settings;

        private void Start()
        {
            multiPlayer.onClick.AddListener(() => SceneManager.LoadScene(2));
        }
    }
}
