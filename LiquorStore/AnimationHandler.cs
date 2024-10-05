using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LiquorStore;

public class AnimationHandler : MonoBehaviour
{
  public Animation anim;
  public AnimClip[] queue;
  public float time;
  protected AnimClip lastClip;

  private void Start()
  {
    if (this.queue.Length == 0)
      return;
    for (int index = 0; index < this.queue.Length; ++index)
    {
      this.queue[index].isWaiting = false;
      this.queue[index].isPlaying = false;
    }
  }

  public void AddAnim(AnimClip clip)
  {
    List<AnimClip> list = ((IEnumerable<AnimClip>)this.queue).ToList<AnimClip>();
    list.Insert(list.Count - 1, clip);
    this.queue = list.ToArray();
  }

  private void Update()
  {
    if (!this.anim.isPlaying)
    {
      for (int index = 0; index < this.queue.Length; ++index)
      {
        if (!this.queue[index].isWaiting)
        {
          if (this.queue.Length < 2)
            this.StartCoroutine(this.PlayClip(this.queue[index]));
          else if (this.queue.Length > 1 && !this.queue[index].keepInQueue)
            this.StartCoroutine(this.PlayClip(this.queue[index]));
        }
      }
    }
    else
    {
      if (this.lastClip is null)
        return;
      this.time = this.anim[this.lastClip.animName].time;
    }
  }

  private IEnumerator PlayClip(AnimClip clip)
  {
    AnimationHandler animationHandler = this;
    clip.isPlaying = true;
    animationHandler.lastClip = clip;
    animationHandler.anim.Play(clip.animName);
    if (clip.keepInQueue)
    {
      clip.isWaiting = true;
      animationHandler.StartCoroutine(animationHandler.Wait(clip));
    }
    else
    {
      List<AnimClip> list = ((IEnumerable<AnimClip>)animationHandler.queue).ToList<AnimClip>();
      list.Remove(clip);
      animationHandler.queue = list.ToArray();
    }

    while (animationHandler.anim.isPlaying)
    {
      if (clip.finishEvent != null && (double)clip.finishEventTriggerTime != 0.0 &&
          (double)animationHandler.anim[clip.animName].time >= (double)clip.finishEventTriggerTime)
      {
        if (!clip.eventTriggered)
          clip.finishEvent();
        clip.eventTriggered = true;
      }

      yield return (object)null;
    }

    if (clip.finishEvent != null && (double)clip.finishEventTriggerTime == 0.0)
      clip.finishEvent();
    clip.eventTriggered = false;
    clip.isPlaying = false;
    yield return (object)null;
  }

  private IEnumerator Wait(AnimClip clip)
  {
    yield return (object)new WaitForSeconds(clip.randomWaitTime
      ? Random.Range(clip.minWait, clip.maxWait)
      : clip.waitTime);
    clip.isWaiting = false;
    yield return (object)null;
  }
};
