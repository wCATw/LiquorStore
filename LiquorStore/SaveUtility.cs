using MSCLoader;
using Newtonsoft.Json;
using UnityEngine;

namespace LiquorStore;
  
public class SaveUtility : MonoBehaviour
{
  public InteractionRaycast itemCast;
  private static SaveSystem saveSystem = new SaveSystem();
  public static SaveUtility instance;
  private static string savePath1 = Path.Combine(Application.persistentDataPath, "LS.savedat");
  public StoreInventory storeInventory;

  private static bool IsPrefab(Transform transform)
  {
    return !((Component) transform).gameObject.activeInHierarchy && ((Component) transform).gameObject.activeSelf && Object.op_Equality((Object) transform.root, (Object) transform);
  }

  private void Start() => SaveUtility.instance = this;

  public static void Save()
  {
    SaveData data = new SaveData();
    data.hasRestocked = OpeningHours.didRestock;
    List<CaseInfo> caseInfoList = new List<CaseInfo>();
    List<BottleInfo> bottleInfoList = new List<BottleInfo>();
    List<ShopInfo> shopInfoList = new List<ShopInfo>();
    List<ShoppingBagInfo> shoppingBagInfoList = new List<ShoppingBagInfo>();
    foreach (CustomLiquorCase customLiquorCase in Resources.FindObjectsOfTypeAll<CustomLiquorCase>())
    {
      if (!SaveUtility.IsPrefab(((Component) customLiquorCase).transform))
        caseInfoList.Add(new CaseInfo()
        {
          brand = customLiquorCase.brand,
          activeBottles = ((IEnumerable<GameObject>) customLiquorCase.bottles).Select<GameObject, bool>((Func<GameObject, bool>) (x => x.activeSelf)).ToArray<bool>(),
          euler = ((Component) customLiquorCase).transform.eulerAngles,
          position = ((Component) customLiquorCase).transform.position
        });
    }
    foreach (DrinkBehaviour drinkBehaviour in Resources.FindObjectsOfTypeAll<DrinkBehaviour>())
    {
      if (!SaveUtility.IsPrefab(((Component) drinkBehaviour).transform))
        bottleInfoList.Add(new BottleInfo()
        {
          brand = drinkBehaviour.brand,
          euler = ((Component) drinkBehaviour).transform.eulerAngles,
          isEmpty = drinkBehaviour.isEmpty,
          position = ((Component) drinkBehaviour).transform.position
        });
    }
    foreach (StoreItem storeItem in SaveUtility.instance.storeInventory.storeItems)
      shopInfoList.Add(new ShopInfo()
      {
        brand = storeItem.brand,
        count = storeItem.count + storeItem.bought
      });
    foreach (ShoppingBag shoppingBag in Resources.FindObjectsOfTypeAll<ShoppingBag>())
    {
      if (!SaveUtility.IsPrefab(((Component) shoppingBag).transform))
        shoppingBagInfoList.Add(new ShoppingBagInfo()
        {
          items = shoppingBag.items.ToArray(),
          euler = ((Component) shoppingBag).transform.eulerAngles,
          position = ((Component) shoppingBag).transform.position
        });
    }
    data.cases = caseInfoList.ToArray();
    data.bottles = bottleInfoList.ToArray();
    data.shopInfos = shopInfoList.ToArray();
    data.bagInfos = shoppingBagInfoList.ToArray();
    JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
    {
      ReferenceLoopHandling = (ReferenceLoopHandling) 1,
      Formatting = (Formatting) 1
    };
    try
    {
      SaveUtility.saveSystem.Save(SaveUtility.savePath1, (object) data);
    }
    catch (Exception ex)
    {
      ModConsole.Error(ex.Message);
    }
    ModConsole.Log("LiquorStore : Save created.");
  }

  public static void Load()
  {
    if (Object.op_Equality((Object) SaveUtility.instance, (Object) null))
      SaveUtility.instance = ((IEnumerable<SaveUtility>) Resources.FindObjectsOfTypeAll<SaveUtility>()).First<SaveUtility>();
    SaveData saveData1 = new SaveData();
    if (File.Exists(SaveUtility.savePath1))
    {
      SaveData saveData2 = SaveUtility.saveSystem.Load<SaveData>(SaveUtility.savePath1);
      OpeningHours.didRestock = saveData2.hasRestocked;
      if (saveData2.cases.Length != 0)
      {
        foreach (CaseInfo caseInfo in saveData2.cases)
        {
          CaseInfo item = caseInfo;
          CustomLiquorCase component = ((GameObject) Object.Instantiate((Object) SaveUtility.instance.storeInventory.storeItems.First<StoreItem>((Func<StoreItem, bool>) (x => x.brand == item.brand)).prefab, item.position, Quaternion.Euler(item.euler))).GetComponent<CustomLiquorCase>();
          for (int index = 0; index < item.activeBottles.Length; ++index)
            component.bottles[index].SetActive(item.activeBottles[index]);
          component.raycast = SaveUtility.instance.itemCast;
        }
      }
      if (saveData2.bottles.Length != 0)
      {
        foreach (BottleInfo bottle in saveData2.bottles)
        {
          BottleInfo item = bottle;
          DrinkBehaviour component = ((GameObject) Object.Instantiate((Object) SaveUtility.instance.storeInventory.storeItems.First<StoreItem>((Func<StoreItem, bool>) (x => x.brand == item.brand)).prefab, item.position, Quaternion.Euler(item.euler))).GetComponent<DrinkBehaviour>();
          component.isEmpty = item.isEmpty;
          component.raycast = SaveUtility.instance.itemCast;
        }
      }
      if (saveData2.shopInfos.Length != 0)
      {
        foreach (ShopInfo shopInfo in saveData2.shopInfos)
        {
          ShopInfo item = shopInfo;
          StoreItem storeItem = SaveUtility.instance.storeInventory.storeItems.First<StoreItem>((Func<StoreItem, bool>) (x => x.brand == item.brand));
          storeItem.count = item.count;
          for (int index = 0; index < storeItem.visuals.Length; ++index)
            storeItem.visuals[index].SetActive(index < storeItem.count);
        }
      }
      if (saveData2.bagInfos.Length != 0)
      {
        foreach (ShoppingBagInfo bagInfo in saveData2.bagInfos)
        {
          ShoppingBag component = ((GameObject) Object.Instantiate((Object) SaveUtility.instance.storeInventory.bagPrefab, bagInfo.position, Quaternion.Euler(bagInfo.euler))).GetComponent<ShoppingBag>();
          component.items = ((IEnumerable<ShoppingBagItem>) bagInfo.items).ToList<ShoppingBagItem>();
          component.raycast = SaveUtility.instance.itemCast;
        }
      }
      ModConsole.Log("LiquorStore : Save loaded and applied.");
    }
    else
      saveData1 = new SaveData();
  }

  public static void Remove()
  {
    SaveUtility.saveSystem.Delete(SaveUtility.savePath1);
    ModConsole.Log("LiquorStore : Save removed");
  }
}
