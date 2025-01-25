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
        var resourceCount = 1;
        
        //detect resources under the building
        
        _timer += Time.deltaTime;
        if ( _timer < 1f / (_frequency * resourceCount) )
            return;
        
        _timer -= 1f / (_frequency * resourceCount);
        
        //spawn resrouce
    }
}