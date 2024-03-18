using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    [Header("Room Layout")]
    [SerializeField]
    private GameObject layoutRoom;
    [SerializeField]
    private int distanceToEnd;
    [SerializeField]
    private Color startingColor, endingColor;
    [SerializeField]
    private Transform generatorPoint;
    [SerializeField]
    private Direction selectedDirection;
    [SerializeField]
    private LayerMask roomLayer;
    [SerializeField]
    private RoomPrefabs rooms;
    [SerializeField]
    private RoomCenter centerStart;
    [SerializeField]
    private RoomCenter centerEnd;
    [SerializeField]
    private RoomCenter[] potentialCenters;

    [Header("Genetic Algorithm Setup")] 
    [SerializeField]
    private int populationSize = 200;
    [SerializeField]
    private float mutationRate = 0.01f;
    [SerializeField]
    private int elitism = 5;
    private System.Random random = new();
    
    
    private readonly List<GameObject> generatedOutlines = new();
    private readonly List<GameObject> roomObjects = new();
    private readonly List<RoomInfo> roomInfos = new();
    
    private static Dictionary<int, HashSet<string[]>> levelDangerProfiles = new();
    private static Dictionary<string, int> enemyDifficulties = new();
    
    private HashSet<(int, int)> distanceSet;
    private GeneticAlgorithm<int> geneticAlgorithm;
    
    private enum Direction {UP, DOWN, RIGHT, LEFT};

    private const float XOffset = 29;
    private const float YOffset = 19;
    private int currentRoomID = 0;
    private double epsilon = 0.0001;
    private GameObject endRoom;

    void Start()
    {
        Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation).GetComponent<SpriteRenderer>().color = startingColor;
        
        selectedDirection = (Direction)Random.Range(0, 4);
        
        RoomInfo roomInfo = new RoomInfo(new Vector3(0, 0, 0), currentRoomID);
        roomInfos.Add(roomInfo);
        
        currentRoomID++;

        PopulateTheDangerProfiles();
        PopulateEnemyDifficulties();
        MoveGenerationPoint();
        GenerateMultipleRooms();
        ConvertToGraph();

        geneticAlgorithm = new GeneticAlgorithm<int>(populationSize, levelDangerProfiles.Count, random, GetRandomDangerProfile, FitnessFunction, elitism, mutationRate);
    }
    
    void Update()
    {
        geneticAlgorithm.NewGeneration();
        if (Math.Abs(geneticAlgorithm.BestFitness - 1) < epsilon)
        {
            enabled = false;
        }
    }
    
    private void MoveGenerationPoint()
    {
        switch(selectedDirection)
        {
            case Direction.UP:
                generatorPoint.position += new Vector3(0f, YOffset, 0f);
                break;
            case Direction.DOWN:
                generatorPoint.position += new Vector3(0f, -YOffset, 0f);
                break;
            case Direction.RIGHT:
                generatorPoint.position += new Vector3(XOffset, 0f, 0f);
                break;
            case Direction.LEFT:
                generatorPoint.position += new Vector3(-XOffset, 0f, 0f);
                break;
        }
    }
    
    private void GenerateMultipleRooms()
    {
        for(int i = 0; i < distanceToEnd; i++)
        {
            GameObject newRoom = Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation);
            roomObjects.Add(newRoom);
            
            RoomInfo roomInfo = new RoomInfo(newRoom.transform.position, currentRoomID);
            roomInfos.Add(roomInfo);
        
            currentRoomID++;


            if(i + 1 == distanceToEnd)
            {
                newRoom.GetComponent<SpriteRenderer>().color = endingColor;
                roomObjects.RemoveAt(roomObjects.Count - 1);
                endRoom = newRoom;
            }

            selectedDirection = (Direction)Random.Range(0, 4);
            MoveGenerationPoint();

            while(Physics2D.OverlapCircle(generatorPoint.position, .2f, roomLayer))
            {
                MoveGenerationPoint();
            }
        }
        CreateRoomOutline(Vector3.zero);

        foreach(GameObject room in roomObjects)
        {
            CreateRoomOutline(room.transform.position);
        }

        CreateRoomOutline(endRoom.transform.position);

        foreach(GameObject outline in generatedOutlines)
        {
            bool generateCenter = false;
            if(outline.transform.position == Vector3.zero)
            {
                Instantiate(centerStart, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                generateCenter = true;
            }
            if(outline.transform.position == endRoom.transform.position)
            {
                Instantiate(centerEnd, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
            }
            if(!generateCenter)
            {
                int centerSelect = Random.Range(0, potentialCenters.Length);
                Instantiate(potentialCenters[centerSelect], outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
            }
        }
    }
    
    private void CreateRoomOutline(Vector3 roomPosition)
    {
        bool roomAbove = Physics2D.OverlapCircle(roomPosition + new Vector3 (0f, YOffset, 0f), .2f, roomLayer);
        bool roomBelow = Physics2D.OverlapCircle(roomPosition + new Vector3 (0f, -YOffset, 0f), .2f, roomLayer);
        bool roomLeft = Physics2D.OverlapCircle(roomPosition + new Vector3 (-XOffset, 0f, 0f), .2f, roomLayer);
        bool roomRight = Physics2D.OverlapCircle(roomPosition + new Vector3 (XOffset, 0f, 0f), .2f, roomLayer);
        bool[] directions = { roomAbove, roomBelow, roomLeft, roomRight };
        
        int directionCount = 0;

        foreach (bool direction in directions)
        {
            if (direction)
            {
                directionCount++;
            }
        }

        switch(directionCount)
        {
            case 0:
                Debug.LogError("Found No Room Exists");
                break;
            case 1:
                if(roomAbove)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleUp, roomPosition, transform.rotation));
                }
                if(roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleDown, roomPosition, transform.rotation));
                }
                if(roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleLeft, roomPosition, transform.rotation));
                }
                if(roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleRight, roomPosition, transform.rotation));
                }
                break;
            case 2:
                if (roomAbove && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleUpDown, roomPosition, transform.rotation));
                }
                if (roomLeft && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleLeftRight, roomPosition, transform.rotation));
                }
                if (roomAbove && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleUpRight, roomPosition, transform.rotation));
                }
                if (roomRight && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleRightDown, roomPosition, transform.rotation));
                }
                if (roomBelow && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleDownLeft, roomPosition, transform.rotation));
                }
                if (roomLeft && roomAbove)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleLeftUp, roomPosition, transform.rotation));
                }
                break;
            case 3:
                if (roomAbove && roomRight && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleUpRightDown, roomPosition, transform.rotation));
                }
                if (roomRight && roomBelow && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleRightDownLeft, roomPosition, transform.rotation));
                }
                if (roomBelow && roomLeft && roomAbove)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleDownLeftUp, roomPosition, transform.rotation));
                }
                if (roomLeft && roomAbove && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleLeftUpRight, roomPosition, transform.rotation));
                }
                break;
            case 4:
                if (roomBelow && roomLeft && roomAbove && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.allFourSides, roomPosition, transform.rotation));
                }
                break;
        }
    }

    public void ConvertToGraph()
    {
        Vector3 vertex = new();
        HashSet<Vector3> edges = new();
        HashSet<int> visitedVertex = new();
        int tempVertex = 0;
        HashSet<int> tempEdges = new();
        GraphTraversal graphTraversal = new (distanceToEnd + 1);
        
        foreach (GameObject outline in generatedOutlines)
        {
            edges.Clear();
            tempEdges.Clear();
            
            generatorPoint.position = outline.transform.position;
            for (int i = 0; i < 4; i++)
            {
                generatorPoint.position = outline.transform.position;
                selectedDirection = (Direction)i;
                MoveGenerationPoint();
                Collider2D overlappedObject  = Physics2D.OverlapCircle(generatorPoint.position, .2f, roomLayer);
                if(overlappedObject)
                {
                    GameObject collidedRooms = overlappedObject.gameObject;
                    vertex = outline.transform.position;
                    if (vertex != collidedRooms.transform.position)
                    {
                        edges.Add(overlappedObject.transform.position);
                    }
                }
            }
            foreach (var roomInfo in roomInfos)
            {
                if (roomInfo.position == vertex)
                {
                    tempVertex = roomInfo.roomID;
                    visitedVertex.Add(tempVertex);
                }
            }

            foreach (var roomInfo in roomInfos)
            {
                if (edges.Contains(roomInfo.position))
                {
                    tempEdges.Add(roomInfo.roomID);
                }
            }
            foreach (var edge in tempEdges)
            {
                if (!visitedVertex.Contains(edge))
                {
                    graphTraversal.AddEdge(tempVertex, edge);
                }
            }
        }
        graphTraversal.PrintAdjacencyMatrix();
        graphTraversal.BFS(0);
        graphTraversal.SortByDistance();
        
        distanceSet = graphTraversal.GetDistancesAsHashSet();
    }
    
    private static void PopulateTheDangerProfiles()
    {
        levelDangerProfiles.Add(1, new HashSet<string[]>
        {
            new[] { "rat" },
            new[] { "zombie" },
            new[] { "zombie" }
        });

        levelDangerProfiles.Add(2, new HashSet<string[]>
        {
            new[] { "rat", "rat" },
            new[] { "zombie", "zombie", "rat" },
            new[] { "zombie", "zombie" },
            new[] { "zombie", "zombie" },
            new[] { "zombie", "zombie", "zombie" }
        });

        levelDangerProfiles.Add(3, new HashSet<string[]>
        {
            new[] { "rat", "rat", "rat" },
            new[] { "zombie", "zombie", "rat", "rat" },
            new[] { "zombie", "zombie", "zombie", "rat" },
            new[] { "zombie", "zombie", "zombie" },
            new[] { "zombie", "zombie", "zombie" },
            new[] { "zombie", "zombie", "zombie", "rat" }
        });
    }
    
    private static void PopulateEnemyDifficulties()
    {
        enemyDifficulties.Add("rat", 2);
        enemyDifficulties.Add("zombie", 5);
    }

    private int GetRandomDangerProfile()
    {
        int randomLevel = random.Next(1, 4);
        return randomLevel;
    }

    private float FitnessFunction(int roomId)
    {
        return 0;
    }
}

[Serializable]
public class RoomPrefabs
{
    public GameObject singleUp, singleDown, singleRight, singleLeft,
                    doubleUpDown, doubleLeftRight, doubleUpRight, doubleRightDown,
                    doubleDownLeft, doubleLeftUp, tripleUpRightDown, tripleRightDownLeft,
                    tripleDownLeftUp, tripleLeftUpRight, allFourSides;
}