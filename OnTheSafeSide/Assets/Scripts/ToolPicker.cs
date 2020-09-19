using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ToolPicker : MonoBehaviour
{
    [SerializeField] private List<Tool> _tools;
    [SerializeField] private GameObject _toolsContainer;
    [SerializeField] private GameObject _buttonPrefab;


    private Tool _currentTool = null;

    public List<Tool> GetAvailableTools(string type = null)
    {
        if (string.IsNullOrEmpty(type))
        {
            return _tools.ToList();
        }

        return _tools.Where(tool => tool.Type == type).ToList();
    }

    public Tool GetTool(string name)
    {
        return _tools.SingleOrDefault(tool => tool.Name == name);
    }

    public Tool GetPickedTool(string name)
    {
        return _currentTool;
    }

    public void PickTool(string name)
    {
        _currentTool = GetTool(name);
    }

    private void _clearToolsContainer()
    {
        foreach (Transform child in _toolsContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
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
        }
    }

    public void OnToolClicked(string name)
    {
        PickTool(name);
    }
    #endregion

}