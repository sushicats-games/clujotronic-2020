using UnityEngine;

public class CellDecorator
{
    public GameObject prefab { get; internal set; }
    public int rotation = 0; // 0 1 2 3 

    private GameObject _view;

    public void UpdateView(Vector3 position, Material materialOverride, bool isPreview, string name)
    {
        Dispose();

        _view = (GameObject)Object.Instantiate(
            original: prefab,
            position: position,
            rotation: Quaternion.Euler(0, rotation * 90, 0));

        if (materialOverride != null)
        {
            var rend = _view.GetComponent<MeshRenderer>();
            rend.material = materialOverride;
        }
        if (isPreview)
        {
            var collider = _view.GetComponent<Collider>();
            if (collider)
            {
                collider.enabled = false;
            }
        }
        if (name == null)
        {
            name = "Cd";
        }

        _view.name = $"{name}:{prefab.name}:r{rotation}";
    }

    public void Dispose()
    {
        if (_view)
        {
            Object.Destroy(_view);
            _view = null;
        }
    }

}
