using UnityEngine;

public class ResourceGenerator : Building
{
    [Header("Generation Settings")] 
    [SerializeField]
    private GameObject _resourcePrefab;
    [SerializeField]
    private float _frequency;
    
    public override bool CanTakeItem(Resource item) => false;
    public override void TakeItem(Resource item) { }
    
    public override void ExecuteStep(float deltaTime)
    {
        throw new System.NotImplementedException();
    }
}