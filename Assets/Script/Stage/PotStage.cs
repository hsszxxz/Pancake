using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PotStage : MonoBehaviour
{
    public float radius;
    private LineRenderer lineRenderer;
    [HideInInspector]
    public Vector2[] trackTarget;
    private void Awake()
    {
        InitTrack();
        lineRenderer = GetComponentInChildren<LineRenderer>();
        DrawDetectArea();
    }
    private void DrawDetectArea()
    {
        lineRenderer.positionCount = 100;
        float angle = 0f;
        float increment = 3.6f;
        for (int i = 0; i < 100; i++)
        {
            float x = Mathf.Sin(angle * Mathf.Deg2Rad) * radius + transform.position.x;
            float y = Mathf.Cos(angle * Mathf.Deg2Rad) * radius + transform.position.y;
            lineRenderer.SetPosition(i, new Vector2(x, y));
            angle += increment;
        }
    }
    private void InitTrack()
    {
        trackTarget = new Vector2[6];
        trackTarget[0] = new Vector2(1 + Mathf.Sqrt(2), 1).normalized * radius;
        trackTarget[1] = new Vector2(1 + Mathf.Sqrt(2), -1).normalized * radius;
        trackTarget[2] = new Vector2(1,-1-Mathf.Sqrt(2)).normalized*radius;
        trackTarget[3] = new Vector2(-1,-1-Mathf.Sqrt(2)).normalized * radius;
        trackTarget[4] = new Vector2(-1 - Mathf.Sqrt(2), -1).normalized * radius;
        trackTarget[5] = new Vector2(-1 - Mathf.Sqrt(2), 1).normalized * radius;
        trackTarget[3] = new Vector2(-1, 1 +Mathf.Sqrt(2)).normalized * radius;
    }
    public int TrackNum(Vector2 targetPos)
    {
        Vector2 center = transform.position;
        Vector2 dir = (targetPos-center).normalized;
        float x = dir.x;
        float y = dir.y;
        if (x<=0 && y<=0)
        {
            if (x > y)
                return 4;
            return 5;
        }
        else if (x>0 && y>0)
        {
            if (x >=y)
                return 1;
            return -1;
        }
        else if (x<=0 && y>0)
        {
            if (Mathf.Abs(x) >= y)
                return 6;
            return -1;
        }
        else if (x >0 && y <=0)
        {
            if (x > Mathf.Abs(y))
                return 2;
            return 3;
        }
        return -1;
    }
}
