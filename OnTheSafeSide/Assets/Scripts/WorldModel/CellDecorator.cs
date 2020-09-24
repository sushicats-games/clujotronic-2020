using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
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

        var rend = _view.GetComponent<MeshRenderer>();

        rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        if (materialOverride != null)
        {
            // rend.material = materialOverride;
            rend.materials = rend.materials.Select(m => materialOverride).ToArray();
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
