using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestrian 
{
    public int _id;
    public List<float> _timeStamps;
    public List<Vector3> _trajectory;

    public Pedestrian(int id)
    {
        _id = id;
        _timeStamps = new List<float>();
        _trajectory = new List<Vector3>();
    }
}
