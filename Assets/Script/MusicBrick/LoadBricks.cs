using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LoadBricks : MonoBehaviour
{
    public Transform center;
    [HideInInspector]
    public PotStage potStage;

    public float singleSpeed;
    public float singleAccelerate;
    public GameObject singleBrickPrefab;
    [Tooltip("单击类型音乐块的判定时间")]
    public List<float> singleClickBricksTime;
    [Tooltip("单击类型音乐块的运动轨道")]
    public List<int> singleClickBricksTrack;

    public float flickSpeed;
    public float flickAccelerate;
    public GameObject flickBrickPrefab;
    [Tooltip("Flick类型音乐块的判定时间")]
    public List<float> flickBricksTime;
    [Tooltip("Flick类型音乐块的运动轨道")]
    public List<int> flickBricksTrack;

    public float touchSpeed;
    public float touchAccelerate;
    public float angleSpeed;
    public GameObject touchBrickPrefab;
    [Tooltip("Touch类型音乐块的判定时间")]
    public List<float> touchBricksTime;
    [Tooltip("Touch类型音乐块的运动轨道")]
    public List<int> touchBricksTrack;
    [Tooltip("Touch类型音乐块在轨道上移动的角度")]
    public List<float> touchBricksWalkTime;

    [HideInInspector]
    public Dictionary<int,List<Transform>> bricksOnTrack = new Dictionary<int,List<Transform>>();

    private List<InitBrick> brickInit = new List<InitBrick>();
    private int index = 0;
    private float currentTime=0;
    private void Start()
    {
        potStage = center.GetComponent<PotStage>();
        for (int i = 0; i < 6; i++)
        {
            bricksOnTrack.Add(i+1, new List<Transform>());
        }
        CreateBrickInit();
    }
    private void Update()
    {
        currentTime += Time.deltaTime;
        GenerateBrick();
    }
    private void GenerateBrick()
    {
        if (index<brickInit.Count &&currentTime > brickInit[index].initTime)
        {
            brickInit[index].BrickInit();
            index++;
        }
    }
    public Transform GetFormestTransform(int trackNum)
    {
        if (trackNum==-1 ||bricksOnTrack[trackNum].Count <= 0)
            return null;
        Transform result = null;
        foreach(Transform t in bricksOnTrack[trackNum])
        {
            if (result == null || Vector2.Distance(t.position,center.position)> Vector2.Distance(result.position, center.position))
                result = t;
        }
        return result;
    }
    public void BricksOnTrackRemove(Transform remove)
    {
        foreach (List<Transform> list in bricksOnTrack.Values)
        {
            if (list.Contains(remove))
                list.Remove(remove);
        }
    }
    private void CreateBrickInit()
    {
        for (int i = 0; i < singleClickBricksTime.Count; i++)
        {
            SingleInitBrick single = new SingleInitBrick(this, singleClickBricksTrack[i], singleClickBricksTime[i]);
            brickInit.Add(single);

        }
        for (int i = 0; i < flickBricksTime.Count; i++)
        {
            FlickInitBrick flick = new FlickInitBrick(this, flickBricksTrack[i], flickBricksTime[i]);
            brickInit.Add(flick);
        }
        for (int i = 0; i < touchBricksTime.Count; i++)
        {
            TouchInitBrick touch = new TouchInitBrick(this, touchBricksTrack[i], touchBricksTime[i], touchBricksWalkTime[i]);
            brickInit.Add(touch);
        }
        SortInitBricks();
    }
    private void SortInitBricks()
    {
        for (int i = 0;i < brickInit.Count;i++)
            for (int j = 0;j<brickInit.Count-1-i;j++)
                if (brickInit[j].initTime > brickInit[j + 1].initTime)
                {
                    InitBrick temp = brickInit[j];
                    brickInit[j] = brickInit[j+1];
                    brickInit[j+1] = temp;
                }
    }
}
