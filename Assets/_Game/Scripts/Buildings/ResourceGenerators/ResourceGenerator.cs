using UnityEngine;

public class ResourceGenerator : Building
{
    [Header("Generation Settings")] 
    [SerializeField]
    private Resource _resourcePrefab;
    [SerializeField]
    private float _frequency;

    private float _timer;
    
    public override bool CanTakeItem(Resource item) => false;
    public override void TakeItem(Resource item) { }
    
    public override void ExecuteStep(float deltaTime)
    {
        var resourceCount = 0;
        
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                if(!GridManager.Instance.Resources.TryGetValue(GridPosition + new Vector2Int(x, y), out var resource))
                    continue;
                
                if(_resourcePrefab != resource.ResourcePrefab)
                    continue;

                resourceCount += 1;
            }
        }
        
        _timer += Time.deltaTime;
        if ( _timer < 1f / (_frequency * resourceCount) )
            return;
        
        _timer -= 1f / (_frequency * resourceCount);
        
        //spawn resrouce
        Debug.Log("Spawn allergy!");
    }
}