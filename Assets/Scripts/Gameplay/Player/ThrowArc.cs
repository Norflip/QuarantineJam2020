using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowArc : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public int segments = 16;

    bool m_show;

    Vector3 startPosition;
    Vector3 startVelocity;
    Vector3 gravitation;
    float mass;

    List<Vector3> points = new List<Vector3>();

    private void Awake()
    {
        Show(false);
    }

    public void Show (bool show)
    {
        this.m_show = show;
        lineRenderer.enabled = show;
    }

    public void SetParams(Vector3 startPosition, Vector3 startVelocity, float mass, Vector3 gravitation)
    {
        this.startPosition = startPosition;
        this.startVelocity = startVelocity;
        this.mass = mass;
        this.gravitation = gravitation;
        Show(true);
    }

    private void Update()
    {
        if (m_show)
        {
            UpdatePoints();
        }
    }

    void UpdatePoints()
    {
        points.Clear();
        points.Add(startPosition);

        Vector3 position = startPosition;
        Vector3 velocity = startVelocity / this.mass;

        for (int i = 1; i < segments; i++)
        {
            float segTime = (velocity.sqrMagnitude != 0.0f) ? 1.0f / velocity.magnitude : 0.0f;   // Time.fixedDeltaTime;

            velocity += Physics.gravity * segTime;
            position += velocity * segTime;
            points.Add(position);
        }

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }
}
