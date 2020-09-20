using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{
    public class BuildingInfo
    {
        public int id;
        public int size;
        public int floors;
        public int doors;
        public int windows;
        public int x;
        public int z;
        public HashSet<int> rooms = new HashSet<int>();
    }

    public IDictionary<int, int> roomStats = new Dictionary<int, int>();
    public IDictionary<int, BuildingInfo> buildingStats = new Dictionary<int, BuildingInfo>();
    public int statsVersion = 0;

    public World world;

    const int CellsPerTick = 1000;

    int lengthX;
    int lengthZ;

    int[,] propagationMap;
    int[,] roomMap;
    int[,] buildingMap;
    Dictionary<int, int> tempRoomStats;
    Dictionary<int, BuildingInfo> tempBuildingStats;

    int x = 0;
    int z = 0;
    int iteration = 0;
    int propagation = 0;

    enum State
    {
        RoomDetect,
        RoomStats,
        BuildingDetect,
        BuildingStats
    }

    State state = State.RoomDetect;

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
            propagationMap = new int[lengthX, lengthZ];
            roomMap = new int[lengthX, lengthZ];
            buildingMap = new int[lengthX, lengthZ];
            x = 0;
            z = 0;
            iteration = 0;
            propagation = 0;
        }
        for (int i=0; i< CellsPerTick; i++)
        {
            switch (state)
            {
                case State.RoomDetect:
                    ProcessRoomDetectCell();
                    break;
                case State.RoomStats:
                    ProcessRoomCopyCell();
                    break;
                case State.BuildingDetect:
                    ProcessBuildingDetectCell();
                    break;
                case State.BuildingStats:
                    ProcessBuildingCopyCell();
                    break;
            }
        }
    }

    void ProcessBuildingCopyCell()
    {
        if (x >= lengthX) { x = 0; z++; }
        if (z >= lengthZ)
        {
            // iteration complete
            z = 0;
            propagation = 0;
            state = State.RoomDetect; // start all over
            foreach (var stat in tempBuildingStats)
            {
                stat.Value.x /= stat.Value.size;
                stat.Value.z /= stat.Value.size;
            }
            buildingStats = tempBuildingStats;
            statsVersion++;
            return;
        }
        var v = propagationMap[x, z];
        buildingMap[x, z] = v;

        if (v != 0)
        {
            if (!tempBuildingStats.ContainsKey(v))
            {
                tempBuildingStats.Add(v, new BuildingInfo { 
                    doors = 0, 
                    floors = 0,
                    id = v,
                    rooms = new HashSet<int>(),
                    size = 0,
                    windows = 0,
                    x = 0,
                    z = 0,
                });
            }
            var stats = tempBuildingStats[v];
            stats.x += x;
            stats.z += z;
            stats.size++;
            stats.rooms.Add(roomMap[x, z]);
            if (world.HasFloor(x, z)) { stats.floors++; }
            if (world.HasDoor(x, z)) { stats.doors++; }
            if (world.HasWindow(x, z)) { stats.windows++; }
        }

        // DEBUG draw
        if (v != 0)
        {
            var lx = x - lengthX * 0.5f + 0.5f;
            var lz = z - lengthZ * 0.5f + 0.5f;
            Debug.DrawLine(new Vector3(lx, 0, lz), new Vector3(lx, 1, lz), new Color(Mathf.Sin(v * 95.234f), Mathf.Cos(v * 195.234f), Mathf.Sin(v * 295.234f)), 0.5f);
        }

        x++;

    }

    void ProcessBuildingDetectCell()
    {
        if (x >= lengthX) { x = 0; z++; }
        if (z >= lengthZ)
        {
            //Debug.Log($"it {iteration} p {propagation}");
            z = 0;
            if (iteration > 0 && propagation == 0)
            {
                // room detection complete
                state = State.BuildingStats;
                tempBuildingStats = new Dictionary<int, BuildingInfo>();
                //Debug.Log($"room detection complete! resetting");
                iteration = 0;
                return;
            }
            else
            {
                iteration++;
            }
            propagation = 0;
        }
        
        if (propagationMap[x, z] != 0)
        {
            PropagateBuilding(x, z, x + 1, z);
            PropagateBuilding(x, z, x - 1, z);
            PropagateBuilding(x, z, x, z + 1);
            PropagateBuilding(x, z, x, z - 1);
        }

        x++;
    }

    void ProcessRoomCopyCell()
    {
        if (x >= lengthX) { x = 0; z++; }
        if (z >= lengthZ)
        {
            // iteration complete
            z = 0;
            propagation = 0;
            state = State.BuildingDetect;
            roomStats = tempRoomStats;
            return;
        }
        var v = propagationMap[x, z];
        roomMap[x, z] = v;

        if (v != 0)
        {
            if (!tempRoomStats.ContainsKey(v))
            {
                tempRoomStats.Add(v, 0);
            }
            tempRoomStats[v]++;
        }

        // DEBUG draw
        if (v != 0)
        {
            var lx = x - lengthX * 0.5f + 0.5f;
            var lz = z - lengthZ * 0.5f + 0.5f;
            Debug.DrawLine(new Vector3(lx, 0, lz), new Vector3(lx, 1, lz), new Color(Mathf.Sin(v * 95.234f), Mathf.Cos(v * 195.234f), Mathf.Sin(v * 295.234f)), 0.5f);
        }

        x++;
    }

    void ProcessRoomDetectCell()
    {
        if (x >= lengthX) { x = 0; z++; }
        if (z >= lengthZ)
        {
            //Debug.Log($"it {iteration} p {propagation}");
            z = 0;
            if (iteration > 0 && propagation == 0)
            {
                // room detection complete
                state = State.RoomStats;
                tempRoomStats = new Dictionary<int, int>();
                //Debug.Log($"room detection complete! resetting");
                iteration = 0;
                return;
            }
            else
            {
                iteration++;
            }
            propagation = 0;
        }
        if (iteration == 0)
        {
            propagationMap[x, z] = x + z * lengthX + 1;
        }
        else if (propagationMap[x, z] != 0)
        {
            PropagateRoom(x, z, x + 1, z);
            PropagateRoom(x, z, x - 1, z);
            PropagateRoom(x, z, x, z + 1);
            PropagateRoom(x, z, x, z - 1);
        }

        x++;
    }

    private void PropagateRoom(int x, int z, int x2, int z2)
    {
        if (world.IsWallBetween(x, z, x2, z2))
        {
            return;
        }
        var currentId = propagationMap[x, z];
        var nextId = 0;
        if (world.IsInBounds(x2, z2))
        {
            nextId = propagationMap[x2, z2];
        }
        if (nextId < currentId)
        {
            propagation++;
            propagationMap[x, z] = nextId;
        }
    }

    private void PropagateBuilding(int x, int z, int x2, int z2)
    {
        var currentId = propagationMap[x, z];
        if (!world.IsInBounds(x2, z2))
        {
            return;
        }
        var nextId = propagationMap[x2, z2];
        if (nextId != 0 && nextId < currentId)
        {
            // don't propagate 0
            propagation++;
            propagationMap[x, z] = nextId;
        }
    }
}
