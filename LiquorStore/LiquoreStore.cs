using MSCLoader;
using UnityEngine;
using Resources = LiquorStore.Properties.Resources;
using Object = UnityEngine.Object;

namespace LiquorStore;

public class LiquorStore : Mod
{
  internal GameObject liquorStore;
  private LiquoreStoreSettings settings;
  private AssetBundle ab;

  public override string ID => nameof (LiquorStore);

  public override string Name => nameof (LiquorStore);

  public override string Author => "BrennFuchS";

  public override string Version => "0.2.3";

  public override string Description => description.random();

  public override string UpdateLink => "https://www.nexusmods.com/mysummercar/mods/865";

  public override byte[] Icon => Resources.Icon;

  public override void OnNewGame() => SaveUtility.Remove();

  public override void OnLoad()
  {
    this.ab = AssetBundle.CreateFromMemoryImmediate(Resources.liquorstore);
  }

  public override void PostLoad()
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

  public override void OnSave() => SaveUtility.Save();
}
