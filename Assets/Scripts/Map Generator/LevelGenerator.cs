using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : SingletonMonoBehavior<LevelGenerator>, ISaveManager
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
    [SerializeField] 
    private int monstersLevelPerDistance;
    [SerializeField] 
    private int monstersExperienceDropPerDistance;
    
    [Header("Database")]
    private readonly List<RoomInfo> roomInfos = new();
    private readonly List<(GameObject, string)> loadedRoomObjects = new();
    private readonly List<(GameObject, string)> loadedGeneratedOutlines = new();
    
    private readonly List<GameObject> roomObjects = new();
    private readonly List<GameObject> generatedOutlines = new();

    private Dictionary<int, HashSet<string[]>> levelDangerProfiles = new();
    private Dictionary<string, float> enemyDifficulties = new();
    
    private HashSet<(int, int)> distanceSet;
    
    private enum Direction {UP, DOWN, RIGHT, LEFT};

    private const float XOffset = 29;
    private const float YOffset = 19;
    private int currentRoomID = 0;
    private GameObject endRoom;
    private RoomCenter roomCenterDetails;

    protected override void Awake()
    {
        base.Awake();
        
         if (roomInfos.Count > 0)
        {
            Debug.Log("There are save items");
        
            foreach (RoomInfo roomInfo in roomInfos)
            {
                GameObject loadedNewRoom = Instantiate(layoutRoom, roomInfo.position, Quaternion.identity);
                loadedRoomObjects.Add((loadedNewRoom, roomInfo.individualRoomCenter.roomName));
            }
            
            foreach((GameObject, string) loadedRoom in loadedRoomObjects)
            {
                loadedGeneratedOutlines.Add((CreateRoomOutline(loadedRoom.Item1.transform.position), loadedRoom.Item2));
            }
            
            foreach((GameObject, string) loadedOutlines in loadedGeneratedOutlines)
            {
                RoomCenter loadedRoomCenter = GetRoomCenterByName(loadedOutlines.Item2);
                RoomCenter loadedRoomCenterDetails = Instantiate(loadedRoomCenter, loadedOutlines.Item1.transform.position, transform.rotation);
                loadedRoomCenterDetails.TheRoom = loadedOutlines.Item1.GetComponent<Room>();
                loadedRoomCenterDetails.TheRoom.roomCenterName = loadedRoomCenter.name;
                loadedRoomCenterDetails.name = loadedOutlines.Item2;
                if (loadedRoomCenterDetails.name == "Starting Center")
                {
                    loadedRoomCenterDetails.tag = "Starting Center";
                }

                foreach (RoomInfo room in roomInfos)
                {
                    if (room.position == loadedRoomCenterDetails.transform.position)
                    {
                        room.individualRoomCenter.roomCenter = loadedRoomCenterDetails;
                        break;
                    }
                }
            }

            foreach (RoomInfo room in roomInfos)
            {
                MonsterBatch monsterBatch = new MonsterBatch();
                
                if (room.monsterBatch != null)
                {
                    List<string> nameToUpdate = new();

                    foreach (var monster in room.monsterBatch)
                    {
                        if (monster.monsterGameObject == null)
                        {
                            nameToUpdate.Add(monster.monsterName);
                        }
                    }

                    foreach (string name in nameToUpdate)
                    {
                        float randomX = Random.Range(-9f, 7.5f);
                        float randomY = Random.Range(-3.5f, 3f);
                        Vector3 randomPosition = new Vector3(randomX, randomY, 0f);
                        
                        GameObject monsterObject = Instantiate(GetMonsterByName(name), room.individualRoomCenter.roomCenter.transform);
                        monsterObject.GetComponent<EnemyStats>().level = room.monsterLevel;
                        monsterObject.GetComponent<Enemy>().enemyExperienceDrop = room.monsterExperienceDrop;
                        monsterObject.transform.localPosition = randomPosition;
                        monsterObject.tag = "Enemy";
                        monsterObject.name = name;
                        room.individualRoomCenter.roomCenter.enemies.Add(monsterObject);
                        monsterObject.SetActive(false);

                        monsterBatch.AddMonster(name, monsterObject);
                    }
                }

                room.monsterBatch = monsterBatch;
            }
            
            return;
        }
        
        Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation).GetComponent<SpriteRenderer>().color = startingColor;
        
        selectedDirection = (Direction)Random.Range(0, 4);
        
        PopulateTheDangerProfiles();
        PopulateEnemyDifficulties();
        MoveGenerationPoint();
        GenerateMultipleRooms();
        ConvertToGraph();
        PopulateRoomInfos();
    }
    
    private void Update()
    {
        for (int i = 0; i < roomInfos.Count; i++)
        {
            RoomInfo room = roomInfos[i];
            if (room.monsterBatch == null)
            {
                continue;
            }
            
            List<int> monstersToRemove = new List<int>();
            
            for (int j = room.monsterBatch.monsters.Count - 1; j >= 0; j--)
            {
                MonsterInfo monster = room.monsterBatch.monsters[j];
                if (monster.monsterGameObject == null)
                {
                    monstersToRemove.Add(j);
                }
            }
            
            foreach (int index in monstersToRemove)
            {
                room.monsterBatch.monsters.RemoveAt(index);
            }
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
        generatedOutlines.Add(CreateRoomOutline(Vector3.zero));

        foreach(GameObject room in roomObjects)
        {
            generatedOutlines.Add(CreateRoomOutline(room.transform.position));
        }

        generatedOutlines.Add(CreateRoomOutline(endRoom.transform.position));

        foreach(GameObject outline in generatedOutlines)
        {
            bool generateCenter = false;
            
            if(outline.transform.position == Vector3.zero)
            {
                roomCenterDetails = Instantiate(centerStart, outline.transform.position, transform.rotation);
                roomCenterDetails.TheRoom = outline.GetComponent<Room>();
                roomCenterDetails.name = "Starting Center";
                roomCenterDetails.TheRoom.roomCenterName = roomCenterDetails.name;
                RoomCenterData roomCenterData = new RoomCenterData(roomCenterDetails.name, roomCenterDetails);
                RoomInfo roomInfo = new RoomInfo(new Vector3(0, 0, 0), currentRoomID, roomCenterData);
                roomInfos.Add(roomInfo);
                
                currentRoomID++;
                generateCenter = true;
            }
            
            if(!generateCenter && outline.transform.position != endRoom.transform.position)
            {
                roomCenterDetails = Instantiate(roomCenter, outline.transform.position, transform.rotation);
                roomCenterDetails.TheRoom = outline.GetComponent<Room>();
                roomCenterDetails.name = "Battle Center";
                roomCenterDetails.TheRoom.roomCenterName = roomCenterDetails.name;
                RoomCenterData roomCenterData = new RoomCenterData(roomCenterDetails.name, roomCenterDetails);
                RoomInfo roomInfo = new RoomInfo(outline.transform.position, currentRoomID, roomCenterData);
                roomInfos.Add(roomInfo);
                
                currentRoomID++;
            }
            
            if(outline.transform.position == endRoom.transform.position)
            {
                roomCenterDetails = Instantiate(centerEnd, outline.transform.position, transform.rotation);
                roomCenterDetails.TheRoom = outline.GetComponent<Room>();
                roomCenterDetails.name = "End Center";
                roomCenterDetails.TheRoom.roomCenterName = roomCenterDetails.name;
                RoomCenterData roomCenterData = new RoomCenterData(roomCenterDetails.name, roomCenterDetails);
                RoomInfo roomInfo = new RoomInfo(outline.transform.position, currentRoomID, roomCenterData);
                roomInfos.Add(roomInfo);
                
                currentRoomID++;
            }
        }
    }
    
    private GameObject CreateRoomOutline(Vector3 roomPosition)
    {
        bool roomAbove = Physics2D.OverlapCircle(roomPosition + new Vector3 (0f, YOffset, 0f), .2f, roomLayer);
        bool roomBelow = Physics2D.OverlapCircle(roomPosition + new Vector3 (0f, -YOffset, 0f), .2f, roomLayer);
        bool roomLeft = Physics2D.OverlapCircle(roomPosition + new Vector3 (-XOffset, 0f, 0f), .2f, roomLayer);
        bool roomRight = Physics2D.OverlapCircle(roomPosition + new Vector3 (XOffset, 0f, 0f), .2f, roomLayer);
        bool[] directions = { roomAbove, roomBelow, roomLeft, roomRight };
        
        int directionCount = 0;
        GameObject generatedOutline;

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
                    generatedOutline = Instantiate(rooms.singleUp, roomPosition, transform.rotation);
                    return generatedOutline;
                }
                if(roomBelow)
                {
                    generatedOutline = Instantiate(rooms.singleDown, roomPosition, transform.rotation);
                    return generatedOutline;
                }
                if(roomLeft)
                {
                    generatedOutline = Instantiate(rooms.singleLeft, roomPosition, transform.rotation);
                    return generatedOutline;
                }
                if(roomRight)
                {
                    generatedOutline = Instantiate(rooms.singleRight, roomPosition, transform.rotation);
                    return generatedOutline;
                }
                break;
            case 2:
                if (roomAbove && roomBelow)
                {
                    generatedOutline = Instantiate(rooms.doubleUpDown, roomPosition, transform.rotation);
                    return generatedOutline;
                }
                if (roomLeft && roomRight)
                {
                    generatedOutline = Instantiate(rooms.doubleLeftRight, roomPosition, transform.rotation);
                    return generatedOutline;
                }
                if (roomAbove && roomRight)
                {
                    generatedOutline = Instantiate(rooms.doubleUpRight, roomPosition, transform.rotation);
                    return generatedOutline;
                }
                if (roomRight && roomBelow)
                {
                    generatedOutline = Instantiate(rooms.doubleRightDown, roomPosition, transform.rotation);
                    return generatedOutline;
                }
                if (roomBelow && roomLeft)
                {
                    generatedOutline = Instantiate(rooms.doubleDownLeft, roomPosition, transform.rotation);
                    return generatedOutline;
                }
                if (roomLeft && roomAbove)
                {
                    generatedOutline = Instantiate(rooms.doubleLeftUp, roomPosition, transform.rotation);
                    return generatedOutline;
                }
                break;
            case 3:
                if (roomAbove && roomRight && roomBelow)
                {
                    generatedOutline = Instantiate(rooms.tripleUpRightDown, roomPosition, transform.rotation);
                    return generatedOutline;
                }
                if (roomRight && roomBelow && roomLeft)
                {
                    generatedOutline = Instantiate(rooms.tripleRightDownLeft, roomPosition, transform.rotation);
                    return generatedOutline;
                }
                if (roomBelow && roomLeft && roomAbove)
                {
                    generatedOutline = Instantiate(rooms.tripleDownLeftUp, roomPosition, transform.rotation);
                    return generatedOutline;
                }
                if (roomLeft && roomAbove && roomRight)
                {
                    generatedOutline = Instantiate(rooms.tripleLeftUpRight, roomPosition, transform.rotation);
                    return generatedOutline;
                }
                break;
            case 4:
                if (roomBelow && roomLeft && roomAbove && roomRight)
                {
                    generatedOutline = Instantiate(rooms.allFourSides, roomPosition, transform.rotation);
                    return generatedOutline;
                }
                break;
        }

        return null;
    }

    private void ConvertToGraph()
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
        graphTraversal.BFS(0);
        graphTraversal.SortByDistance();
        
        distanceSet = graphTraversal.GetDistancesAsHashSet();

        foreach (var room in roomInfos)
        {
            foreach ((int id, int danger) in distanceSet)
            {
                if (room.roomID == id)
                {
                    room.dangerLevel = danger;
                    room.monsterLevel = danger * monstersLevelPerDistance;
                    room.monsterExperienceDrop = danger * monstersExperienceDropPerDistance;
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

    private List<string[]> InitializeGeneticAlgorithm(int distance, int monsterLevel, out float passMonsterBatchDangerLevel)
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
        DNA result = solution.Solve(mutationRate, numberOfGenerations, levels, difficultyScores, distance, monsterLevel);
        
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
            if (room.roomID == 0 || room.roomID == distanceToEnd)
            {
                continue; 
            }

            List<string[]> dangerProfiles = InitializeGeneticAlgorithm(room.dangerLevel + 1, room.monsterLevel, out float monsterBatchDangerLevel);
            room.monsterBatchDangerLevel = monsterBatchDangerLevel;

            MonsterBatch monsterBatch = new MonsterBatch();

            foreach (string[] batch in dangerProfiles)
            {
                foreach (string monsterName in batch)
                {
                    GameObject monsterPrefab = GetMonsterByName(monsterName);
                    if (monsterPrefab != null)
                    {
                        float randomX = Random.Range(-9f, 7.5f);
                        float randomY = Random.Range(-3.5f, 3f);
                        Vector3 randomPosition = new Vector3(randomX, randomY, 0f);

                        GameObject monsterInstance = Instantiate(monsterPrefab, room.individualRoomCenter.roomCenter.transform);
                        monsterInstance.GetComponent<EnemyStats>().level = room.monsterLevel;
                        monsterInstance.GetComponent<Enemy>().enemyExperienceDrop = room.monsterExperienceDrop;
                        monsterInstance.transform.localPosition = randomPosition;
                        monsterInstance.tag = "Enemy";
                        monsterInstance.name = monsterName;
                        room.individualRoomCenter.roomCenter.enemies.Add(monsterInstance);
                        monsterInstance.SetActive(false);
                        
                        monsterBatch.AddMonster(monsterName, monsterInstance);
                    }
                }
            }
           
            room.monsterBatch = monsterBatch;
        }
    }
    
    private GameObject GetMonsterByName(string monsterName)
    {
        switch (monsterName)
        {
            case "slimes":
                return slime;
            case "skeletons":
                return skeleton;
            case "zombies":
                return zombie;
            default:
                return null;
        }
    }
    
    private RoomCenter GetRoomCenterByName(string centerName)
    {
        switch (centerName)
        {
            case "Starting Center":
                return centerStart;
            case "End Center":
                return centerEnd;
            case "Battle Center":
                return roomCenter;
            default:
                return null;
        }
    }

    public void LoadData(GameData data)
    {
        Debug.Log("Load Data");
        foreach (var savedRoomInfo in data.roomInfoSave)
        {
            RoomCenterData roomCenterData = new RoomCenterData(savedRoomInfo.individualRoomCenter.roomName, savedRoomInfo.individualRoomCenter.roomCenter);
            RoomInfo roomInfo = new RoomInfo(savedRoomInfo.position, savedRoomInfo.roomID, roomCenterData)
            {
                dangerLevel = savedRoomInfo.dangerLevel,
                monsterBatchDangerLevel = savedRoomInfo.monsterBatchDangerLevel,
                monsterBatch = new MonsterBatch(),
                monsterLevel = savedRoomInfo.monsterLevel,
                monsterExperienceDrop = savedRoomInfo.monsterExperienceDrop
            };
            
            foreach (var monsterInfo in savedRoomInfo.monsterBatch)
            {
                roomInfo.monsterBatch.AddMonster(monsterInfo.monsterName, null);
            }

            roomInfos.Add(roomInfo);
        }
    }
    
    public void SaveData(ref GameData data)
    {
        data.roomInfoSave.Clear();
        
        foreach (RoomInfo roomInfo in roomInfos)
        {
            RoomInfo newRoomInfoWithoutDuplicate = AddNewRoom(roomInfo);
            data.roomInfoSave.Add(newRoomInfoWithoutDuplicate);
        }
    }

    private RoomInfo AddNewRoom(RoomInfo roomInfo)
    {
        RoomInfo newRoomInfo = new RoomInfo(roomInfo.position, roomInfo.roomID, roomInfo.individualRoomCenter)
        {
            monsterBatch = roomInfo.monsterBatch,
            monsterBatchDangerLevel = roomInfo.monsterBatchDangerLevel,
            dangerLevel = roomInfo.dangerLevel,
            monsterLevel = roomInfo.monsterLevel,
            monsterExperienceDrop = roomInfo.monsterExperienceDrop
        };
        return newRoomInfo;
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