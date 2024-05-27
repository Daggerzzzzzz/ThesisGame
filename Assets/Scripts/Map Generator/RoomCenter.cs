using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomCenter : MonoBehaviour
{
    [SerializeField]
    private bool openWhenEnemiesCleared;
    
    public Room TheRoom { get; set; }
    public List<GameObject> enemies = new();
    
    private bool canSummonTheNextBatch;
    private int batchSize;
    private int destroyedCount;
    
    private TilemapRenderer tilemapRenderer;
    private bool alreadyPlaySounds;
    
    public Vector2 roomCenterPos;

    private void Awake()
    {
        tilemapRenderer = GetComponentInChildren<TilemapRenderer>();
        roomCenterPos = tilemapRenderer.bounds.center;
    }

    private void Start()
    {
        if(openWhenEnemiesCleared)
        {
            TheRoom.closeWhenEntered = true;
        }
        
        canSummonTheNextBatch = true;
        batchSize = 3;
        destroyedCount = 0;
    }

    void Update()
    {
        if(enemies.Count > 0 && TheRoom.roomActive && openWhenEnemiesCleared)
        {
            RemoveMissingEnemies();

            if(enemies.Count == 0)
            {
               TheRoom.OpenDoors();
            }
            
            SummonNextBatch(); 
            
            if (destroyedCount == batchSize) 
            {
                if (enemies.Count != 0)
                {
                    canSummonTheNextBatch = true;
                    SummonNextBatch(); 
                }
            }
        }
        else
        {
            RemoveMissingEnemies();

            if (enemies.Count == 0)
            {
                TheRoom.closeWhenEntered = false;
            }
        }
    }

    private void RemoveMissingEnemies()
    {
        for(int i = 0; i < enemies.Count; i++)
        {
            if(enemies[i] == null)
            {
                enemies.RemoveAt(i);
                i--;
                destroyedCount++;
            }
        }
    }

    private void SummonNextBatch()
    {
        if (canSummonTheNextBatch)
        {
            int remainder = enemies.Count % 3;
            
            batchSize = remainder != 0 ? remainder : 3;
            
            for (int i = 0; i < batchSize && i < enemies.Count ; i++)
            {
                enemies[i].SetActive(true);
            }
            
            canSummonTheNextBatch = false;
            destroyedCount = 0;
        }
    }
}

