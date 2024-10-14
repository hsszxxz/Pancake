using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
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
    [HideInInspector]
    public List<Vector2> cancelTouch = new List<Vector2>();

    public Transform stage;
    private PotStage potStage;
    private LoadBricks loadBricks;
    private Dictionary<Transform,DetectMessage> releaseTouchBrick = new Dictionary<Transform, DetectMessage>();

    public MusicBreakChargeNum chargeNum;

    private float currentTime=0;
    private void Start()
    {
        potStage = stage.GetComponent<PotStage>();
        loadBricks = GetComponent<LoadBricks>();
    }
    private void Update()
    {
        currentTime += Time.deltaTime;
        StoreInput();
        //FlickDetect();
        DetectSingleClick();
        DetectTouch();
        FlickDetect();
    }
    private void FlickDetect()
    {
        for (int i = 0; i < lastTouch.Count; ++i)
        {
            Transform brick = loadBricks.GetFormestTransform(potStage.TrackNum(lastTouch[i]));
            if (brick == null)
                return;
            MusicBreak musicBreak = brick.GetComponent<MusicBreak>();
            if (musicBreak.brickType == BrickType.Flick)
            {
                DetectMessage message = new DetectMessage()
                {
                    ShouldTime = musicBreak.shouldTime,
                    ActuallyTime = currentTime
                };
                Performance performance = chargeNum.PerformanceCharge(message);
                Debug.Log(performance);
                loadBricks.BricksOnTrackRemove(brick);
                GameObjectPool.Instance.CollectObject(brick.gameObject);
            }
        }
    }
    public GameObject IsHitTouchBrick(Vector2 touchWorldPosition)
    {
        RaycastHit2D hit = Physics2D.Raycast(touchWorldPosition, Vector2.zero);
        if (hit.collider != null && hit.collider.CompareTag("TouchBrick"))
        {
            return hit.collider.gameObject;
        }
        return null;
    }
    private void DetectSingleClick()
    {
        for (int i = 0; i < singleClick.Count; ++i)
        {
            Transform brick = loadBricks.GetFormestTransform(potStage.TrackNum(singleClick[i]));
            if (brick == null)
                return;
            MusicBreak musicBreak = brick.GetComponent<MusicBreak>();
            if (musicBreak.brickType == BrickType.SingleClick)
            {
                DetectMessage message = new DetectMessage()
                {
                    ShouldTime = musicBreak.shouldTime,
                    ActuallyTime = currentTime
                };
                Performance performance = chargeNum.PerformanceCharge(message);
                Debug.Log(performance);
                loadBricks.BricksOnTrackRemove(brick);
                GameObjectPool.Instance.CollectObject(brick.gameObject);
            }
        }
    }
    private Transform NearestTouchBrick(float currentTime)
    {
        Transform target = null;
        float offSetTimeMin = 0;
        foreach (Transform t in releaseTouchBrick.Keys)
        {
            float offTime =Mathf.Abs(releaseTouchBrick[t].ShouldTime-currentTime);
            if (target == null||offTime<offSetTimeMin)
            {
                target = t;
                offSetTimeMin = offTime;
            }
        }
        if (offSetTimeMin>2f)
            return null;
        return target;
    }
    private void DetectTouch()
    {
        for (int i = 0; i < touch.Count; ++i)
        {
            Transform brick = loadBricks.GetFormestTransform(potStage.TrackNum(touch[i]));
            if (brick == null ||brick.tag != "TouchBrick")
                return;
            TouchMusicBreak musicBreak = brick.GetComponent<TouchMusicBreak>();
            if (musicBreak.isTouch==false)
            {
                DetectMessage message = new DetectMessage()
                {
                    ShouldTime = musicBreak.shouldTime,
                    ActuallyTime = currentTime
                };
                Performance performance = chargeNum.PerformanceCharge(message);
                musicBreak.isTouch = true;
                Debug.Log("1:"+performance);
                message.ShouldTime += musicBreak.WalkTime;
                loadBricks.BricksOnTrackRemove(brick);
                if (!releaseTouchBrick.ContainsKey(brick))
                    releaseTouchBrick.Add(brick, message);
            }
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
        cancelTouch.Clear();
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch finger = Input.GetTouch(i);
            Vector2 pos = Camera.main.ScreenToWorldPoint(finger.position);
            if (finger.phase == TouchPhase.Began)
            {
                singleClick.Add(pos);
            }
            if (finger.phase == TouchPhase.Ended)
            {
                Transform target = NearestTouchBrick(currentTime);
                if (target!=null)
                {
                    releaseTouchBrick[target].ActuallyTime = currentTime;
                    Performance performance = chargeNum.PerformanceCharge(releaseTouchBrick[target]);
                    Debug.Log("2:"+performance);
                    releaseTouchBrick.Remove(target);
                }
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
