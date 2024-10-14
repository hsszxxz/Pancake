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
    public float shouldTime;

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

    void Update()
    {
        if (speed != 0)
        {
            BrickMotor();
        }
        DeletGameObject();
    }
    protected void BrickMotor()
    {
        Vector2 direction = (targetPos - new Vector2(transform.position.x,transform.position.y)).normalized;
        velocity += direction * accelerate * Time.deltaTime;
        transform.position += (Vector3)velocity * Time.deltaTime;
    }
    protected virtual void DeletGameObject()
    {
        if (Vector2.Distance(transform.position, targetPos) <0.03f)
            GameObjectPool.Instance.CollectObject(gameObject);
    }
    public virtual void OnReset()
    {
        speed = 0;
        accelerate = 0;
        velocity = Vector2.zero;
        targetPos = Vector2.zero;
    }
}
