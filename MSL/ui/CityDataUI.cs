using System.Collections.Generic;
using ColossalFramework.UI;
using MSL.model;
using UnityEngine;

namespace MSL.ui
{
    public class CityDataUI : MonoBehaviour
    {
        private UIPanel _panel;
        private UIButton _toggleButton;
        private UILabel _titleLabel;
        private UIButton _closeButton;
        private UIListBox _listBox;
        private bool _isVisible;
        
        private Dictionary<string, CityData> _cityData = new Dictionary<string, CityData>();
        
        public void Start()
        {
            CreateUI();
        }

        private void CreateUI()
        {
                // Créer un bouton flottant
                _toggleButton = (UIButton)UIView.GetAView().AddUIComponent(typeof(UIButton));
                _toggleButton.text = "MSL";
                _toggleButton.width = 80;
                _toggleButton.height = 30;
                _toggleButton.normalBgSprite = "ButtonMenu";
                _toggleButton.hoveredBgSprite = "ButtonMenuHovered";
                _toggleButton.pressedBgSprite = "ButtonMenuPressed";
                _toggleButton.isVisible = true;
                _toggleButton.relativePosition = new Vector3(80, 10); // Position en haut à gauche

                _toggleButton.eventClick += (component, eventParam) =>
                {
                    _isVisible = !_isVisible;
                    _panel.isVisible = _isVisible;
                };

                // Créer le panneau principal
                _panel = (UIPanel)UIView.GetAView().AddUIComponent(typeof(UIPanel));
                _panel.backgroundSprite = "MenuPanel";
                _panel.width = 600;
                _panel.height = 300;
                _panel.relativePosition = new Vector3(100, 100); // Position sur l'écran
                _panel.isVisible = true;

                // Ajouter un titre
                _titleLabel = (UILabel)_panel.AddUIComponent(typeof(UILabel));
                _titleLabel.text = "Friends city's";
                _titleLabel.relativePosition = new Vector3(10, 10);

                // Ajouter une liste pour afficher les données des joueurs
                _listBox = (UIListBox)_panel.AddUIComponent(typeof(UIListBox));
                _listBox.width = 500;
                _listBox.height = 200;
                _listBox.relativePosition = new Vector3(10, 40);

                // Ajouter un bouton de fermeture
                _closeButton = (UIButton)_panel.AddUIComponent(typeof(UIButton));
                _closeButton.text = "X";
                _closeButton.width = 30;
                _closeButton.height = 30;
                _closeButton.normalBgSprite = "ButtonClose";
                _closeButton.relativePosition = new Vector3(_panel.width - 40, 10);
                _closeButton.eventClick += (component, eventParam) => { _panel.isVisible = false; };
        }
        
        public void UpdateCityDataDisplay(Dictionary<string, CityData> newCityData)
        {
            MslLogger.LogSuccess("Updating city data..");

            _cityData = newCityData;
            _listBox.items = new string[_cityData.Count];

            var index = 0;
            foreach (var entry in _cityData)
            {
                _listBox.items[index] = $"{entry.Key} : CONSO :{entry.Value.ElectricConsumption/1000} MW, PROD: {entry.Value.ElectricProduction/1000}MW, EXTRA: {entry.Value.ElectricExtra/1000}MW";
                index++;
            }
        }
    }
}