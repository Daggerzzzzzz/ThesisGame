using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomInfo
{
    public int RoomID { get; set; }
    public int dangerLevel { get; set; }
    public Vector3 Position { get; set; }
    public List<string[]> MonsterBatch { get; set; }
    
    public RoomInfo(Vector3 pos, int id)
    {
        RoomID = id;
        Position = pos;
    }
}
