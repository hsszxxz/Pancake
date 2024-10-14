using UnityEngine;
public class DetectMessage
{
    public float ShouldTime;
    public float ActuallyTime;
}
[CreateAssetMenu(menuName = "MusicBreakChargeNum")]
public class MusicBreakChargeNum :ScriptableObject
{
    [Tooltip("�ڶ���֡���֮��������")]
    public int PerfectTimeOffset;
    [Tooltip("�ڶ���֡���֮���ǲ���")]
    public int GreatTimeOffset;
    [Tooltip("�ڶ���֡���֮����һ��")]
    public int NormalTimeOffset;
    public Performance PerformanceCharge(DetectMessage message)
    {

        float offset = Mathf.Abs(message.ActuallyTime - message.ShouldTime);
        int zhen = (int)(offset / 0.02f);
        if (zhen <= PerfectTimeOffset)
        {
            return Performance.Perfect;
        }
        else if (zhen <= GreatTimeOffset)
        {
            return Performance.Great;
        }
        else if (zhen <= NormalTimeOffset)
        {
            return Performance.Normal;
        }
        else
        {
            return Performance.Bad;
        }
    }
}
