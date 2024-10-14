using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitBrick
{
    public float initTime;

    protected LoadBricks loadBricks;
    protected int initTrack;
    protected float shouldTime;
    protected string poolTag;
    protected GameObject prefab;
    protected float speed;
    protected float accelerate;
    public virtual void BrickInit()
    {
        GameObject brick = GameObjectPool.Instance.CreateObject(poolTag, prefab, loadBricks.potStage.transform.position, Quaternion.identity);
        MusicBreak musicBreak = brick.GetComponent<MusicBreak>();
        musicBreak.accelerate = accelerate;
        musicBreak.targetPos = loadBricks.potStage.trackTarget[initTrack - 1];
        musicBreak.shouldTime = shouldTime;
        musicBreak.speed = speed;
        loadBricks.bricksOnTrack[initTrack].Add(musicBreak.transform);
    }
}
public class SingleInitBrick : InitBrick
{
    public SingleInitBrick(LoadBricks LoadBricks, int Track, float ShouldTime)
    {
        loadBricks = LoadBricks;
        initTrack = Track;
        shouldTime = ShouldTime;
        poolTag = "singlebrick";
        prefab = loadBricks.singleBrickPrefab;
        speed = loadBricks.singleSpeed;
        accelerate = loadBricks.singleAccelerate;
        initTime = shouldTime - (Mathf.Sqrt(speed * speed + 2 * accelerate * loadBricks.potStage.radius) - speed) / accelerate;
        if (initTime < 0)
        {
            Debug.LogError("检测时间过短，音乐块无法在检测时间内抵达判定边界");
        }
    }
}
public class FlickInitBrick : InitBrick
{
    public FlickInitBrick( LoadBricks LoadBricks, int Track, float ShouldTime)
    {
        loadBricks = LoadBricks;
        initTrack = Track;
        shouldTime = ShouldTime;
        poolTag = "flickbrick";
        prefab = loadBricks.flickBrickPrefab;
        speed = loadBricks.flickSpeed;
        accelerate = loadBricks.flickAccelerate;
        initTime = shouldTime - (Mathf.Sqrt(speed * speed + 2 * accelerate * loadBricks.potStage.radius) - speed) / accelerate;
        if (initTime < 0)
        {
            Debug.LogError("检测时间过短，音乐块无法在检测时间内抵达判定边界");
        }
    }
}
public class TouchInitBrick : InitBrick
{
    private float walkTime;
    private float angleSpeed;
    public TouchInitBrick(LoadBricks LoadBricks, int Track, float ShouldTime, float WalkTime)
    {
        loadBricks = LoadBricks;
        initTrack = Track;
        shouldTime = ShouldTime;
        poolTag = "touchbrick";
        prefab = loadBricks.touchBrickPrefab;
        speed = loadBricks.touchSpeed;
        accelerate = loadBricks.touchAccelerate;
        walkTime = WalkTime;
        initTime = shouldTime-(Mathf.Sqrt(speed * speed + 2 * accelerate * loadBricks.potStage.radius) - speed) / accelerate;
        angleSpeed = loadBricks.angleSpeed;
        if (initTime < 0)
        {
            Debug.LogError("检测时间过短，音乐块无法在检测时间内抵达判定边界");
        }

    }
    public override void BrickInit()
    {
        GameObject brick = GameObjectPool.Instance.CreateObject(poolTag, prefab, loadBricks.potStage.transform.position, Quaternion.identity);
        TouchMusicBreak musicBreak = brick.GetComponent<TouchMusicBreak>();
        musicBreak.accelerate = accelerate;
        musicBreak.targetPos = loadBricks.potStage.trackTarget[initTrack - 1];
        musicBreak.shouldTime = shouldTime;
        musicBreak.speed = speed;
        loadBricks.bricksOnTrack[initTrack].Add(musicBreak.transform);
        musicBreak.WalkTime = walkTime;
        musicBreak.center = loadBricks.center;
        musicBreak.radius = loadBricks.potStage.radius;
        musicBreak.angleSpeed = angleSpeed;
    }
}

