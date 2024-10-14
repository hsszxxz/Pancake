using UnityEngine;
public class DetectMessage
{
    public float ShouldTime;
    public float ActuallyTime;
}
[CreateAssetMenu(menuName = "MusicBreakChargeNum")]
public class MusicBreakChargeNum :ScriptableObject
{
    [Tooltip("在多少秒误差之内是完美")]
    public float PerfectTimeOffset;
    [Tooltip("在多少秒误差之内是不错")]
    public float GreatTimeOffset;
    [Tooltip("在多少秒误差之内是一般")]
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
