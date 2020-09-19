using UnityEngine;

public class World : MonoBehaviour
{
    public Object TestBlock;
    public Material PreviewMaterial;

    float GridSize;
    int LengthX;
    int LengthZ;

    Wall[,] WallsGrid;
    Cell[,] CellsGrid;
    Cell Preview;

    // Start is called before the first frame update
    void Start()
    {
        GridSize = 1.0f;
        LengthX = 16;
        LengthZ = 16;
        WallsGrid = new Wall[LengthX + 1, LengthZ + 1];
        CellsGrid = new Cell[LengthX, LengthZ];
    }

    (int,int) ToCellCoords(Vector3 vector)
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

    void ReplaceCell(Vector3 vector, Cell newCell)
    {
        var (x, z) = ToCellCoords(vector);
        var existing = CellsGrid[x, z];
        if (existing != null)
        {
            existing.Dispose();
        }
        CellsGrid[x, z] = newCell;
        if (newCell != null)
        {
            ShowCell(newCell, x, z);
        }
    }

    private void ShowCell(Cell cell, int x, int z)
    {
        cell.X = x;
        cell.Z = z;
        cell.world = this;
        cell.UpdateView();
    }

    public void PutBlock(Vector3 vector)
    {
        var cell = new Cell();
        ReplaceCell(vector, cell);
    }

    public void PreviewBlock(Vector3 vector)
    {
        var (x, z) = ToCellCoords(vector);
        if (Preview != null)
        {
            if (Preview.X == x && Preview.Z == z)
            {
                return;
            }
            Preview.Dispose();
            Preview = null;
        }
        var existingCell = GetCell(vector);
        if (existingCell != null)
        {
            Debug.Log($"already something there {existingCell.Name}");
            return;
        }
        var cell = new Cell();
        cell.IsPreview = true;
        cell.MaterialOverride = PreviewMaterial;
        cell.Name = "Preview";
        Preview = cell;
        ShowCell(Preview, x, z);
    }

    public void EraseBlock(Vector3 vector)
    {
        ReplaceCell(vector, null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
