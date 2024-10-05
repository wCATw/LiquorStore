using LiquorStore.Properties;
using MSCLoader;
using UnityEngine;

namespace LiquorStore;
public class LiquorStore : Mod
{
  internal GameObject liquorStore;
  private LiquoreStoreSettings settings;
  private AssetBundle ab;

  public virtual string ID => nameof (LiquorStore);

  public virtual string Name => nameof (LiquorStore);

  public virtual string Author => "BrennFuchS";

  public virtual string Version => "0.2.3";

  public virtual string Description => description.random();

  public virtual string UpdateLink => "https://www.nexusmods.com/mysummercar/mods/865";

  public virtual byte[] Icon => Resources.Icon;

  public virtual void OnNewGame() => SaveUtility.Remove();

  public virtual void OnLoad()
  {
    this.ab = AssetBundle.CreateFromMemoryImmediate(Resources.liquorstore);
  }

  public virtual void PostLoad()
  {
    ((Component) GameObject.Find("PERAJARVI").transform.Find("HouseRintama2")).gameObject.SetActive(false);
    ((Component) GameObject.Find("garbage barrel(itemx)").transform.Find("Fire/GarbageTrigger")).gameObject.AddComponent<GarbageBurner>();
    GameObject gameObject = this.ab.LoadAsset<GameObject>("LiquorStore.prefab");
    this.liquorStore = Object.Instantiate<GameObject>(gameObject);
    Object.Destroy((Object) gameObject);
    this.liquorStore.transform.SetParent(GameObject.Find("PERAJARVI").transform);
    this.liquorStore.transform.localPosition = new Vector3(68.86419f, -5.799999f, -59.04696f);
    this.liquorStore.transform.localEulerAngles = new Vector3(-2.689169E-07f, 116.7075f, 3.461563E-07f);
    this.liquorStore.transform.localScale = new Vector3(1f, 1f, 1f);
    this.liquorStore.transform.SetParent((Transform) null, true);
    this.ab.Unload(false);
    this.settings = ((Component) GameObject.Find("Systems").transform.Find("OptionsMenu")).gameObject.AddComponent<LiquoreStoreSettings>();
    this.settings.Setup();
    SaveUtility.Load();
  }

  public virtual void OnSave() => SaveUtility.Save();
}
