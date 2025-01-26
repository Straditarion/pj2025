using UnityEngine;
using UnityEngine.UIElements;

public class ShopController : MonoBehaviour
{
    public GameObject CancelButton;
    
    public void ChangePrefab(Building prefab)
    {
        Player.Instance.GetSystem<BuildingSystem>().ChangePrefab(prefab);
        CancelButton.SetActive(prefab != null);
    }
}
