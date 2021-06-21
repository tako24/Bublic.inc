using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class StageGeneration : MonoBehaviour
{
    public bool WithRepetitions;
    public int RoomsCount;
    public int RoomsDensity;
    public int BossSpawnDelay;
    public AstarPath AstarPath;
    public GameObject BossRoom;
    public GameObject ShopRoom;
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

    private bool spawnShop;
    private float delta=0;

    void Start()
    {
        Initialize();
        
        GenerateStage();

        AstarPath.Scan();
    }

    private void Update()
    {
        if (delta <= 0)
        {
            AstarPath.Scan();
            delta = 5f;
        }
        delta -= Time.deltaTime;
    }


    private void Initialize()
    {
        spawnedRooms = new List<GameObject>();

        roomToSpawn = RoomsPrefabs[0];
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
    }

    private void GenerateStage()
    {
        for (int i = 1; i < RoomsCount; i++)
        {
            if (i == RoomsCount - 1)
            {
                roomToSpawnFrom = spawnedRooms[UnityEngine.Random.Range(1, spawnedRooms.Count)];
                spawnShop = true;
            }
            else
                roomToSpawnFrom = spawnedRooms[UnityEngine.Random.Range(0, spawnedRooms.Count)];

            mapX = roomToSpawnFrom.GetComponent<RoomProperties>().MapX;
            mapY = roomToSpawnFrom.GetComponent<RoomProperties>().MapY;
            var possibleDirections = GetPossibleDirections();

            if (possibleDirections.Count == 0)
            {
                spawnedRooms.Remove(roomToSpawnFrom);
                i--;
                continue;
            }

            var spawnDirection = possibleDirections[UnityEngine.Random.Range(0, possibleDirections.Count)];

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
        roomToSpawn = spawnShop ? ShopRoom : RoomsPrefabs[UnityEngine.Random.Range(1, RoomsPrefabs.Count)];

        if(!WithRepetitions)
            RoomsPrefabs.Remove(roomToSpawn);

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

    public void GenerateBossRoom()
    {
        var roomsWithDistance = new Dictionary<GameObject, int>();
        var mapX = GameController.CurrentRoom.MapX;
        var mapY = GameController.CurrentRoom.MapY;

        var maxDistance = 0;
        foreach (var room in spawnedRooms)
        {
            var distance = 0;
            distance += System.Math.Abs(room.GetComponent<RoomProperties>().MapX - mapX);
            distance += System.Math.Abs(room.GetComponent<RoomProperties>().MapY - mapY);
            roomsWithDistance.Add(room, distance);

            if (distance > maxDistance)
                maxDistance = distance;
        }

        var distantRooms = new List<GameObject>();
        foreach (var room in roomsWithDistance)
            if (room.Value == maxDistance)
                distantRooms.Add(room.Key);

        var possibleDirections = new List<Direction>();
        for (int i = 0; i < distantRooms.Count; i++)
        {
            roomToSpawnFrom = distantRooms[UnityEngine.Random.Range(0, distantRooms.Count)];
            mapX = roomToSpawnFrom.GetComponent<RoomProperties>().MapX;
            mapY = roomToSpawnFrom.GetComponent<RoomProperties>().MapY;
            possibleDirections = GetPossibleDirections();

            if (possibleDirections.Count == 0)
            {
                distantRooms.Remove(roomToSpawnFrom);
                i--;
                continue;
            }
        }

        var spawnDirection = possibleDirections[UnityEngine.Random.Range(0, possibleDirections.Count)];

        switch (spawnDirection)
        {
            case Direction.Up:
                {
                    SpawnBossRoom(1, 1, 0, 1);
                    break;
                }
            case Direction.Down:
                {
                    SpawnBossRoom(-1, -1, 0, -1);
                    break;
                }
            case Direction.Right:
                {
                    SpawnBossRoom(1, -1, 1, 0);
                    break;
                }
            case Direction.Left:
                {
                    SpawnBossRoom(-1, 1, -1, 0);
                    break;
                }
            default:
                break;
        }

        RoomsMap[mapX, mapY] = lastSpawnedRoom;
        lastSpawnedRoom.transform.parent = transform;
        spawnedRooms.Add(lastSpawnedRoom);
    }

    private void SpawnBossRoom(int xSign, int ySign, int mapDX, int mapDY)
    {
        roomToSpawn = BossRoom;
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
        grid.nodeSize = 0.5f;
        grid.collision.mask = LayerMask.GetMask(new string[] { "column" });
        grid.rotation = new Vector3(45, 270, 270);
        var position = lastSpawnedRoom.transform.position;
        grid.center = new Vector3(position.x, position.y + 0.244f, position.z);
        var size = roomToSpawn.GetComponent<AISize>();
        if (size!=null)
            grid.SetDimensions(size.size * (int)Math.Ceiling(1 / grid.nodeSize), size.size * (int)Math.Ceiling(1 / grid.nodeSize), grid.nodeSize);
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
