using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadBricks : MonoBehaviour
{
    public float speed;
    public float accelerate;
    public Transform[] musicTrackStartPoint;
    public GameObject brickPrefab;
    [Tooltip("单击类型音乐块的判定时间")]
    public List<float> singleClickBricksTime;
    [Tooltip("单击类型音乐块的运动轨道（与上面对应）")]
    public List<int> singleClickBricksTrack;
    private void Start()
    {
        CreateBrick();
    }
    private void CreateBrick()
    {
        for (int i = 0; i < singleClickBricksTime.Count; i++)
        {
            GameObject brick = GameObjectPool.Instance.CreateObject("brick", brickPrefab, musicTrackStartPoint[singleClickBricksTrack[i]].position, Quaternion.identity);
            MusicBreak musicBreak = brick.GetComponent<MusicBreak>();
            musicBreak.accelerate = accelerate;
            musicBreak.targetPos = Vector3.zero;
            musicBreak.brickType = BrickType.SingleClick;
            musicBreak.speed = speed;
        }
    }
}
