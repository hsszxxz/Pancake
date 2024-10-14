using UnityEngine;
public class DetectMessage
{
    public float ShouldTime;
    public float ActuallyTime;
}
[CreateAssetMenu(menuName = "MusicBreakChargeNum")]
public class MusicBreakChargeNum :ScriptableObject
{
    [Tooltip("�ڶ��������֮��������")]
    public float PerfectTimeOffset;
    [Tooltip("�ڶ��������֮���ǲ���")]
    public float GreatTimeOffset;
    [Tooltip("�ڶ��������֮����һ��")]
    public float NormalTimeOffset;
    public Performance PerformanceCharge(DetectMessage message)
    {

        float offset = Mathf.Abs(message.ActuallyTime - message.ShouldTime);
        if (offset <= PerfectTimeOffset)
        {
            return Performance.Perfect;
        }
        else if (offset <= GreatTimeOffset)
        {
            return Performance.Great;
        }
        else if (offset <= NormalTimeOffset)
        {
            return Performance.Normal;
        }
        else
        {
            return Performance.Bad;
        }
    }
}
