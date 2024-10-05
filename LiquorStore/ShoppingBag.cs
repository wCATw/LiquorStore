using HutongGames.PlayMaker;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LiquorStore;

public class ShoppingBag : MonoBehaviour
{
  public InteractionRaycast raycast;
  public Collider trigger;
  public List<ShoppingBagItem> items = new List<ShoppingBagItem>();
  public Transform spawnPivot;
  private FsmBool guiUse;
  private FsmString guiInteraction;
  private bool mouseOver;
  private bool isHolding;
  private float time;
  private float wait;

  private void Start()
  {
    this.guiUse = FsmVariables.GlobalVariables.FindFsmBool("GUIuse");
    this.guiInteraction = FsmVariables.GlobalVariables.FindFsmString("GUIinteraction");
  }

  private void Update()
  {
    if ((double) this.wait > 0.0)
      this.wait -= Time.deltaTime;
    if (this.raycast.GetHit(this.trigger) && (double) this.wait <= 0.0)
    {
      this.guiUse.Value = true;
      this.mouseOver = true;
      if (cInput.GetButtonDown("Use"))
        this.isHolding = true;
      if (cInput.GetButton("Use") && this.isHolding)
      {
        this.guiInteraction.Value = "OPENING THE BAG...";
        this.time += Time.deltaTime;
      }
      if (cInput.GetButtonUp("Use"))
      {
        if ((double) this.time < 2.0)
          this.CreateItem(false);
        this.wait = 1f;
        this.isHolding = false;
        this.guiInteraction.Value = "";
      }
      if ((double) this.time <= 2.0)
        return;
      this.CreateItem(true);
      this.guiInteraction.Value = "";
    }
    else
    {
      if (!this.mouseOver)
        return;
      this.guiUse.Value = false;
      this.mouseOver = false;
    }
  }

  private void CreateItem(bool all)
  {
    if (!all)
    {
      MasterAudio.PlaySound3DAndForget("HouseFoley", ((Component) this).transform, false, 1f, new float?(1f), 0.0f, "plasticbag_open2");
      ((GameObject) Object.Instantiate((Object) SaveUtility.instance.storeInventory.storeItems.First<StoreItem>((Func<StoreItem, bool>) (x => x.brand == this.items[this.items.Count - 1].brand)).prefab, this.spawnPivot.position, this.spawnPivot.rotation)).GetComponent<DrinkBehaviour>().raycast = SaveUtility.instance.itemCast;
      --this.items[this.items.Count - 1].count;
      if (this.items[this.items.Count - 1].count <= 0)
        this.items.Remove(this.items[this.items.Count - 1]);
      if (this.items.Select<ShoppingBagItem, int>((Func<ShoppingBagItem, int>) (x => x.count)).Count<int>() > 0)
        return;
      Object.Destroy((Object) ((Component) this).gameObject);
    }
    else
    {
      MasterAudio.PlaySound3DAndForget("HouseFoley", ((Component) this).transform, false, 1f, new float?(1f), 0.0f, "plasticbag_open1");
      foreach (ShoppingBagItem shoppingBagItem in this.items)
      {
        ShoppingBagItem item = shoppingBagItem;
        if (item.count > 0)
        {
          for (int index = 0; index < item.count; ++index)
            ((GameObject) Object.Instantiate((Object) SaveUtility.instance.storeInventory.storeItems.First<StoreItem>((Func<StoreItem, bool>) (x => x.brand == item.brand)).prefab, this.spawnPivot.position, this.spawnPivot.rotation)).GetComponent<DrinkBehaviour>().raycast = SaveUtility.instance.itemCast;
          item.count = 0;
        }
      }
      Object.Destroy((Object) ((Component) this).gameObject);
    }
  }
}
