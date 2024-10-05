// Decompiled with JetBrains decompiler
// Type: LiquorStore.SmokingHelper
// Assembly: LiquorStore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FC65CEA2-D49C-4FF1-A770-819DB08A2A29
// Assembly location: C:\Users\Kotovskiy\Documents\MySummerCar\Mods\LiquorStore.dll

using UnityEngine;

#nullable disable
namespace LiquorStore
{
    public class SmokingHelper : MonoBehaviour
    {
        public Animation anim;
        public LookAt lookAt;

        private void OnEnable() => this.lookAt.isSmoking = true;

        private void OnDisable() => this.lookAt.isSmoking = false;

        private void Update()
        {
            if (this.anim.isPlaying)
                return;
            ((Behaviour) this).enabled = false;
        }
    }
}