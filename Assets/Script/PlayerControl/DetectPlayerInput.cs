using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
public class DetectPlayerInput : MonoSingleton<DetectPlayerInput>
{
    [HideInInspector]
    public List<Vector2> singleClick = new List<Vector2>();
    [HideInInspector]
    public List<Vector2> touch  = new List<Vector2>();
    [HideInInspector]
    public List<Vector2> flickDir = new List<Vector2>();
    [HideInInspector]
    public List<Vector2>lastTouch = new List<Vector2>();


    public Transform circleCenter;
    public float distance = 1.0f;

    public LineRenderer lineRenderer;
    private float currentTime=0;
    private void Start()
    {
        DrawDetectArea();
    }
    private void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime> 0.5f)
        {
            GameObjectPool.Instance.Clear("Line");
            currentTime = 0;
        }
        StoreInput();
        TypeDreawLine(singleClick, Color.white);
        TypeDreawLine(touch, Color.yellow);
        FlickDetect();
    }
    private void FlickDetect()
    {
        for (int i = 0; i < flickDir.Count; ++i)
        {
            LineRenderer line = GameObjectPool.Instance.CreateObject("Line", Resources.Load("Prefabs/Line") as GameObject, Vector3.zero, Quaternion.identity).GetComponent<LineRenderer>();
            line.startColor = Color.green;
            line.endColor = Color.green;
            line.SetPosition(0, touch[i]);
            line.SetPosition(1, touch[i] + flickDir[i]*2f);
        }
    }
    public GameObject IsHitBreak(Vector2 touchWorldPosition)
    {
        RaycastHit2D hit = Physics2D.Raycast(touchWorldPosition, Vector2.zero);
        if (hit.collider !=null &&hit.collider.CompareTag("Brick"))
        {
            return hit.collider.gameObject;
        }
        return null;
    }
    private void TypeDreawLine(List<Vector2> target,Color color)
    {
        for (int i = 0; i < target.Count; ++i)
        {
            LineRenderer line = GameObjectPool.Instance.CreateObject("Line", Resources.Load("Prefabs/Line") as GameObject, Vector3.zero, Quaternion.identity).GetComponent<LineRenderer>();
            line.startColor = color;
            line.endColor = color;
            line.SetPosition(0, circleCenter.position);
            line.SetPosition(1, target[i]);
            GameObject brick = IsHitBreak(target[i]);
            if (brick != null && brick.GetComponent<MusicBreak>().brickType != BrickType.Flick)
            {
                Debug.Log(brick.GetComponent<MusicBreak>().PlayerPerformance(circleCenter.position, distance));
                GameObjectPool.Instance.CollectObject(brick);
            }
        }
    }

    private void DrawDetectArea()
    {
        lineRenderer.positionCount = 100;
        float angle = 0f;
        float increment = 3.6f;
        for (int i = 0; i < 100; i++)
        {
            float x = Mathf.Sin(angle*Mathf.Deg2Rad)*distance + circleCenter.position.x;
            float y = Mathf.Cos(angle*Mathf.Deg2Rad)*distance+circleCenter.position.y;
            lineRenderer.SetPosition(i,new Vector2(x,y));
            angle += increment;
        }
    }

    private void StoreInput()
    {
        lastTouch.Clear();
        foreach (Vector2 pos in touch)
        {
            lastTouch.Add(pos);
        }
        singleClick.Clear();
        touch.Clear();
        flickDir.Clear();
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch finger = Input.GetTouch(i);
            Vector2 pos = Camera.main.ScreenToWorldPoint(finger.position);
            if (finger.phase == TouchPhase.Began)
            {
                singleClick.Add(pos);
                
                Debug.Log("began");
            }
            if (finger.phase == TouchPhase.Moved)
            {
                touch.Add(pos);
                if (lastTouch.Count > i)
                    flickDir.Add((pos - lastTouch[i]).normalized);
            }
        }
    }
}
