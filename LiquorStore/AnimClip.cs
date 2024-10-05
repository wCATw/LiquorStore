using UnityEngine;

namespace LiquorStore;

public class AnimClip : ScriptableObject
{
    public string animName;
    public bool keepInQueue;
    public bool randomWaitTime;
    public float minWait;
    public float maxWait = 1f;
    public float waitTime;
    public bool isWaiting;
    public float finishEventTriggerTime;
    internal bool eventTriggered;
    public bool isPlaying;
    public Action finishEvent;
}