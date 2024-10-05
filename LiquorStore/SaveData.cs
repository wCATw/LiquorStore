// Decompiled with JetBrains decompiler
// Type: LiquorStore.SaveData
// Assembly: LiquorStore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FC65CEA2-D49C-4FF1-A770-819DB08A2A29
// Assembly location: C:\Users\Kotovskiy\Documents\MySummerCar\Mods\LiquorStore.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace LiquorStore
{
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
}