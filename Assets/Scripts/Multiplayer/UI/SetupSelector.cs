using System;
using Multiplayer.Snake;
using UnityEngine;
using UnityEngine.UI;

namespace Multiplayer.UI
{
    [Serializable]
    public class SetupSelector
    {
        [SerializeField] private Image panel;
        [SerializeField] private Color[] colors;
        [SerializeField] private Image colorImage;
        [SerializeField] private Button rightButton;
        [SerializeField] private Button leftButton;

        private int _index;
        private int _lenght;
        
        public void SetActive(bool value)
        {
            panel.gameObject.SetActive(value);
            if (value)
            {
                rightButton.onClick.AddListener(() => Index += 1);
                leftButton.onClick.AddListener(() => Index -= 1);

                _lenght = colors.Length;
            }
            else
            {
                rightButton.onClick.RemoveAllListeners();
                leftButton.onClick.RemoveAllListeners();
                ReadyButton.onClick.RemoveAllListeners();
            }
        }

        private void ChangeColor(int index)
        {
            colorImage.color = colors[index];
        }

        public void Dispose()
        {
            rightButton.onClick.RemoveAllListeners();
            leftButton.onClick.RemoveAllListeners();
        }

        private int Index
        {
            get => _index;
            set
            {
                _index = value;
                if (_index < 0) _index = _lenght - 1;
                if (_index > _lenght - 1) _index = 0;
                ChangeColor(_index);
            }
        }

        public Color SelectedColor => colors[_index];

        [field: SerializeField] public Button ReadyButton { get; private set; }
    }
}