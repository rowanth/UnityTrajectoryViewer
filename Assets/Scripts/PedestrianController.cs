using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PedestrianController : MonoBehaviour
{
    public Slider timeline;
    public Text currentTime;

    public GenerateTrajectories trajectories;
    public GameObject pedestrianProxy;
    public Material trajectoryMaterial;

    private GameObject[] pedTrajectories;
    private GameObject[] pedProxies;

    private Color[] colors = { Color.red, Color.green, Color.blue, Color.cyan, Color.yellow, Color.magenta, Color.white, Color.black, Color.gray};
    
    void Start()
    {
        pedTrajectories = new GameObject[trajectories.Pedestrians.Count];
        pedProxies = new GameObject[trajectories.Pedestrians.Count];
        initPedestrians();
    }

    public void initPedestrians()
    {
        float currentSliderTime = timeline.value;
        for (int n = 0; n < trajectories.Pedestrians.Count - 1; n++)
        {
            Color col = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f); 

            //Create The Trajectory GO
            pedTrajectories[n] = new GameObject();
            pedTrajectories[n].name = "TrajectoryLine_" + n;
            pedTrajectories[n].AddComponent<LineRenderer>();
            LineRenderer trajectoryRenderer = pedTrajectories[n].GetComponent<LineRenderer>();
            trajectoryRenderer.material = trajectoryMaterial;
            trajectoryRenderer.material.color = col;
            trajectoryRenderer.startWidth = 0.05f;

            for (int i = 0; i < trajectories.Pedestrians[n]._trajectory.Count - 1; i++)
            {
                trajectoryRenderer.positionCount = trajectories.Pedestrians[n]._trajectory.Count;
                trajectoryRenderer.SetPositions(
                    trajectories.Pedestrians[n]._trajectory.ToArray());
            }
            pedTrajectories[n].gameObject.SetActive(false);

            //Create The Proxy GO
            pedProxies[n] = GameObject.Instantiate(
            pedestrianProxy,
            trajectories.Pedestrians[n]._trajectory[0],
            Quaternion.identity);
            pedProxies[n].GetComponent<SpriteRenderer>().color = col;
            pedProxies[n].gameObject.SetActive(false);
        }
        updatePedestrians();
    }

    public void updatePedestrians()
    {
        float currentSliderTime = timeline.value;
        currentTime.text = timeline.value.ToString();
        for (int n = 0; n < trajectories.Pedestrians.Count - 1; n++)
        {
            if (currentSliderTime < trajectories.Pedestrians[n]._timeStamps[trajectories.Pedestrians[n]._timeStamps.Count - 1]
            && currentSliderTime > trajectories.Pedestrians[n]._timeStamps[0])
            {
                pedProxies[n].gameObject.SetActive(true);
                pedTrajectories[n].gameObject.SetActive(true);
                int i = 0;
                while (currentSliderTime > trajectories.Pedestrians[n]._timeStamps[i])
                    i++;

                float endTime = trajectories.Pedestrians[n]._timeStamps[i];
                float startTime = trajectories.Pedestrians[n]._timeStamps[i - 1];
                Vector3 currPos = Vector3.Lerp(
                    trajectories.Pedestrians[n]._trajectory[i - 1],
                    trajectories.Pedestrians[n]._trajectory[i],
                    (currentSliderTime - startTime) / (endTime - startTime));

                pedProxies[n].transform.position = currPos;
            }
            else
            {
                pedProxies[n].gameObject.SetActive(false);
                pedTrajectories[n].gameObject.SetActive(false);
            }
        }
    }
}