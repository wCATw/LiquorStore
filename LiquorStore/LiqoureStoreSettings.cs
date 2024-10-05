// Decompiled with JetBrains decompiler
// Type: LiquorStore.LiquoreStoreSettings
// Assembly: LiquorStore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FC65CEA2-D49C-4FF1-A770-819DB08A2A29
// Assembly location: C:\Users\Kotovskiy\Documents\MySummerCar\Mods\LiquorStore.dll

using HutongGames.PlayMaker;
using MSCLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
namespace LiquorStore
{
    internal class LiquoreStoreSettings : MonoBehaviour
    {
        private FsmBool shadows;
        private Light[] lights;

        public void Setup()
        {
            this.shadows = ((IEnumerable<PlayMakerFSM>) ((Component) GameObject.Find("Systems").transform.Find("Options")).GetComponents<PlayMakerFSM>()).First<PlayMakerFSM>((Func<PlayMakerFSM, bool>) (x => x.FsmName == "GFX")).FsmVariables.FindFsmBool("ShadowsHouse");
            this.lights = ((Component) (ModLoader.GetMod("LiquorStore", true) as LiquorStore.LiquorStore).liquorStore.transform.Find("lights")).GetComponentsInChildren<Light>(true);
            this.OnDisable();
        }

        private void OnDisable()
        {
            for (int index = 0; index < this.lights.Length; ++index)
                this.lights[index].shadows = this.shadows.Value ? (LightShadows) 1 : (LightShadows) 0;
        }
    }
}