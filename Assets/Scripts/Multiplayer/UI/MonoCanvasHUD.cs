using UnityEngine.SceneManagement;
using Multiplayer.UI.Enums;
using UnityEngine.UI;
using UnityEngine;
using System;
using Mirror;
using TMPro;

namespace Multiplayer
{
    public class MonoCanvasHUD : MonoBehaviour
    {
        [SerializeField] private Button buttonCreate;
        [SerializeField] private Button buttonHost;
        [SerializeField] private Button buttonServer;
        [SerializeField] private Button buttonJoin;
        [SerializeField] private Button buttonClient;
        [SerializeField] private Button backButton;
        [SerializeField] private Image mainPanel;
        [SerializeField] private Image createPanel;
        [SerializeField] private Image joinPanel;
        [SerializeField] private Image connectionPanel;
        [SerializeField] private TextMeshProUGUI connectionText;
        [SerializeField] private TMP_InputField inputField;

        public void Start()
        {
            backButton.onClick.AddListener(ButtonBack);
            buttonClient.onClick.AddListener(ButtonClient);
            buttonCreate.onClick.AddListener(() => OpenPage(EMenuPage.Create));
            buttonJoin.onClick.AddListener(() => OpenPage(EMenuPage.Join));
            buttonHost.onClick.AddListener(NetworkManager.singleton.StartHost);
            buttonServer.onClick.AddListener(NetworkManager.singleton.StartServer);
            inputField.onValueChanged.AddListener(str => NetworkManager.singleton.networkAddress = str);
        }

        private void ButtonBack()
        {
            if (connectionPanel.IsActive())
            {
                NetworkManager.singleton.StopClient();
                OpenPage(EMenuPage.Join);
            }

            if (mainPanel.IsActive())
            {
                SceneManager.LoadScene(0);
            }

            OpenPage(EMenuPage.Main);
        }

        private void OpenPage(EMenuPage page)
        {
            ClosePages();
            switch (page)
            {
                case EMenuPage.Main:
                    mainPanel.gameObject.SetActive(true);
                    break;
                case EMenuPage.Create:
                    createPanel.gameObject.SetActive(true);
                    break;
                case EMenuPage.Join:
                    joinPanel.gameObject.SetActive(true);
                    break;
                case EMenuPage.Connect:
                    connectionPanel.gameObject.SetActive(true);
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        private void ClosePages()
        {
            mainPanel.gameObject.SetActive(false);
            createPanel.gameObject.SetActive(false);
            joinPanel.gameObject.SetActive(false);
            connectionPanel.gameObject.SetActive(false);
        }

        private void ButtonClient()
        {
            NetworkManager.singleton.StartClient();
            OpenPage(EMenuPage.Connect);
            connectionText.text = "Connection to " + NetworkManager.singleton.networkAddress;
        }

        [field: SerializeField]
        public TextMeshProUGUI AddressText
        {
            get;
            private set;
        }
    }
}