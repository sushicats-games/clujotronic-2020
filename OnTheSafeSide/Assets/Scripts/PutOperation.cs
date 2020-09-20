using UnityEngine;


public class PutOperation
{
    public int cellX;
    public int cellZ;
    public int rotation; // 0 1 2 3
    public GameObject prefab;
    public CellSlot prefabCellSlot;

    public static bool operator ==(PutOperation a, PutOperation b)
    {
        if (Object.ReferenceEquals(a, b)) { return true; }
        if (Object.ReferenceEquals(a, null)) { return false; }
        if (Object.ReferenceEquals(b, null)) { return false; }
        return a.cellX == b.cellX
            && a.cellZ == b.cellZ
            && a.rotation == b.rotation
            && a.prefab == b.prefab
            && a.prefabCellSlot == b.prefabCellSlot;
    }

    public static bool operator !=(PutOperation a, PutOperation b)
    {
        return !(a == b);
    }

}
