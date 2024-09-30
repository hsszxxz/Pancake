using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
public enum Performance
{
    Bad,
    Normal,
    Great,
    Perfect
}
public enum BrickType
{
    SingleClick,
    Touch,
    Flick
}
public class MusicBreak : MonoBehaviour,IResetable
{
    [HideInInspector]
    public BrickType brickType;
    private float speedValue=0;
    [HideInInspector]
    public float speed
    {
        get
        {
            return speedValue;
        }
        set
        {
            speedValue = value;
            velocity = value * (targetPos - new Vector2(transform.position.x, transform.position.y)).normalized;
        }
    }
    [HideInInspector]
    public float accelerate;
    [HideInInspector]
    public Vector2 targetPos;
    private Vector2 velocity;
    private void Update()
    {
        if (speed != 0)
        {
            BrickMotor();
        }
        DeletGameObject();
    }
    private void BrickMotor()
    {
        Vector2 direction = (targetPos - new Vector2(transform.position.x,transform.position.y)).normalized;
        velocity += direction * accelerate * Time.deltaTime;
        transform.position += (Vector3)velocity * Time.deltaTime;
    }
    private void DeletGameObject()
    {
        if (Vector2.Distance(transform.position, targetPos) <0.1f)
            GameObjectPool.Instance.CollectObject(gameObject);
    }
    public bool IsSelfHit(Vector2 touchWorldPosition)
    {
        RaycastHit2D hit = Physics2D.Raycast(touchWorldPosition,Vector2.zero);
        if (hit.collider.gameObject == gameObject)
        {
            return true;
        }
        return false;
    }
    public Performance PlayerPerformance(Vector2 target,float distanceOffset)
    {
        float dis =Vector2.Distance(target,transform.position)- distanceOffset;
        if (dis<=0.2f)
        {
            return Performance.Perfect;
        }
        else if (dis<=0.5f)
        {
            return Performance.Great;
        }
        else if (dis <= 0.8f)
        {
            return Performance.Normal;
        }
        else
        {
            return Performance.Bad;
        }
    }

    public void OnReset()
    {
        speed = 0;
        accelerate = 0;
        velocity = Vector2.zero;
    }
}
