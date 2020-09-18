using UnityEngine;


public class Cell
{
    public int X { get; internal set; }
    public int Z { get; internal set; }
    public World world { get; internal set; }

    private Object _view;

    public void UpdateView()
    {
        Dispose();
        _view = Object.Instantiate(world.TestBlock, world.ToVector(X,Z), Quaternion.identity);
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
