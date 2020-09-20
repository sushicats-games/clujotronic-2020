using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cell : IEnumerable<CellDecorator>
{
    public World world { get; internal set; }

    public int x { get; internal set; }
    public int z { get; internal set; }
    public bool isPreview { get; internal set; }
    public Material materialOverride { get; set; }
    public string name;

    CellDecorator Wall0;
    CellDecorator Wall1;
    CellDecorator Wall2;
    CellDecorator Wall3;

    CellDecorator Floor;
    CellDecorator Ceiling;

    public bool IsEmpty()
    {
        return Floor == null && Ceiling == null && Wall0 == null && Wall1 == null && Wall2 == null && Wall3 == null;
    }

    public bool IsSingleWallEmpty(CellSlot slot)
    {
        switch (slot)
        {
            case CellSlot.Wall0: return Wall0 == null;
            case CellSlot.Wall1: return Wall1 == null;
            case CellSlot.Wall2: return Wall2 == null;
            case CellSlot.Wall3: return Wall3 == null;
            default: throw new InvalidOperationException();
        }
    }

    public void PutDecorator(CellSlot slot, GameObject prefab, int rotation)
    {
        var deco = prefab == null ? null : new CellDecorator
        {
            prefab = prefab,
            rotation = rotation
        };

        if (slot.HasFlag(CellSlot.Floor))
        {
            DisposeDeco(Floor);
            Floor = deco;
        }

        if (slot.HasFlag(CellSlot.Ceiling))
        {
            DisposeDeco(Ceiling);
            Ceiling = deco;
        }

        if (slot.HasFlag(CellSlot.Wall0))
        {
            DisposeDeco(Wall0);
            Wall0 = deco;
        }

        if (slot.HasFlag(CellSlot.Wall1))
        {
            DisposeDeco(Wall1);
            Wall1 = deco;
        }

        if (slot.HasFlag(CellSlot.Wall2))
        {
            DisposeDeco(Wall2);
            Wall2 = deco;
        }

        if (slot.HasFlag(CellSlot.Wall3))
        {
            DisposeDeco(Wall3);
            Wall3 = deco;
        }

        UpdateView();
    }

    private void DisposeDeco(CellDecorator deco)
    {
        if (deco == null)
        {
            return;
        }
        deco.Dispose();
        if (Floor == deco) { Floor = null; };
        if (Ceiling == deco) { Ceiling = null; };
        if (Wall0 == deco) { Wall0 = null; };
        if (Wall1 == deco) { Wall1 = null; };
        if (Wall2 == deco) { Wall2 = null; };
        if (Wall3 == deco) { Wall3 = null; };
    }

    public void UpdateView()
    {
        var position = world.ToVector(x, z);
        foreach (var deco in this)
        {
            deco.UpdateView(position, materialOverride, isPreview, name);
        }
    }

    public bool Clear()
    {
        bool wasEmpty = IsEmpty();
        foreach (var deco in this)
        {
            deco.Dispose();
        }
        Floor = null;
        Ceiling = null;
        Wall0 = null;
        Wall1 = null;
        Wall2 = null;
        Wall3 = null;
        return !wasEmpty;
    }

    public IEnumerator<CellDecorator> GetEnumerator() 
    {
        if (Floor != null) { yield return Floor; }
        if (Ceiling != null) { yield return Ceiling; }
        if (Wall0 != null) { yield return Wall0; }
        if (Wall1 != null) { yield return Wall1; }
        if (Wall2 != null) { yield return Wall2; }
        if (Wall3 != null) { yield return Wall3; }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    internal bool HasFloor() => Floor != null;

    internal bool HasDoor() => IsDoor(Wall0) || IsDoor(Wall1) || IsDoor(Wall2) || IsDoor(Wall3);

    internal bool HasWindow() => IsWindow(Wall0) || IsWindow(Wall1) || IsWindow(Wall2) || IsWindow(Wall3);

    bool IsDoor(CellDecorator deco) => deco?.prefab.name.Contains("door") ?? false;

    bool IsWindow(CellDecorator deco) => deco?.prefab.name.Contains("window") ?? false;
}
