using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    [Header("Room Layout Setup")]
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
    private RoomCenter roomCenter;

    [Header("Genetic Algorithm Setup")] 
    [SerializeField]
    private int populationSize = 200;
    [SerializeField]
    private float mutationRate = 0.01f;
    [SerializeField]
    private int numberOfGenerations = 50;
    
    [Header("Monsters")]
    [SerializeField] 
    private GameObject slime;
    [SerializeField] 
    private GameObject skeleton;
    [SerializeField] 
    private GameObject zombie;
    
    private readonly List<GameObject> generatedOutlines = new();
    private readonly List<GameObject> roomObjects = new();
    private readonly List<RoomInfo> roomInfos = new();
    private readonly List<RoomCenter> roomCenterDatas = new();

    private Dictionary<int, HashSet<string[]>> levelDangerProfiles = new();
    private Dictionary<string, float> enemyDifficulties = new();
    
    private HashSet<(int, int)> distanceSet;
    
    private enum Direction {UP, DOWN, RIGHT, LEFT};

    private const float XOffset = 29;
    private const float YOffset = 19;
    private int currentRoomID = 0;
    private GameObject endRoom;
    private RoomCenter roomCenterDetails;

    void Start()
    {
        Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation).GetComponent<SpriteRenderer>().color = startingColor;
        
        selectedDirection = (Direction)Random.Range(0, 4);
        
        PopulateTheDangerProfiles();
        PopulateEnemyDifficulties();
        MoveGenerationPoint();
        GenerateMultipleRooms();
        ConvertToGraph();
        PopulateRoomInfos();
        PopulateRoomCenters();
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
                roomCenterDetails = Instantiate(centerStart, outline.transform.position, transform.rotation);
                roomCenterDetails.TheRoom = outline.GetComponent<Room>();
                roomCenterDetails.name = "Starting Center";
                RoomInfo roomInfo = new RoomInfo(new Vector3(0, 0, 0), currentRoomID, centerStart);
                roomInfos.Add(roomInfo);
                roomCenterDatas.Add(roomCenterDetails);
                
                currentRoomID++;
                generateCenter = true;
            }
            
            if(!generateCenter && outline.transform.position != endRoom.transform.position)
            {
                roomCenterDetails = Instantiate(roomCenter, outline.transform.position, transform.rotation);
                roomCenterDetails.TheRoom = outline.GetComponent<Room>();
                roomCenterDetails.name = "Battle Center";
                RoomInfo roomInfo = new RoomInfo(outline.transform.position, currentRoomID, roomCenter);
                roomInfos.Add(roomInfo);
                roomCenterDatas.Add(roomCenterDetails);
                
                currentRoomID++;
            }
            
            if(outline.transform.position == endRoom.transform.position)
            {
                roomCenterDetails = Instantiate(centerEnd, outline.transform.position, transform.rotation);
                roomCenterDetails.TheRoom = outline.GetComponent<Room>();
                roomCenterDetails.name = "End Center";
                RoomInfo roomInfo = new RoomInfo(outline.transform.position, currentRoomID, centerEnd);
                roomInfos.Add(roomInfo);
                roomCenterDatas.Add(roomCenterDetails);
                
                currentRoomID++;
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
                if (roomInfo.Position == vertex)
                {
                    tempVertex = roomInfo.RoomID;
                    visitedVertex.Add(tempVertex);
                }
            }

            foreach (var roomInfo in roomInfos)
            {
                if (edges.Contains(roomInfo.Position))
                {
                    tempEdges.Add(roomInfo.RoomID);
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

        graphTraversal.PrintTraversal();
        graphTraversal.BFS(0);
        graphTraversal.SortByDistance();
        
        distanceSet = graphTraversal.GetDistancesAsHashSet();

        foreach (var room in roomInfos)
        {
            foreach ((int id, int danger) in distanceSet)
            {
                if (room.RoomID == id)
                {
                    room.DangerLevel = danger;
                }
            }
        }
    }
    
    private void PopulateTheDangerProfiles()
    {
        levelDangerProfiles.Add(1, new HashSet<string[]>
        {
            new[] { "zombies" },
            new[] { "zombies" },
            new[] { "zombies" }
        });
        levelDangerProfiles.Add(2, new HashSet<string[]>
        {
            new[] { "skeletons" },
            new[] { "zombies" },
            new[] { "zombies" }
        });
        levelDangerProfiles.Add(3, new HashSet<string[]>
        {
            new[] { "slimes" },
            new[] { "zombies" },
            new[] { "zombies" }
        });
        levelDangerProfiles.Add(4, new HashSet<string[]>
        {
            new[] { "skeletons" },
            new[] { "skeletons" },
            new[] { "zombies" }
        });
        levelDangerProfiles.Add(5, new HashSet<string[]>
        {
            new[] { "skeletons" },
            new[] { "slimes" },
            new[] { "zombies" }
        });
        levelDangerProfiles.Add(6, new HashSet<string[]>
        {
            new[] { "slimes" },
            new[] { "slimes" },
            new[] { "zombies" }
        });
        levelDangerProfiles.Add(7, new HashSet<string[]>
        {
            new[] { "skeletons" },
            new[] { "skeletons" },
            new[] { "skeletons" }
        });
        levelDangerProfiles.Add(8, new HashSet<string[]>
        {
            new[] { "skeletons" },
            new[] { "skeletons" },
            new[] { "slimes" }
        });
        levelDangerProfiles.Add(9, new HashSet<string[]>
        {
            new[] { "skeletons" },
            new[] { "slimes" },
            new[] { "slimes" }
        });
        levelDangerProfiles.Add(10, new HashSet<string[]>
        {
            new[] { "slimes" },
            new[] { "slimes" },
            new[] { "slimes" }
        });
    }
    
    private void PopulateEnemyDifficulties()
    {
        enemyDifficulties.Add("slimes", 0.25f);
        enemyDifficulties.Add("skeletons", 0.50f);
        enemyDifficulties.Add("zombies", 0.75f);
    }

    private List<string[]> InitializeGeneticAlgorithm(int distance, out float passMonsterBatchDangerLevel)
    {
        List<int> levels = new ();
        List<float> difficultyScores = new ();
        List<List<string[]>> monsterLists = new ();
        List<string[]> dangerProfiles = new List<string[]>();
        
        foreach (var kvp in levelDangerProfiles)
        {
            levels.Add(kvp.Key);
            monsterLists.Add(new List<string[]>(kvp.Value));
        }
        
        foreach (List<string[]> monsters in monsterLists)
        {
            float totalDifficulty = 0;
            
            foreach (string[] monsterArray in monsters)
            {
                foreach (string monster in monsterArray)
                {
                    if (enemyDifficulties.ContainsKey(monster))
                    {
                        totalDifficulty += enemyDifficulties[monster];
                    }
                }
            }
            difficultyScores.Add(totalDifficulty);
        }

        GeneticAlgorithm solution = new GeneticAlgorithm(populationSize);
        DNA result = solution.Solve(mutationRate, numberOfGenerations, levels, difficultyScores, distance);
        
        for (int i = 0; i < result.Chromosome.Count; i++)
        {
            int levelIndex = i + 1;
            if (result.Chromosome[i] == 1)
            {
                HashSet<string[]> profiles = levelDangerProfiles[levelIndex];
                foreach (string[] profile in profiles)
                {
                    dangerProfiles.Add(profile);
                }
            }
        }

        passMonsterBatchDangerLevel = result.ScoreEvaluation;
        return dangerProfiles;
    }
    
    private void PopulateRoomInfos()
    {
        foreach (var room in roomInfos)
        {
            if (room.RoomID == 0 || room.RoomID == distanceToEnd)
            {
                continue;
            }
            List<string[]> dangerProfiles = InitializeGeneticAlgorithm(room.DangerLevel + 1, out float monsterBatchDangerLevel);
            room.MonsterBatch = dangerProfiles;
            room.MonsterBatchDangerLevel = monsterBatchDangerLevel;
        }
    }
    
    private void PopulateRoomCenters()
    {
        foreach (var room in roomInfos)
        {
            foreach (var center in roomCenterDatas)
            {
                if (center.transform.position == room.Position)
                {
                    int counter = 0;
                    if (room.MonsterBatch == null)
                    {
                        center.NumberOfEnemies = 0;
                        continue;
                    }
                    center.NumberOfEnemies = room.MonsterBatch.Count;
                    foreach (var monsters in room.MonsterBatch)
                    {
                        foreach (var monster in monsters)
                        {
                            float randomX = Random.Range(-9f, 7.5f);
                            float randomY = Random.Range(-3.5f, 3f);
                            Vector3 randomPosition = new Vector3(randomX, randomY, 0f);
                            
                            if (monster == "slimes")
                            {
                                GameObject slimeInstance = Instantiate(slime, center.transform);
                                slimeInstance.transform.localPosition = randomPosition;
                                slimeInstance.name = "Slime Enemy " + counter;
                                slimeInstance.tag = "Enemy";
                                center.Enemies.Add(slimeInstance);
                                slimeInstance.SetActive(false);
                            }
                            if (monster == "skeletons")
                            {
                                GameObject skeletonInstance = Instantiate(skeleton, center.transform);
                                skeletonInstance.transform.localPosition = randomPosition;
                                skeletonInstance.name = "Skeleton Enemy " + counter;
                                skeletonInstance.tag = "Enemy";
                                center.Enemies.Add(skeletonInstance);
                                skeletonInstance.SetActive(false);
                            }
                            if (monster == "zombies")
                            {
                                GameObject zombieInstance = Instantiate(zombie, center.transform);
                                zombieInstance.transform.localPosition = randomPosition;
                                zombieInstance.name = "Zombie Enemy " + counter;
                                zombieInstance.tag = "Enemy";
                                center.Enemies.Add(zombieInstance);
                                zombieInstance.SetActive(false);
                            }
                        }
                        
                        counter++;
                    }
                }
            }
        }
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