using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
class ProceduralCircle : MonoBehaviour
{
    public int vertexCount = 40;
    public float lineWidth = 0.2f;
    public float radius;
    public bool circleFillscreen;
    
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        setupCircle();
    }

    private void setupCircle()
    {
        lineRenderer.widthMultiplier = lineWidth;
        if (circleFillscreen)
        {
            radius = Vector3.Distance(Camera.main.ScreenToWorldPoint(new Vector3(0f, Camera.main.pixelRect.yMax, 0f)),
                Camera.main.ScreenToWorldPoint(new Vector3(0f, Camera.main.pixelRect.yMin, 0f))) * 0.5f - lineWidth;
        }

        float deltaTheta = (2f * Mathf.PI) / vertexCount;
        float theta = 0f;

        lineRenderer.positionCount = vertexCount;
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            Vector3 pos = new Vector3(
                radius * Mathf.Cos(theta),
                radius * Mathf.Sin(theta),
                0f);
            lineRenderer.SetPosition(i, pos + transform.position);
            theta += deltaTheta;
        }
    }
}
