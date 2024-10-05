using UnityEngine;
using Object = UnityEngine.Object;

namespace LiquorStore;
public class GarbageBurner : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        CustomLiquorCase component1 = ((Component) other).GetComponent<CustomLiquorCase>();
        DrinkBehaviour component2 = ((Component) other).GetComponent<DrinkBehaviour>();
        if (component1 != null && component1.bottles.Where(x => x.activeSelf).Count() <= 0)
            Object.Destroy(other.gameObject);
        if (component2 == null || !component2.isEmpty)
            return;
        Object.Destroy((Object) ((Component) other).gameObject);
    }
}