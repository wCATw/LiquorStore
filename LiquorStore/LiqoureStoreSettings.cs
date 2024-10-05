using HutongGames.PlayMaker;
using MSCLoader;
using UnityEngine;

namespace LiquorStore;
internal class LiquoreStoreSettings : MonoBehaviour
{
    private FsmBool shadows;
    private Light[] lights;

    public void Setup()
    {
        this.shadows = ((IEnumerable<PlayMakerFSM>) ((Component) GameObject.Find("Systems").transform.Find("Options")).GetComponents<PlayMakerFSM>()).First<PlayMakerFSM>((Func<PlayMakerFSM, bool>) (x => x.FsmName == "GFX")).FsmVariables.FindFsmBool("ShadowsHouse");
        this.lights = ((Component) (ModLoader.GetMod("LiquorStore", true) as LiquorStore).liquorStore.transform.Find("lights")).GetComponentsInChildren<Light>(true);
        this.OnDisable();
    }

    private void OnDisable()
    {
        for (int index = 0; index < this.lights.Length; ++index)
            this.lights[index].shadows = this.shadows.Value ? (LightShadows) 1 : (LightShadows) 0;
    }
}