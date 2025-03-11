using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ColossalFramework.UI;
using MSL.model;
using UnityEngine;

namespace MSL.client.ui
{
    public class CityDataGrid  : UIPanel
    {
        private UIPanel _tableContainer;
        private List<UIPanel> _rows = new List<UIPanel>();

        private const float ColumnWidth = 120f;
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
            
            UILabel titleLabel = AddUIComponent<UILabel>();
            titleLabel.text = "City Data";
            titleLabel.relativePosition = new Vector3(10, 7);
            
            _tableContainer = AddUIComponent<UIPanel>();
            _tableContainer.width = 530;
            _tableContainer.height = 270;
            _tableContainer.relativePosition = new Vector3(0, 30);
            _tableContainer.backgroundSprite = "GenericPanelLight";
            
            AddRow("City", "Cons.(MW)", "Prod.(MW)","Extra (MW)", isHeader: true);
        }

        public void UpdateGrid(Dictionary<string, CityData> cityData)
        {
            foreach (var row in _rows)
            {
                Destroy(row.gameObject);
            }
            _rows.Clear();

            if (!CityDataUI.ToggleContract)
            {
                AddRow("City", "Cons.(MW)", "Prod.(MW)","Extra (MW)", isHeader: true);
                foreach (var entry in cityData)
                {
                    AddRow(entry.Key, $"{entry.Value.ElectricConsumption / 1000}", $"{entry.Value.ElectricProduction / 1000}",$"{entry.Value.ElectricExtra / 1000}");
                }
            }
            else
            {
                AddRow("City", "Type", "Amount","Price", isHeader: true);
                foreach (var contract in cityData.SelectMany(entry => entry.Value.Contracts))
                {
                    AddRow( 
                        contract.From, 
                        contract.Type.ToString(),
                        contract.Amount.ToString(CultureInfo.InvariantCulture),
                        contract.Price.ToString(CultureInfo.InvariantCulture));
                }
            }
        }
        
        // Todo use table instead of this ugly line system
        private void AddRow(string line1, string line2, string line3,string line4, bool isHeader = false)
        {
            UIPanel rowPanel = _tableContainer.AddUIComponent<UIPanel>();
            rowPanel.width = _tableContainer.width;
            rowPanel.height = RowHeight;
            rowPanel.relativePosition = new Vector3(0, _rows.Count * RowHeight);
            rowPanel.backgroundSprite = isHeader ? "GenericPanel" : "GenericPanelLight";
            
            CreateCell(rowPanel, line1, 0, isHeader);
            CreateCell(rowPanel, line2, 1, isHeader);
            CreateCell(rowPanel, line3, 2, isHeader);
            CreateCell(rowPanel, line4, 3, isHeader);

            _rows.Add(rowPanel);
        }

        private void CreateCell(UIPanel row, string text, int columnIndex, bool isHeader)
        {
            UILabel cell = row.AddUIComponent<UILabel>();
            cell.text = text;
            cell.width = ColumnWidth;
            cell.height = RowHeight;
            cell.autoSize = false; 
            cell.relativePosition = new Vector3(columnIndex * ColumnWidth + 10, 8);
            cell.color = isHeader ? new Color32(255, 255, 255, 255) : new Color32(0, 0, 0, 200);
        }
    }
}