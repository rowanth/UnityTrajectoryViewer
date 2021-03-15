using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GenerateTrajectories : MonoBehaviour
{
    //Public Vars
    public TextAsset trajFile;
    public Slider timeline;

    //Private Vars
    private List<Pedestrian> pedestrians = new List<Pedestrian>();
    public List<Pedestrian> Pedestrians { get { return pedestrians; } }
    private float minTime;
    public float MinTime { get{ return minTime; }}
    private float maxTime;
    public float MaxTime { get{ return maxTime; }}

    struct TrajRecord
    {
        public int id;
        public float timestamp;
        public Vector3 position;
    };

    void Awake()
    {
        ParseFile();

        timeline.minValue = minTime;
        timeline.maxValue = maxTime;
    }

    void ParseFile()
    {
        List<TrajRecord> records = new List<TrajRecord>();

        string text = trajFile.text;
        string[] lines = Regex.Split(text, "\n|\r|\r\n");

        for (int i = 0; i < lines.Length; i++)
        {
            string valueLine = lines[i];
            string[] values = Regex.Split(valueLine, "\t");

            if (values.Length == 4)
            {
                TrajRecord record = new TrajRecord();
                record.timestamp = float.Parse(values[0]);
                record.id = (int)float.Parse(values[1]);
                record.position = new Vector3(
                    float.Parse(values[2]), 
                    float.Parse(values[3]), 0.0f);
                records.Add(record);
            }
        }

        //Split into individual pedestrians
        var recordsById = records.GroupBy(r => r.id);
        foreach (var pedestrianRecord in recordsById)
        {
            TrajRecord[] pedestrianRecordArray = pedestrianRecord.ToArray();
            Pedestrian p = new Pedestrian(pedestrianRecordArray[0].id);
            for (int i = 0; i < pedestrianRecordArray.Length; i++)
            {
                p._timeStamps.Add(pedestrianRecordArray[i].timestamp);
                p._trajectory.Add(pedestrianRecordArray[i].position);
            }
            pedestrians.Add(p);
        }

        //Get the min and max times for timeline
        minTime = records[0].timestamp;
        maxTime = records[records.Count - 1].timestamp;

        //float xmin = 0;
        //float xmax = 0;
        //float ymin = 0;
        //float ymax = 0;
        //for (int i = 0; i < pedestrians.Count; i++)
        //{
        //    for(int j = 0; j < pedestrians[i]._trajectory.Count; j++)
        //    {
        //        if (pedestrians[i]._trajectory[j].x > xmax)
        //            xmax = pedestrians[i]._trajectory[j].x;
        //        else if(pedestrians[i]._trajectory[j].x < xmin)
        //            xmin = pedestrians[i]._trajectory[j].x;

        //        if (pedestrians[i]._trajectory[j].y > ymax)
        //            ymax = pedestrians[i]._trajectory[j].y;
        //        else if (pedestrians[i]._trajectory[j].y < ymin)
        //            ymin = pedestrians[i]._trajectory[j].y;
        //    }
        //}
    }
}