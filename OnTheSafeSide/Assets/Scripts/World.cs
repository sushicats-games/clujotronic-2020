using System;
using UnityEngine;

public partial class World : MonoBehaviour
{
    public Material PreviewMaterial;

    float GridSize;
    int LengthX;
    int LengthZ;

    Cell[,] CellsGrid;
    Cell previewCell;

    // Start is called before the first frame update
    void Start()
    {
        previewCell = new Cell { name = "Preview", materialOverride = PreviewMaterial, isPreview = true, world = this };
        GridSize = 1.0f;
        LengthX = 16;
        LengthZ = 16;
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

    public void EraseBlock(Vector3 vector)
    {
        GetCell(vector)?.Clear();
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

    public bool IsInBounds(PutOperation op) => IsInBounds(op.cellX, op.cellZ);
}
