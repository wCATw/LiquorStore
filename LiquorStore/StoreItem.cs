using HutongGames.PlayMaker;
using System.Collections;
using UnityEngine;

namespace LiquorStore;

public class StoreItem : MonoBehaviour
{
  [Header("Variables")]
  public InteractionRaycast raycast;
  public CashRegisterBehaviour cashRegister;
  public Vector3 spawnRot;
  public Collider trigger;
  public bool goesInShoppingBag;
  public GameObject prefab;
  public int count;
  public int bought;
  public GameObject[] visuals;
  public float price;
  public string articleName;
  public string brand;
  private FsmBool guiBuy;
  private FsmString guiInteraction;
  [Space(20f)]
  private bool mouseOver;
  [Header("Defaults")]
  public int _count;

  private void Start()
  {
    this.guiBuy = PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIbuy");
    this.guiInteraction = PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction");
  }

  public void Reset()
  {
    this.count = this._count;
    if (this.visuals.Length == 0)
      return;
    for (int index = 0; index < this.visuals.Length; ++index)
      this.visuals[index].SetActive(true);
  }

  private void Update() => this.ShoppingHandler();

  public void ShoppingHandler()
  {
    if (this.raycast.GetHit(this.trigger))
    {
      this.mouseOver = true;
      this.guiBuy.Value = true;
      this.guiInteraction.Value = string.Format("{0} {1} MK", (object) this.articleName.ToUpper(), (object) this.price);
      if (this.count > 0 && Input.GetMouseButtonDown(0))
      {
        this.cashRegister.AddPrice(this.price);
        ++this.bought;
        --this.count;
        for (int index = 0; index < this.visuals.Length; ++index)
          this.visuals[index].SetActive(index < this.count);
      }
      else
      {
        if (this.bought <= 0 || !Input.GetMouseButtonDown(1))
          return;
        this.cashRegister.AddPrice(-this.price);
        --this.bought;
        ++this.count;
        for (int index = 0; index < this.visuals.Length; ++index)
          this.visuals[index].SetActive(index < this.count);
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

  public IEnumerator CreateItems(Vector3 spawnPos, InteractionRaycast raycast)
  {
    for (int i = 0; i < this.bought; ++i)
    {
      GameObject gameObject = Object.Instantiate<GameObject>(this.prefab);
      gameObject.transform.eulerAngles = this.spawnRot;
      gameObject.transform.position = spawnPos;
      gameObject.SetActive(true);
      gameObject.GetComponent<CustomLiquorCase>().raycast = raycast;
      yield return (object) new WaitForSeconds(0.1f);
    }
    this.bought = 0;
    yield return (object) null;
  }
}