// Decompiled with JetBrains decompiler
// Type: LiquorStore.Radio
// Assembly: LiquorStore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FC65CEA2-D49C-4FF1-A770-819DB08A2A29
// Assembly location: C:\Users\Kotovskiy\Documents\MySummerCar\Mods\LiquorStore.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
namespace LiquorStore
{
    public class Radio : MonoBehaviour
    {
        public AudioSource speaker;
        public AudioSource ch;

        private void Start()
        {
            this.ch = ((IEnumerable<AudioSource>) Resources.FindObjectsOfTypeAll<AudioSource>()).First<AudioSource>((Func<AudioSource, bool>) (x => ((Object) ((Component) x).gameObject).name == "Channel1"));
            this.speaker.clip = this.ch.clip;
            this.speaker.Play();
        }

        private void Update()
        {
            if (!(((Object) this.speaker.clip).name != ((Object) this.ch.clip).name))
                return;
            this.speaker.clip = this.ch.clip;
            this.speaker.Play();
            this.speaker.timeSamples = this.ch.timeSamples;
        }
    }
}