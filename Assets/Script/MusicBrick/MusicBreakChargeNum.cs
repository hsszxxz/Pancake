using UnityEngine;
public class DetectMessage
{
    public float ShouldTime;
    public float ActuallyTime;
}
[CreateAssetMenu(menuName = "MusicBreakChargeNum")]
public class MusicBreakChargeNum :ScriptableObject
{
    [Tooltip("在多少帧误差之内是完美")]
    public int PerfectTimeOffset;
    [Tooltip("在多少帧误差之内是不错")]
    public int GreatTimeOffset;
    [Tooltip("在多少帧误差之内是一般")]
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
