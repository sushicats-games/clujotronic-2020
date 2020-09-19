using UnityEngine;
public class Tool : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] private string _type;
    [SerializeField] private bool _isCorner;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Sprite icon;

    public string Name { get => _name; set => _name = value; }
    public string Type { get => _type; set => _type = value; } // wall, floor, roof, foliageanddecor
    public bool IsCorner { get => _isCorner; set => _isCorner = value; }
    public GameObject Prefab { get => _prefab; set => _prefab = value; }
    public Sprite Icon { get => icon; set => icon = value; }
}
