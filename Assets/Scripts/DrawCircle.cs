using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCircle : MonoBehaviour
{
    public Material lineRendererMaterial;
    public float radius;
    private LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        // Add radius
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = lineRendererMaterial;
        lineRenderer.startColor = Color.blue;
        lineRenderer.startWidth = 0.75f;
        lineRenderer.positionCount = 100;
    }
    void Update()
    {

        float theta_scale = 0.02f;  // Circle resolution
        float theta = 0f;
        for (int i = 0; i < 100; i++)
        {
            theta += (2.0f * Mathf.PI * theta_scale);
            float x = radius * Mathf.Cos(theta);
            float y = radius * Mathf.Sin(theta);
            x += gameObject.transform.position.x;
            y += gameObject.transform.position.y;
            var pos = new Vector3(x, y, 0);
            lineRenderer.SetPosition(i, pos);
        }
    }
}

