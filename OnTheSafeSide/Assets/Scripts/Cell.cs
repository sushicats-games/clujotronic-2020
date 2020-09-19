using UnityEngine;


public class Cell
{
    public int x { get; internal set; }
    public int z { get; internal set; }
    public int rotation = 0; // 0 1 2 3 
    public World world { get; internal set; }
    public bool isPreview { get; internal set; }
    public string name { get; internal set; }
    public Material materialOverride { get; set; }

    private GameObject _view;

    public void UpdateView()
    {
        Dispose();

        _view = (GameObject) Object.Instantiate(
            original: world.TestBlock,
            position: world.ToVector(x,z), 
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
        if (name != null)
        {
            _view.name = name;
        }
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
