using HutongGames.PlayMaker;
using System.Collections;
using UnityEngine;

namespace LiquorStore;

public class Door : MonoBehaviour
{
  public OpeningHours openingHours;
  public Animation animation;
  public Collider handle;
  public InteractionRaycast raycast;
  public bool open;
  public bool locked;
  private bool allowClick = true;
  private FsmBool use;

  private void Start() => this.use = FsmVariables.GlobalVariables.FindFsmBool("GUIuse");

  private void Update()
  {
    this.locked = this.openingHours.closed;
    if (this.raycast.GetHit(this.handle))
    {
      this.use.Value = true;
      if (Input.GetMouseButtonDown(0) && this.allowClick)
        this.DoorFunction();
    }
    if (!this.locked || !this.open)
      return;
    this.DoorFunction(false);
  }

  private void DoorFunction(bool userInput = true)
  {
    this.use.Value = false;
    if (this.locked & userInput)
      MasterAudio.PlaySound3DAndForget("Store", ((Component) this).transform, false, 1f, new float?(), 0.0f, "door_locked");
    else if (!this.locked & userInput)
    {
      Transform transform = ((Component) this).transform;
      string str1 = !this.open ? "door_open" : "door_close";
      float? nullable = new float?();
      string str2 = str1;
      MasterAudio.PlaySound3DAndForget("Store", transform, false, 1f, nullable, 0.0f, str2);
    }
    else if (this.locked && this.open)
      MasterAudio.PlaySound3DAndForget("Store", ((Component) this).transform, false, 1f, new float?(), 0.0f, "door_close");
    if (!this.locked)
      this.animation.Play(!this.open ? "Open" : "Close");
    else if (this.locked && this.open)
      this.animation.Play("Close");
    this.open = !this.open;
    if (this.locked)
      this.open = false;
    this.allowClick = false;
    this.StartCoroutine(this.Wait());
  }

  private IEnumerator Wait()
  {
    yield return (object) new WaitForSeconds(0.5f);
    this.allowClick = true;
    yield return (object) null;
  }
}
