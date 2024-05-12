using System.Collections.Generic;
using UnityEngine;

public class RoomInfo
{
    public int RoomID { get; set; }
    public int DangerLevel { get; set; }
    public float MonsterBatchDangerLevel { get; set; }
    public Vector3 Position { get; set; }
    public List<string[]> MonsterBatch { get; set; }
    public RoomCenter IndividualRoomCenter { get; set; }
    
    public RoomInfo(Vector3 pos, int id, RoomCenter roomCenter)
    {
        RoomID = id;
        Position = pos;
        IndividualRoomCenter = roomCenter;
    }
}
