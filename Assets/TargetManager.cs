using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public static TargetManager instance;
    public Target[] targets;

    void Awake()
    {
        instance = this;
    }

    public void ResetAllTargets()
    {
        foreach (Target t in targets)
        {
            t.ResetTarget();

        }
    }
}