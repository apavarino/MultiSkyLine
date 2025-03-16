using System;
using System.Linq;
using ColossalFramework.UI;
using MSL.model;
using MSL.model.repository;
using MSL.utils;
using UnityEngine;

namespace MSL.client.ui
{
    public class CityDataUI : MonoBehaviour
    {
        // Main panel
        private UIPanel _panel;
        private UIButton _toggleButton;
        private UIButton _toggleTabButton;
        public static bool ToggleContract = false;
        private UIButton _newContractButton;
        private UIButton _powerButton;
        private UILabel _titleLabel;
        private UIButton _closeButton;
        private UIListBox _listBox;
        private bool _isVisible;
        private CityDataGrid _cityDataGrid;
        
        // New contract panel
        private UIPanel _newContractPanel;
        private UITextField _amountTextField;
        private UITextField _priceTextField;
        private UIDropDown _dropdown;
        private UISlider _amountSlider;
        private UISlider _priceSlider;
        private UIButton _applyButton;
        private UIButton _newContractCloseButton;
        
        private CityDataRepository _cityDataRepository;
        
        public void Initialize(CityDataRepository cityDataRepository )
        {
            _cityDataRepository = cityDataRepository;
        }
        
        public void Start()
        {
            CreateUI();
        }

        private void CreateUI()
        {
            MainUI();
            NewContractUI();
        }

        private void NewContractUI()
        {
            // New contract panel
            _newContractPanel = (UIPanel)UIView.GetAView().AddUIComponent(typeof(UIPanel));
            _newContractPanel.backgroundSprite = "MenuPanel";
            _newContractPanel.width = 425;
            _newContractPanel.height = 200;
            _newContractPanel.relativePosition = new Vector3(220, 520); 
            _newContractPanel.isVisible = false;
            
            // Label
            UILabel titleLabel = _newContractPanel.AddUIComponent<UILabel>();
            titleLabel.text = "New contract";
            titleLabel.relativePosition = new Vector3(10, 10);
            
            // DropDown
            _dropdown = UIBuilder.CreateDropdown(_newContractPanel,"Resource", 55, 
                Enum.GetValues(typeof(ContractType))
                    .Cast<ContractType>()
                    .Select(e => e.ToString())
                    .ToList()
            );
            
            // Slider
            _amountSlider = UIBuilder.CreateSlider(_newContractPanel,"Amount", 95, 0, 1000, 1,_amountTextField);
            _priceSlider = UIBuilder.CreateSlider(_newContractPanel,"Price", 140, 0, 5000, 5, _priceTextField);
            
            //Apply
            _applyButton = UIBuilder.CreateButton(_newContractPanel,"New Contract", 120,150,160);
            _applyButton.eventClick += (component, eventParam) =>
            {
                var amount = _amountSlider.value;
                var price = _priceSlider.value;
                MslLogger.LogSuccess($"Amount: {amount}, Price: {price}");
                var contract = new Contract
                {
                    From = _cityDataRepository.FindCurrentCityName(),
                    Amount = _amountSlider.value,
                    Price = _priceSlider.value,
                    Type = (ContractType)Enum.Parse(typeof(ContractType),_dropdown.selectedValue)
                };
                _cityDataRepository.AddContract(contract);
                UpdateCityDataDisplay();
                _newContractPanel.isVisible = false;
            };

            // Closed
            _newContractCloseButton = (UIButton)_newContractPanel.AddUIComponent(typeof(UIButton));
            _newContractCloseButton.text = "X";
            _newContractCloseButton.width = 30;
            _newContractCloseButton.height = 30;
            _newContractCloseButton.normalBgSprite = "ButtonClose";
            _newContractCloseButton.relativePosition = new Vector3(_newContractPanel.width - 30, 5);
            _newContractCloseButton.eventClick += (component, eventParam) => _newContractPanel.isVisible = false;
        }

        private void MainUI()
        {
            // MSL Button
            _toggleButton = (UIButton)UIView.GetAView().AddUIComponent(typeof(UIButton));
            _toggleButton.text = "MSL";
            _toggleButton.width = 80;
            _toggleButton.height = 30;
            _toggleButton.normalBgSprite = "ButtonMenu";
            _toggleButton.hoveredBgSprite = "ButtonMenuHovered";
            _toggleButton.pressedBgSprite = "ButtonMenuPressed";
            _toggleButton.isVisible = true;
            _toggleButton.relativePosition = new Vector3(80, 10); 
            _toggleButton.eventClick += (component, eventParam) =>
            {
                _isVisible = !_isVisible;
                _panel.isVisible = _isVisible;
            };

            // Main Panel
            _panel = (UIPanel)UIView.GetAView().AddUIComponent(typeof(UIPanel));
            _panel.backgroundSprite = "MenuPanel";
            _panel.width = 650;
            _panel.height = 400;
            _panel.relativePosition = new Vector3(100, 100);
            _panel.isVisible = true;

            // Title
            _titleLabel = (UILabel)_panel.AddUIComponent(typeof(UILabel));
            _titleLabel.text = "Friends city's";
            _titleLabel.relativePosition = new Vector3(10, 10);
            
            _cityDataGrid = (CityDataGrid)_panel.AddUIComponent(typeof(CityDataGrid));
            
            // Close button
            _closeButton = (UIButton)_panel.AddUIComponent(typeof(UIButton));
            _closeButton.text = "X";
            _closeButton.width = 30;
            _closeButton.height = 30;
            _closeButton.normalBgSprite = "ButtonClose";
            _closeButton.relativePosition = new Vector3(_panel.width - 30, 5);
            _closeButton.eventClick += (component, eventParam) => { _panel.isVisible = false; };

            // Power button
            _powerButton = UIBuilder.CreateButton(_panel, "Power", 100,10, 50);
            
            // Toggle Button
            _toggleTabButton = UIBuilder.CreateButton(_panel, "Toggle open contracts", 190,_panel.width - 360, _panel.height - 45);
            _toggleTabButton.eventClick += (component, eventParam) =>
            {
                ToggleContract = !ToggleContract;
                _toggleTabButton.text = ToggleContract ? "Toggle cities data" : "Show open contracts";
                UpdateCityDataDisplay();
            };
            // New Contract Button
            _newContractButton = UIBuilder.CreateButton(_panel, "New Contract", 150,_panel.width - 160, _panel.height - 45);
            _newContractButton.eventClick += (component, eventParam) =>
            {
                _newContractPanel.isVisible = true;
            };
        }
        
        public void UpdateCityDataDisplay()
        {
            _cityDataGrid.UpdateGrid(_cityDataRepository.FindAll());
        }
    }
}