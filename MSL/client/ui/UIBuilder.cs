using System.Collections.Generic;
using ColossalFramework.UI;
using UnityEngine;

namespace MSL.client.ui
{
    public static class UIBuilder
    {
        public static UISlider CreateSlider(
            UIPanel panel,
            string label,
            float yOffset,
            float min, 
            float max,
            float step,
            UITextField textField)
        {
            UILabel sliderLabel = panel.AddUIComponent<UILabel>();
            sliderLabel.text = label;
            sliderLabel.relativePosition = new Vector3(10, yOffset);
            
            UISlider slider = panel.AddUIComponent<UISlider>();
            slider.width = 250;
            slider.height = 10;
            slider.relativePosition = new Vector3(120, yOffset);
            slider.minValue = min;
            slider.maxValue = max;
            slider.stepSize = step;
            slider.value = min;
            
            UISlicedSprite sliderBackground = slider.AddUIComponent<UISlicedSprite>();
            sliderBackground.relativePosition = Vector3.zero;
            sliderBackground.size = new Vector2(250, 10);
            sliderBackground.spriteName = "ScrollbarTrack";
            
            UISprite sliderThumb = slider.AddUIComponent<UISprite>();
            sliderThumb.spriteName = "ScrollbarThumb";
            sliderThumb.size = new Vector2(15, 15);
            slider.thumbObject = sliderThumb;
            
            textField = panel.AddUIComponent<UITextField>();
            textField.text = min.ToString();
            textField.width = 50;
            textField.height = 20;
            textField.relativePosition = new Vector3(380, yOffset - 5);

            slider.eventValueChanged += (component, value) => textField.text = value.ToString("0");

            return slider;
        }
        
        public static UIButton CreateButton(UIPanel panel,string text,float width, float xOffset, float yOffset)
        {
            UIButton button = panel.AddUIComponent<UIButton>();
            button.text = text;
            button.width = width;
            button.height = 30;
            button.relativePosition = new Vector3(xOffset, yOffset);
            button.normalBgSprite = "ButtonMenu";
            button.hoveredBgSprite = "ButtonMenuHovered";
            button.pressedBgSprite = "ButtonMenuPressed";
            return button;
        }
        
        // This component is buggy, need to be reworked
        public static UIDropDown CreateDropdown(UIPanel panel, string label, float yOffset, List<string> options)
        {
            UILabel dropdownLabel = panel.AddUIComponent<UILabel>();
            dropdownLabel.text = label;
            dropdownLabel.relativePosition = new Vector3(10, yOffset);
            UIDropDown dropdown = panel.AddUIComponent<UIDropDown>();
            dropdown.popupTextColor = new Color32(255, 255, 255, 255);
            dropdown.popupColor = new Color32(255, 255, 255, 125);
            dropdown.normalBgSprite = "ButtonMenu";     
            dropdown.hoveredBgSprite = "ButtonMenuHovered";
            dropdown.focusedBgSprite = "GenericPanel";  
            dropdown.isInteractive = true;
            dropdown.itemHover = "ListItemHover";
            dropdown.itemHighlight = "ListItemHighlight";
            dropdown.isVisible = true;
            dropdown.listWidth = 250;
            dropdown.listHeight = 150;
            dropdown.width = 250;
            dropdown.height = 25;
            dropdown.relativePosition = new Vector3(120, yOffset - 5);
            dropdown.items = options.ToArray();
            dropdown.selectedIndex = 0;
            dropdown.color = new Color32(0, 0, 0, 125); 
            dropdown.foregroundSpriteMode = UIForegroundSpriteMode.Scale;
            dropdown.zOrder = 1;
            dropdown.listPosition = UIDropDown.PopupListPosition.Below;
            
            return dropdown;
        }
    }
}