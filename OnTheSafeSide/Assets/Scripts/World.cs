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

    public (int,int) ToCellCoords(Vector3 vector)
    {
        int x = Mathf.FloorToInt(vector.x / GridSize + LengthX * 0.5f);
        int z = Mathf.FloorToInt(vector.z / GridSize + LengthZ * 0.5f);
        return (x, z);
    }

    public Vector3 ToVector(int x, int z)
    {
        return new Vector3((x - LengthX * 0.5f + 0.5f) * GridSize, 0, (z - LengthZ * 0.5f + 0.5f) * GridSize);
    }

    Cell GetCell(Vector3 vector)
    {
        var (x, z) = ToCellCoords(vector);
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

    public void PutBlock(PutOperation op)
    {
        var (x, z) = (op.cellX, op.cellZ);
        var cell = GetOrAllocEmptyCell(x, z);
        if (op.rotation == 2)
        {
            Debug.DebugBreak();
        }
        cell.PutDecorator(op.prefabCellSlot, op.prefab, op.rotation);
    }

    public void ShowPreviewBlock(PutOperation op)
    {
        previewCell.Clear();
        previewCell.x = op.cellX;
        previewCell.z = op.cellZ;
        previewCell.PutDecorator(op.prefabCellSlot, op.prefab, op.rotation);
    }

    public void EraseBlock(Vector3 vector)
    {
        GetCell(vector)?.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
