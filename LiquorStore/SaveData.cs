using System.Runtime.Serialization;

namespace LiquorStore;

[Serializable]
public class SaveData
{
    [OptionalField]
    public bool hasRestocked;
    [OptionalField]
    public CaseInfo[] cases = new CaseInfo[0];
    [OptionalField]
    public BottleInfo[] bottles = new BottleInfo[0];
    [OptionalField]
    public ShopInfo[] shopInfos = new ShopInfo[0];
    [OptionalField]
    public ShoppingBagInfo[] bagInfos = new ShoppingBagInfo[0];
}