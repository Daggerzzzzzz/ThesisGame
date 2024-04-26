using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomCenter : MonoBehaviour
{
    [field:SerializeField]
    public Room TheRoom { get; set; }
    [field: SerializeField] 
    public int NumberOfEnemies { get; set; }
    [field:SerializeField]
    public List<GameObject> Enemies { get; set; } = new();
    
    [SerializeField]
    private bool openWhenEnemiesCleared;

    private bool canSummonTheNextBatch = true;
    private int batchSize = 3;
    private int destroyedCount = 0;
    
    private void Start() 
    {
        if(openWhenEnemiesCleared)
        {
            TheRoom.closeWhenEntered = true;
        }
    }

    void Update()
    {
        if(Enemies.Count > 0 && TheRoom.roomActive && openWhenEnemiesCleared)
        {
            SummonNextBatch(); 
            
            for(int i = 0; i < Enemies.Count; i++)
            {
                if(Enemies[i] == null)
                {
                    Enemies.RemoveAt(i);
                    i--;
                    destroyedCount++;
                }
            }

            if(Enemies.Count == 0)
            {
               TheRoom.OpenDoors();
            }
            
            if (destroyedCount == batchSize) 
            {
                if (Enemies.Count != 0)
                {
                    canSummonTheNextBatch = true;
                    SummonNextBatch(); 
                }
            }
        }
    }
    
    private void SummonNextBatch()
    {
        if (canSummonTheNextBatch)
        {
            for (int i = 0; i < batchSize; i++)
            {
                Enemies[i].SetActive(true);
            }

            canSummonTheNextBatch = false;
            destroyedCount = 0;
        }
    }
}

