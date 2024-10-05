using HutongGames.PlayMaker;
using UnityEngine;
using UnityEngine.Events;

namespace LiquorStore;

public class CashRegisterBehaviour : MonoBehaviour
{
  public StoreInventory storeInventory;
  public InteractionRaycast raycast;
  private PlayMakerFSM speakDB;
  public TextMesh display;
  public float total;
  public float priceTotal;
  public AnimationHandler animHandler;
  public Collider trigger;
  public AnimClip cashRegisterAnim;
  private FsmBool guiBuy;
  private FsmString guiInteraction;
  private FsmFloat playerMoney;
  private bool clipAdded;
  private bool mouseOver;
  private float noMoneyWait;
  public UnityEvent purchaseEvent;

  private void Start()
  {
    this.guiBuy = PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIbuy");
    this.guiInteraction = PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction");
    this.playerMoney = FsmVariables.GlobalVariables.FindFsmFloat("PlayerMoney");
    this.speakDB = ((Component) GameObject.Find("PLAYER").transform.Find("Pivot/AnimPivot/Camera/FPSCamera/SpeakDatabase")).GetComponent<PlayMakerFSM>();
    this.cashRegisterAnim.finishEvent += new Action(this.AnimAction);
  }

  public void AddPrice(float amount) => this.priceTotal += amount;

  private void Update()
  {
    if ((double) this.noMoneyWait > 0.0)
      this.noMoneyWait -= Time.deltaTime;
    if (this.clipAdded && ((IEnumerable<AnimClip>) this.animHandler.queue).Where<AnimClip>((Func<AnimClip, bool>) (x => x.animName == this.cashRegisterAnim.animName)).Count<AnimClip>() <= 0)
      this.clipAdded = false;
    if ((double) this.priceTotal != (double) this.total && !this.clipAdded && !this.cashRegisterAnim.isPlaying)
    {
      this.animHandler.AddAnim(this.cashRegisterAnim);
      this.clipAdded = true;
    }
    if (this.raycast.GetHit(this.trigger) && (double) this.total > 0.0 && (double) this.total == (double) this.priceTotal)
    {
      this.mouseOver = true;
      this.guiBuy.Value = true;
      if ((double) this.noMoneyWait <= 0.0)
        this.guiInteraction.Value = "PRICE TOTAL: " + Mathf.Clamp(this.total, 0.0f, 1000000f).ToString("0.00") + " MK";
      if (!Input.GetMouseButtonDown(0))
        return;
      if ((double) this.playerMoney.Value >= (double) this.priceTotal)
      {
        this.storeInventory.Purchase();
        this.playerMoney.Value -= this.priceTotal;
        this.purchaseEvent?.Invoke();
        MasterAudio.PlaySound3DAndForget("Store", ((Component) this).transform, false, 1f, new float?(1f), 0.0f, "cash_register_2");
        this.total = 0.0f;
        this.priceTotal = 0.0f;
        this.display.text = "0.00";
      }
      else
      {
        this.guiInteraction.Value = "NOT ENOUGH MONEY";
        this.noMoneyWait = 3f;
        this.speakDB.SendEvent("SWEARING");
      }
    }
    else
    {
      if (!this.mouseOver)
        return;
      this.mouseOver = false;
      this.guiBuy.Value = false;
      this.guiInteraction.Value = "";
    }
  }

  private void AnimAction()
  {
    this.total = this.priceTotal;
    MasterAudio.PlaySound3DAndForget("Store", ((Component) this).transform, false, 1f, new float?(1f), 0.0f, "cash_register_1");
    this.display.text = Mathf.Clamp(this.total, 0.0f, 1000000f).ToString("0.00");
    this.clipAdded = false;
  }
}
