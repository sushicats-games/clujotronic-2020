using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToolPicker : MonoBehaviour
{
    [SerializeField] private List<Tool> _tools;
    [SerializeField] private GameObject _toolsContainer;
    [SerializeField] private GameObject _buttonPrefab;
    private GameObject _colorButtonPrefab;
    private GameObject _colorEntryPrefab;
    private GameObject _colorPanel;
    private GameObject _colorPanelToggleButton;

    private static Color DEFAULT_COLOR = new Color(0, 0, 0); // white

    private List<string> colorsAsHTML = new List<string>
    {
        // pastels
        "#ffadad",  // 1 
        "#ffd6a5",  // 2
        "#fdffb6",  // 3
        "#caffbf",  // 4
        "#9bf6ff",  // 5
        "#a0c4ff",  // 6
        "#bdb2ff",  // 7
        "#ffc6ff",  // 8
        "#fffffc",   // 9

        // browns
        "#c6c7cc",
        "#838691",
        "#a67a61",
        "#5e5051",
        "#2e3446",
        "#777777",
        "#5a5d67",
        //"#7a655f",
        //"#6f8353"

    };
    private List<Color> COLORS;

    private Tool _currentTool = null;
    private Color _currentColor = DEFAULT_COLOR;

    void Start()
    {
        COLORS = colorsAsHTML
            .Select(htmlColor => GetColor(htmlColor))
            .ToList();
    }

    private static Color GetColor(string htmlColor)
    {
        ColorUtility.TryParseHtmlString(htmlColor, out Color color);
        return color;
    }

    public List<Tool> GetAvailableTools(string type = null)
    {
        if (string.IsNullOrEmpty(type))
        {
            return _tools.ToList();
        }

        return _tools.Where(tool => tool.Type == type).ToList();
    }

    public List<Color> GetAvailableColors()
    {
        return COLORS.ToList();
    }

    public Tool GetTool(string name)
    {
        return _tools.SingleOrDefault(tool => tool.Name == name);
    }

    public Tool GetPickedTool()
    {
        return _currentTool;
    }

    public Color GetPickedColor()
    {
        return _currentColor;
    }

    public void ClearPickedTool()
    {
        _currentTool = null;
    }

    public void PickTool(string name)
    {
        _currentTool = GetTool(name);
    }

    public void PickColor(string htmlColor)
    {
        _currentColor = GetColor(htmlColor);
    }

    private void _clearToolsContainer()
    {
        foreach (Transform child in _toolsContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    private void _clearColorsContainer()
    {
        foreach (Transform child in _colorPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    private void _loadColorArray()
    {
        _clearColorsContainer();

        if (_currentTool == null)
            return;

        if (_currentTool.Prefab.gameObject.name.Contains("wall"))
        {
            GameObject obj = Instantiate(_colorEntryPrefab, _colorPanel.transform);
            var text = obj.gameObject.transform.GetChild(0).GetComponent<TMP_Text>();
            text.text = "Color #1";

            GameObject colorContainer = obj.gameObject.transform.GetChild(0).gameObject;
            foreach (var color in COLORS)
            {
                GameObject btn = Instantiate(_colorButtonPrefab, colorContainer.transform);
                var img = btn.gameObject.transform.GetChild(0).GetComponent<Image>();
                img.color = _currentColor;
            }

            if (_currentTool.Prefab.gameObject.name.Contains("window") ||
                _currentTool.Prefab.gameObject.name.Contains("door"))
            {
                GameObject obj2 = Instantiate(_colorEntryPrefab, _colorPanel.transform);
                var text2 = obj2.gameObject.transform.GetChild(0).GetComponent<TMP_Text>();
                text2.text = "Color #2";

                GameObject colorContainer2 = obj.gameObject.transform.GetChild(0).gameObject;
                foreach (var color in COLORS)
                {
                    GameObject btn = Instantiate(_colorButtonPrefab, colorContainer2.transform);
                    var img = btn.gameObject.transform.GetChild(0).GetComponent<Image>();
                    img.color = _currentColor;
                }
            }
        }
    }

    #region Event Listeners
    public void OnTypeButtonClicked(string type)
    {
        _clearToolsContainer();

        var filteredTools = GetAvailableTools(type);

        foreach (var tool in filteredTools)
        {
            GameObject obj = Instantiate(_buttonPrefab, _toolsContainer.transform);
            obj.name = tool.name;
            Button btn = obj.GetComponent<Button>();
            btn.onClick.AddListener(() => OnToolClicked(tool.Name));
            Image icon = btn.gameObject.transform.GetChild(0).GetComponent<Image>();
            icon.sprite = tool.Icon;
        }
    }

    public void OnToolClicked(string name)
    {
        PickTool(name);
    }

    public void OnColorClicked(string htmlColor)
    {
        PickColor(htmlColor);

        // update "color button" color
    }

    public void OnColorButtonClicked()
    {
        // toggle "color panel"
    }

    #endregion

}