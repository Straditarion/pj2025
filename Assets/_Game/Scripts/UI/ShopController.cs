using UnityEngine;

public class ShopController : MonoBehaviour
{
    public void ChangePrefab(Building prefab)
    {
        Player.Instance.GetSystem<BuildingSystem>().ChangePrefab(prefab);
    }
}
