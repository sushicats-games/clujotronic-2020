using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class World : MonoBehaviour
{
    public Material PreviewMaterial;

    float GridSize;
    public int LengthX { private set; get; }
    public int LengthZ { private set; get; }

    Cell[,] CellsGrid;
    Cell previewCell;

    // Start is called before the first frame update
    void Start()
    {
        previewCell = new Cell { name = "Preview", materialOverride = PreviewMaterial, isPreview = true, world = this };
        GridSize = 1.0f;
        LengthX = 64;
        LengthZ = 64;
        CellsGrid = new Cell[LengthX, LengthZ];
    }

    public (int, int) ToCellCoords(Vector3 vector)
    {
        int x = Mathf.FloorToInt(vector.x / GridSize + LengthX * 0.5f);
        int z = Mathf.FloorToInt(vector.z / GridSize + LengthZ * 0.5f);
        return (x, z);
    }

    public Vector3 ToVector(int x, int z)
    {
        return new Vector3((x - LengthX * 0.5f + 0.5f) * GridSize, 0, (z - LengthZ * 0.5f + 0.5f) * GridSize);
    }

    public void PutBlock(PutOperation op)
    {
        if (!IsInBounds(op)) { return; };
        var cell = GetOrAllocEmptyCell(op.cellX, op.cellZ);
        var slot = op.prefabCellSlot.Rotate(op.rotation);
        cell.PutDecorator(slot, op.prefab, op.rotation);
        ClearNeighbourWallSlots(op.cellX, op.cellZ, slot);
    }

    private void ClearNeighbourWallSlots(int x, int z, CellSlot slot)
    {
        for (int i=0; i<4; i++)
        {
            var wallSlot = CellSlotExtensions.ToWallSlot(i);
            if (slot.HasFlag(wallSlot))
            {
                var oppositeWallSlot = CellSlotExtensions.RotateWall(wallSlot, 2);
                var (dx, dz) = wallSlot.ToDirectionVector();
                ClearCellSlot(x + dx, z + dz, oppositeWallSlot);
            }
        }
    }

    internal bool IsWallBetween(int x, int z, int x2, int z2)
    {
        var c1 = GetCell(x, z);
        var c2 = GetCell(x2, z2);
        var slot1 = CellSlotExtensions.CellSlotFromDirectionVector(x2 - x, z2 - z);
        var slot2 = CellSlotExtensions.OppositeWall(slot1);
        return c1 != null && !c1.IsSingleWallEmpty(slot1) || c2 != null && !c2.IsSingleWallEmpty(slot2);
    }

    private void ClearCellSlot(int x, int z, CellSlot oppositeWallSlot)
    {
        var cell = GetCell(x, z);
        cell?.PutDecorator(oppositeWallSlot, null, 0);
    }

    public void ClearPreviewBlock()
    {
        previewCell?.Clear();
    }

    public void ShowPreviewBlock(PutOperation op)
    {
        previewCell.Clear();
        if (!IsInBounds(op)) { return; };
        previewCell.x = op.cellX;
        previewCell.z = op.cellZ;
        previewCell.PutDecorator(op.prefabCellSlot, op.prefab, op.rotation);
    }

    public bool EraseBlock(int x, int z)
    {
        return GetCell(x,z)?.Clear() ?? false;
    }

    Cell GetCell(Vector3 vector)
    {
        var (x, z) = ToCellCoords(vector);
        return GetCell(x, z);
    }

    Cell GetCell(int x, int z)
    {
        if (!IsInBounds(x, z))
        {
            return null;
        }
        return CellsGrid[x, z];
    }

    Cell GetOrAllocEmptyCell(int x, int z)
    {
        if (CellsGrid[x, z] == null)
        {
            CellsGrid[x, z] = new Cell { x = x, z = z, world = this };
        }
        return CellsGrid[x, z];
    }

    public bool IsInBounds(int x, int z) => x >= 0 && x < LengthX && z >= 0 && z < LengthZ;

    internal bool HasFloor(int x, int z) => GetCell(x, z)?.HasFloor() ?? false;

    internal bool HasDoor(int x, int z) => GetCell(x, z)?.HasDoor() ?? false;

    internal bool HasWindow(int x, int z) => GetCell(x, z)?.HasWindow() ?? false;

    public bool IsInBounds(PutOperation op) => IsInBounds(op.cellX, op.cellZ);

    public IEnumerable<Cell> AllCells()
    {
        for (int z = 0; z < LengthZ; z++)
        {
            for (int x = 0; x < LengthX; x++)
            {
                var cell = CellsGrid[x, z];
                if (cell != null)
                {
                    yield return cell;
                }
            }
        }
    }
}
