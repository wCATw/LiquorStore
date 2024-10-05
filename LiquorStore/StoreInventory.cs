using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
namespace LiquorStore
{
  public class StoreInventory : MonoBehaviour
  {
    public List<StoreItem> storeItems = new List<StoreItem>();
    public Transform spawnPivot;
    public Transform spawnPivotTable;
    public GameObject bagPrefab;

    public void Restock()
    {
      if (this.storeItems.Count <= 0)
        return;
      for (int index = 0; index < this.storeItems.Count; ++index)
        this.storeItems[index].Reset();
    }

    public void Purchase()
    {
      List<StoreItem> list1 = this.storeItems.Where<StoreItem>((Func<StoreItem, bool>) (x => !x.goesInShoppingBag)).ToList<StoreItem>();
      List<StoreItem> list2 = this.storeItems.Where<StoreItem>((Func<StoreItem, bool>) (x => x.goesInShoppingBag)).ToList<StoreItem>();
      if (list1.Count > 0)
        this.StartCoroutine(this.ItemCreator(SaveUtility.instance.itemCast));
      if (list2.Count <= 0 || list2.Select<StoreItem, int>((Func<StoreItem, int>) (x => x.bought)).Sum() <= 0)
        return;
      ShoppingBag component = ((GameObject) Object.Instantiate((Object) this.bagPrefab, this.spawnPivotTable.position, Quaternion.Euler(this.spawnPivotTable.eulerAngles))).GetComponent<ShoppingBag>();
      foreach (StoreItem storeItem in list2)
      {
        if (storeItem.bought > 0)
          component.items.Add(new ShoppingBagItem()
          {
            brand = storeItem.brand,
            count = storeItem.bought
          });
        storeItem.bought = 0;
      }
      component.raycast = SaveUtility.instance.itemCast;
    }

    private IEnumerator ItemCreator(InteractionRaycast raycast)
    {
      StoreInventory storeInventory = this;
      for (int i = 0; i < storeInventory.storeItems.Count; ++i)
      {
        if (storeInventory.storeItems[i].bought > 0 && !storeInventory.storeItems[i].goesInShoppingBag)
        {
          storeInventory.StartCoroutine(storeInventory.storeItems[i].CreateItems(storeInventory.spawnPivot.position, raycast));
          yield return (object) new WaitForSeconds(0.1f);
        }
      }
      yield return (object) null;
    }
  }
}
