using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ColossalFramework.UI;
using MSL.model;
using MSL.model.repository;
using UnityEngine;

namespace MSL.client.ui
{
    public class CityDataGrid  : UIPanel
    {
        private UIPanel _tableContainer;
        private readonly List<UIPanel> _rows = new List<UIPanel>();

        private const float ColumnWidth = 110f;
        private const float RowHeight = 30f;

        public override void Start()
        {
            base.Start();
            CreateUI();
        }

        private void CreateUI()
        {
            width = 530;
            height = 300;
            relativePosition = new Vector3(120, 40);
            backgroundSprite = "GenericPanel";
            
            var titleLabel = AddUIComponent<UILabel>();
            titleLabel.text = "City Data";
            titleLabel.relativePosition = new Vector3(10, 7);
            
            _tableContainer = AddUIComponent<UIPanel>();
            _tableContainer.width = 530;
            _tableContainer.height = 270;
            _tableContainer.relativePosition = new Vector3(0, 30);
            _tableContainer.backgroundSprite = "GenericPanelLight";
            
            AddRow(new List<string> { "City", "Cons.(MW)", "Prod.(MW)", "Extra (MW)" }, isHeader: true);
        }

        public void UpdateGrid(
            Dictionary<string, CityData> cityData, 
            string currentCity,
            CityDataRepository cityDataRepository)
        {
            foreach (var row in _rows)
            {
                Destroy(row.gameObject);
            }
            _rows.Clear();

            if (!CityDataUI.ToggleContract)
            {
                AddRow(new List<string> { "City", "Cons.(MW)", "Prod.(MW)", "Extra (MW)" }, isHeader: true);
                foreach (var entry in cityData)
                {
                    AddRow(new List<string>
                    {
                        entry.Key, $"{entry.Value.ElectricConsumption / 1000}",
                        $"{entry.Value.ElectricProduction / 1000}",
                        $"{entry.Value.ElectricExtra / 1000}"
                    });
                }
            }
            else
            {
                AddRow(new List<string> { "City", "Type", "Amount", "Price", "Action" }, isHeader: true);
                foreach (var contract in cityData.SelectMany(entry => entry.Value.Contracts))
                {
                    AddRow(new List<string>
                    {
                        contract.From, contract.Type.ToString(), 
                        contract.Amount.ToString(CultureInfo.InvariantCulture),
                        contract.Price.ToString(CultureInfo.InvariantCulture)
                    });

                    if (contract.From == currentCity)
                    {
                       var removeContractButton =  UIBuilder.CreateButton(_tableContainer, "x", 20, 4 * ColumnWidth + 15 , 35);
                       removeContractButton.height = 20;
                       removeContractButton.eventClick += (component, eventParam) =>
                       {
                           cityDataRepository.RemoveContract(contract);
                           Destroy(removeContractButton.gameObject);
                           UpdateGrid(cityDataRepository.FindAll(), currentCity, cityDataRepository);  
                       };
                    }
                }
            }
        }
        
        private void AddRow(List<string> lines, bool isHeader = false)
        {
            var rowPanel = _tableContainer.AddUIComponent<UIPanel>();
            rowPanel.width = _tableContainer.width;
            rowPanel.height = RowHeight;
            rowPanel.relativePosition = new Vector3(0, _rows.Count * RowHeight);
            rowPanel.backgroundSprite = isHeader ? "GenericPanel" : "GenericPanelLight";
            
            foreach (var line in lines.Select((value, index) => new { index, value }))
            {
                CreateCell(rowPanel, line.value, line.index, isHeader);
            }
            
            _rows.Add(rowPanel);
        }

        private static void CreateCell(UIPanel row, string text, int columnIndex, bool isHeader)
        {
            var cell = row.AddUIComponent<UILabel>();
            cell.text = text;
            cell.width = ColumnWidth;
            cell.height = RowHeight;
            cell.autoSize = false; 
            cell.relativePosition = new Vector3(columnIndex * ColumnWidth + 10, 8);
            cell.color = isHeader ? new Color32(255, 255, 255, 255) : new Color32(0, 0, 0, 200);
        }
    }
}