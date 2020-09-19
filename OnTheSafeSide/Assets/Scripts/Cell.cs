using UnityEngine;


public class Cell
{
    public int X { get; internal set; }
    public int Z { get; internal set; }
    public World world { get; internal set; }
    public bool IsPreview { get; internal set; }
    public string Name { get; internal set; }
    public Material MaterialOverride { get; set; }

    private GameObject _view;

    public void UpdateView()
    {
        Dispose();
        _view = (GameObject) Object.Instantiate(world.TestBlock, world.ToVector(X,Z), Quaternion.identity);
        if (MaterialOverride != null)
        {
            var rend = _view.GetComponent<MeshRenderer>();
            rend.material = MaterialOverride;
        }
        if (IsPreview)
        {
            var collider = _view.GetComponent<Collider>();
            collider.enabled = false;
        }
        if (Name != null)
        {
            _view.name = Name;
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
