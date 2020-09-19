using UnityEngine;

class Detection : MonoBehaviour
{
    public World world;

    const int CellsPerTick = 1000;

    int lengthX;
    int lengthZ;

    int[,] roomDetection;

    int x = 0;
    int z = 0;
    int iteration = 0;
    int propagation = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (lengthX != world.LengthX || lengthZ != world.LengthZ)
        {
            lengthX = world.LengthX;
            lengthZ = world.LengthZ;
            roomDetection = new int[lengthX, lengthZ];
            x = 0;
            z = 0;
            iteration = 0;
            propagation = 0;
        }
        for (int i=0; i< CellsPerTick; i++)
        {
            ProcessCell();
        }
    }

    void ProcessCell()
    {
        if (x >= lengthX)
        {
            x = 0;
            z++;
        }
        if (z >= lengthZ)
        {
            Debug.Log($"it {iteration} p {propagation}");
            z = 0;
            if (iteration > 0 && propagation == 0)
            {
                Debug.Log($"room detection complete! resetting");
                iteration = 0;
            }
            else
            {
                iteration++;
            }
            propagation = 0;
        }
        if (iteration == 0)
        {
            roomDetection[x, z] = x + z * lengthX + 1;
        }
        else if (roomDetection[x, z] != 0)
        {
            Propagate(x, z, x + 1, z);
            Propagate(x, z, x - 1, z);
            Propagate(x, z, x, z + 1);
            Propagate(x, z, x, z - 1);
        }

        var v = roomDetection[x, z];
        var lx = x - lengthX * 0.5f + 0.5f;
        var lz = z - lengthZ * 0.5f + 0.5f;
        if (iteration > 0)
        {
            Debug.DrawLine(new Vector3(lx, 0, lz), new Vector3(lx, 1, lz), new Color(Mathf.Sin(v * 95.234f), Mathf.Cos(v * 195.234f), Mathf.Sin(v * 295.234f)), 0.5f);
        }
        x++;
    }

    private void Propagate(int x, int z, int x2, int z2)
    {
        if (world.IsWallBetween(x, z, x2, z2))
        {
            return;
        }
        var currentId = roomDetection[x, z];
        var nextId = 0;
        if (world.IsInBounds(x2, z2))
        {
            nextId = roomDetection[x2, z2];
        }
        if (nextId < currentId)
        {
            propagation++;
            roomDetection[x, z] = nextId;
        }
    }
}
