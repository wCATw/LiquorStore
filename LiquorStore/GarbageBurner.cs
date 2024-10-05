using UnityEngine;

namespace LiquorStore;
public class GarbageBurner : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        CustomLiquorCase component1 = ((Component) other).GetComponent<CustomLiquorCase>();
        DrinkBehaviour component2 = ((Component) other).GetComponent<DrinkBehaviour>();
        if (Object.op_Inequality((Object) component1, (Object) null) && ((IEnumerable<GameObject>) component1.bottles).Where<GameObject>((Func<GameObject, bool>) (x => x.activeSelf)).Count<GameObject>() <= 0)
            Object.Destroy((Object) ((Component) other).gameObject);
        if (!Object.op_Inequality((Object) component2, (Object) null) || !component2.isEmpty)
            return;
        Object.Destroy((Object) ((Component) other).gameObject);
    }
}