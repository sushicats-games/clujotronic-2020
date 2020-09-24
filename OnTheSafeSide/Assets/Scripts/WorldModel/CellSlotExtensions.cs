using System;

public static class CellSlotExtensions {
    
    public static CellSlot Rotate(this CellSlot slot, int rotation)
    {
        CellSlot result = CellSlot.None;
        if (slot.HasFlag(CellSlot.Floor)) { result |= CellSlot.Floor; }
        if (slot.HasFlag(CellSlot.Ceiling)) { result |= CellSlot.Ceiling; }
        if (slot.HasFlag(CellSlot.Wall0)) { result |= RotateWall(CellSlot.Wall0, rotation); }
        if (slot.HasFlag(CellSlot.Wall1)) { result |= RotateWall(CellSlot.Wall1, rotation); }
        if (slot.HasFlag(CellSlot.Wall2)) { result |= RotateWall(CellSlot.Wall2, rotation); }
        if (slot.HasFlag(CellSlot.Wall3)) { result |= RotateWall(CellSlot.Wall3, rotation); }
        return result;
    }

    public static CellSlot RotateWall(CellSlot wallSlot, int rotation)
    {
        return ToWallSlot(wallSlot.ToDirection() + rotation);
    }

    public static CellSlot OppositeWall(CellSlot wallSlot)
    {
        return RotateWall(wallSlot, 2);
    }

    public static int ToDirection(this CellSlot wallSlot)
    {
        switch (wallSlot)
        {
            case CellSlot.Wall0: return 0;
            case CellSlot.Wall1: return 1;
            case CellSlot.Wall2: return 2;
            case CellSlot.Wall3: return 3;
            default:
                throw new InvalidOperationException();
        }
    }

    public static CellSlot ToWallSlot(int direction)
    {
        direction &= 3;
        switch (direction)
        {
            case 0: return CellSlot.Wall0;
            case 1: return CellSlot.Wall1;
            case 2: return CellSlot.Wall2;
            case 3: return CellSlot.Wall3;
            default:
                throw new InvalidOperationException();
        }
    }

    public static (int, int) ToDirectionVector(this CellSlot slot)
    {
        switch (slot)
        {
            case CellSlot.Wall0: return (0, -1);
            case CellSlot.Wall1: return (-1, 0);
            case CellSlot.Wall2: return (0, +1);
            case CellSlot.Wall3: return (+1, 0);
            default:
                throw new InvalidOperationException();
        }
    }

    public static CellSlot CellSlotFromDirectionVector(int x, int z)
    {
        if (x == 0 && z == -1) { return CellSlot.Wall0; }
        if (x == -1 && z == 0) { return CellSlot.Wall1; }
        if (x == 0 && z == +1) { return CellSlot.Wall2; }
        if (x == +1 && z == 0) { return CellSlot.Wall3; }
        throw new InvalidOperationException();
    }

}
