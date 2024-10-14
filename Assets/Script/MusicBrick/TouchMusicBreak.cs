using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchMusicBreak : MusicBreak
{
    [HideInInspector]
    public float WalkTime;
    [HideInInspector]
    public Transform center;
    [HideInInspector]
    public float radius;
    [HideInInspector]
    public float angleSpeed;

    private float angle;
    private float time=0;
    private bool isCircle;
    [HideInInspector]
    public bool isTouch=false;

    void Start()
    {
        brickType = BrickType.Touch;
    }

    private void CircleMotor()
    {
        if (time <= WalkTime)
        {
            time += Time.deltaTime;
            angle += angleSpeed * Time.deltaTime;
            float x = center.position.x + Mathf.Cos(angle) * radius;
            float y = center.position.y + Mathf.Sin(angle) * radius;
            transform.position = new Vector3(x, y, 0);
        }
        else
        {
            GameObjectPool.Instance.CollectObject(gameObject);
        }
    }
    public override void OnReset()
    {
        base.OnReset();
        WalkTime = 0;
        angle = 0;
        isCircle = false;
        isTouch = false;
        time = 0;
    }
    protected override void DeletGameObject()
    {
        if (Vector2.Distance(transform.position, targetPos) < 0.03f)
        {
            angle = Mathf.Atan2(transform.position.y - center.position.y, transform.position.x - center.position.x);
            isCircle = true;
        }
        if (isCircle )
        {
            CircleMotor();
        }
    }
}
