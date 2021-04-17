using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class StageGeneration : MonoBehaviour
{
    public int RoomsCount;
    public int RoomsDensity;
    public AstarPath AstarPath;
    public List<GameObject> RoomsPrefabs;
    public GameObject LeftConnection;
    public GameObject RightConnection;

    private GameObject[,] RoomsMap;
    private int mapX;
    private int mapY;

    private GameObject roomToSpawn;
    private GameObject lastSpawnedRoom;
    private GameObject roomToSpawnFrom;

    private List<GameObject> spawnedRooms;

    void Start()
    {
        Initialize();
        
        GenerateStage();

        AstarPath.Scan();

        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.GetComponent<AIDestinationSetter>().target = GameObject.Find("Player 1").transform;
            enemy.GetComponent<AIPath>().canSearch = false;
        }
    }

    private void Initialize()
    {
        spawnedRooms = new List<GameObject>();

        roomToSpawn = RoomsPrefabs[Random.Range(0, RoomsPrefabs.Count)];
        lastSpawnedRoom = Instantiate(roomToSpawn, Vector2.zero, Quaternion.identity);
        lastSpawnedRoom.transform.parent = transform;
        spawnedRooms.Add(lastSpawnedRoom);

        RoomsMap = new GameObject[RoomsCount * 2 - 1, RoomsCount * 2 - 1];
        mapX = RoomsCount - 1;
        mapY = RoomsCount - 1;
        lastSpawnedRoom.GetComponent<RoomProperties>().MapX = mapX;
        lastSpawnedRoom.GetComponent<RoomProperties>().MapY = mapY;
        RoomsMap[mapX, mapY] = lastSpawnedRoom;

        GameController.CurrentRoom = RoomsMap[mapX, mapY].GetComponent<RoomProperties>();
        GameController.CurrentRoom.IsCleared = true;

        lastSpawnedRoom.transform.Find("Square").gameObject.SetActive(false);
    }

    private void GenerateStage()
    {
        for (int i = 1; i < RoomsCount; i++)
        {
            roomToSpawnFrom = spawnedRooms[Random.Range(0, spawnedRooms.Count)];
            mapX = roomToSpawnFrom.GetComponent<RoomProperties>().MapX;
            mapY = roomToSpawnFrom.GetComponent<RoomProperties>().MapY;
            var possibleDirections = GetPossibleDirections();

            if (possibleDirections.Count == 0)
            {
                spawnedRooms.Remove(roomToSpawnFrom);
                i--;
                continue;
            }

            var spawnDirection = possibleDirections[Random.Range(0, possibleDirections.Count)];

            switch (spawnDirection)
            {
                case Direction.Up:
                    {
                        SpawnRoom(1, 1, 0, 1);
                        break;
                    }
                case Direction.Down:
                    {
                        SpawnRoom(-1, -1, 0, -1);
                        break;
                    }
                case Direction.Right:
                    {
                        SpawnRoom(1, -1, 1, 0);
                        break;
                    }
                case Direction.Left:
                    {
                        SpawnRoom(-1, 1, -1, 0);
                        break;
                    }
                default:
                    break;
            }

            RoomsMap[mapX, mapY] = lastSpawnedRoom;
            lastSpawnedRoom.transform.parent = transform;
            spawnedRooms.Add(lastSpawnedRoom);
        }
    }

    private void SpawnRoom(int xSign, int ySign, int mapDX, int mapDY)
    {
        roomToSpawn = RoomsPrefabs[Random.Range(0, RoomsPrefabs.Count)];
        var maxRoomSize = System.Math.Max(roomToSpawn.GetComponent<RoomProperties>().Size,
            roomToSpawnFrom.GetComponent<RoomProperties>().Size);
        var roomDX = maxRoomSize - RoomsDensity;
        var roomDY = roomDX / 2;

        lastSpawnedRoom = Instantiate(roomToSpawn, new Vector3(roomToSpawnFrom.transform.position.x + roomDX * xSign,
        roomToSpawnFrom.transform.position.y + roomDY * ySign, 0), Quaternion.identity);
        GenerateAiNet();
        lastSpawnedRoom.GetComponent<RoomProperties>().MapX = roomToSpawnFrom.GetComponent<RoomProperties>().MapX + mapDX;
        lastSpawnedRoom.GetComponent<RoomProperties>().MapY = roomToSpawnFrom.GetComponent<RoomProperties>().MapY + mapDY;
        mapX += mapDX;
        mapY += mapDY;
        MakeConnection(xSign, ySign, maxRoomSize);
    }

    private void GenerateAiNet()
    {
        var grid = AstarPath.data.AddGraph(typeof(GridGraph)) as GridGraph;
        grid.inspectorGridMode = InspectorGridMode.IsometricGrid;
        grid.isometricAngle = 60;
        grid.collision.use2D = true;
        grid.rotation = new Vector3(45, 270, 270);
        var position = lastSpawnedRoom.transform.position;
        grid.center = new Vector3(position.x, position.y + 0.244f, position.z);
        switch (roomToSpawn.name)
        {
            case "Room 1":
                grid.SetDimensions(10,10,grid.nodeSize);
                break;
            case "Room 2":
                grid.SetDimensions(7, 7, grid.nodeSize);
                break;
        }
    }

    private void MakeConnection(int xSign, int ySign, float maxRoomSize)
    {
        var minRoomSize = System.Math.Min(roomToSpawn.GetComponent<RoomProperties>().Size,
            roomToSpawnFrom.GetComponent<RoomProperties>().Size);
        var roomSize = roomToSpawnFrom.GetComponent<RoomProperties>().Size;
        var connectionX = roomToSpawnFrom.transform.position.x + (roomSize / 2 - (roomSize - 2) / 4) * xSign;
        var connectionY = roomToSpawnFrom.transform.position.y + (roomSize / 2 - (roomSize - 2) / 4) / 2 * ySign;

        var connectionLength = maxRoomSize + (maxRoomSize - minRoomSize) / 2 - 1 - RoomsDensity * 2;
        for (int connectionsCount = 0; connectionsCount < connectionLength; connectionsCount++)
        {
            var connectionToSpawn = xSign * ySign == 1 ? RightConnection : LeftConnection;
            var connectionPart = Instantiate(connectionToSpawn, new Vector3(connectionX, connectionY, 0), Quaternion.identity);
            connectionPart.transform.parent = transform;
            connectionX += 0.5f * xSign;
            connectionY += 0.25f * ySign;
        }

        switch ((xSign, ySign))
        {
            case (1, 1):
                roomToSpawnFrom.GetComponent<RoomProperties>().SpawnExit(Direction.Up);
                lastSpawnedRoom.GetComponent<RoomProperties>().SpawnExit(Direction.Down);
                break;
            case (-1, -1):
                roomToSpawnFrom.GetComponent<RoomProperties>().SpawnExit(Direction.Down);
                lastSpawnedRoom.GetComponent<RoomProperties>().SpawnExit(Direction.Up);
                break;
            case (1, -1):
                roomToSpawnFrom.GetComponent<RoomProperties>().SpawnExit(Direction.Right);
                lastSpawnedRoom.GetComponent<RoomProperties>().SpawnExit(Direction.Left);
                break;
            case (-1, 1):
                roomToSpawnFrom.GetComponent<RoomProperties>().SpawnExit(Direction.Left);
                lastSpawnedRoom.GetComponent<RoomProperties>().SpawnExit(Direction.Right);
                break;
        }
    }

    private List<Direction> GetPossibleDirections()
    {
        var possibleDirections = new List<Direction>();
        for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 1; y++)
            {
                if (x * y != 0) continue;
                if (RoomsMap[mapX + x, mapY + y] == null)
                {
                    if (y == 1)
                        possibleDirections.Add(Direction.Up);
                    else if (y == -1)
                        possibleDirections.Add(Direction.Down);
                    else if (x == 1)
                        possibleDirections.Add(Direction.Right);
                    else if (x == -1)
                        possibleDirections.Add(Direction.Left);
                }
            }

        return possibleDirections;
    }
}
