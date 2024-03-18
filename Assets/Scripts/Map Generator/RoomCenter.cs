using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCenter : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> enemies = new();
    [SerializeField]
    private bool openWhenEnemiesCleared;
    [HideInInspector]
    public Room theRoom;

    private void Start() 
    {
        if(openWhenEnemiesCleared)
        {
            theRoom.closeWhenEntered = true;
        }
    }

    void Update()
    {
        if(enemies.Count > 0 && theRoom.roomActive && openWhenEnemiesCleared)
        {
            for(int i = 0; i < enemies.Count; i++)
            {
                if(enemies[i] == null)
                {
                    enemies.RemoveAt(i);
                    i--;
                }
            }

            if(enemies.Count == 0)
            {
               theRoom.OpenDoors();
            }
        }
    }
}
