using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomInfo
{
    public int roomID;
    public Vector3 position;
    
    public RoomInfo(Vector3 pos, int id)
    {
        roomID = id;
        position = pos;
    }
}
