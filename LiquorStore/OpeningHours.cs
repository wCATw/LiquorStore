// Decompiled with JetBrains decompiler
// Type: LiquorStore.OpeningHours
// Assembly: LiquorStore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FC65CEA2-D49C-4FF1-A770-819DB08A2A29
// Assembly location: C:\Users\Kotovskiy\Documents\MySummerCar\Mods\LiquorStore.dll

using HutongGames.PlayMaker;
using UnityEngine;

#nullable disable
namespace LiquorStore
{
  public class OpeningHours : MonoBehaviour
  {
    public bool closed;
    public int OpeningHour = 8;
    public int ClosingHour = 20;
    public int restockDay = 1;
    public GameObject[] objects;
    private FsmInt gd;
    private FsmInt t;
    private Transform pl;
    public StoreInventory inventory;
    public Material windows;
    public static bool didRestock;

    private void Start()
    {
      this.gd = FsmVariables.GlobalVariables.FindFsmInt("GlobalDay");
      this.t = GameObject.Find("MAP/SUN/Pivot/SUN").GetComponent<PlayMakerFSM>().FsmVariables.GetFsmInt("Time");
      this.pl = GameObject.Find("PLAYER").transform;
    }

    private void FixedUpdate()
    {
      if (this.gd.Value == this.restockDay && !OpeningHours.didRestock)
      {
        this.inventory.Restock();
        OpeningHours.didRestock = true;
      }
      if (this.gd.Value <= this.restockDay && this.gd.Value >= this.restockDay)
        return;
      OpeningHours.didRestock = false;
    }

    private void LateUpdate() => this.CheckTimeDay();

    private void CheckTimeDay()
    {
      if (this.gd.Value == 7 || (this.t.Value >= this.ClosingHour || this.t.Value < this.OpeningHour) && (double) Vector3.Distance(((Component) this).transform.position, this.pl.position) >= 9.0)
        this.closed = true;
      if (this.gd.Value != 7 && this.t.Value < this.ClosingHour && this.t.Value >= this.OpeningHour)
        this.closed = false;
      for (int index = 0; index < this.objects.Length; ++index)
      {
        if (this.objects[index].activeInHierarchy != !this.closed)
          this.objects[index].SetActive(!this.closed);
      }
      this.windows.SetColor("_EmissionColor", !this.closed ? Color.black : new Color(0.7647059f, 0.7647059f, 0.15f, 1f));
    }
  }
}
