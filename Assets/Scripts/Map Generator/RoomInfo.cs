using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomInfo
{
    public int roomID;
    public int dangerLevel;
    public int monsterLevel;
    public int monsterExperienceDrop;
    public float monsterBatchDangerLevel;
    public Vector3 position;
    public MonsterBatch monsterBatch;
    public RoomCenterData individualRoomCenter;
    
    public RoomInfo(Vector3 pos, int id,RoomCenterData roomCenter)
    {
        roomID = id;
        position = pos;
        individualRoomCenter = roomCenter;
    }
}

[System.Serializable]
public class RoomCenterData
{
    public string roomName;
    public RoomCenter roomCenter;
    
    public RoomCenterData(string roomName, RoomCenter roomCenter)
    {
        this.roomName = roomName;
        this.roomCenter = roomCenter;
    }
}

[System.Serializable]
public class MonsterBatch : IEnumerable<MonsterInfo>
{
    public List<MonsterInfo> monsters = new();
    
    public void AddMonster(string monsterName, GameObject monsterGameObject)
    {
        monsters.Add(new MonsterInfo(monsterName, monsterGameObject));
    }
    
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    public IEnumerator<MonsterInfo> GetEnumerator()
    {
        return monsters.GetEnumerator();
    }
}

[System.Serializable]
public class MonsterInfo
{
    public string monsterName;
    public GameObject monsterGameObject;

    public MonsterInfo(string name, GameObject gameObject)
    {
        monsterName = name;
        monsterGameObject = gameObject;
    }
}

