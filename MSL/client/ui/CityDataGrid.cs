using System.Collections.Generic;
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
            
            AddRow("City", "Cons.(MW)", "Prod.(MW)","Extra (MW)", isHeader: true);
            foreach (var entry in cityData)
            {
                AddRow(entry.Key, $"{entry.Value.ElectricConsumption / 1000}", $"{entry.Value.ElectricProduction / 1000}",$"{entry.Value.ElectricExtra / 1000}");
            }
        }

        private void AddRow(string cityName, string consumption, string production,string extra, bool isHeader = false)
        {
            UIPanel rowPanel = _tableContainer.AddUIComponent<UIPanel>();
            rowPanel.width = _tableContainer.width;
            rowPanel.height = RowHeight;
            rowPanel.relativePosition = new Vector3(0, _rows.Count * RowHeight);
            rowPanel.backgroundSprite = isHeader ? "GenericPanel" : "GenericPanelLight";
            
            CreateCell(rowPanel, cityName, 0, isHeader);
            CreateCell(rowPanel, consumption, 1, isHeader);
            CreateCell(rowPanel, production, 2, isHeader);
            CreateCell(rowPanel, extra, 3, isHeader);

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