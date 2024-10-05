using UnityEngine;

namespace LiquorStore;

[Serializable]
public class ShoppingBagInfo
{
    public ShoppingBagItem[] items;
    public Vector3 position;
    public Vector3 euler;
}