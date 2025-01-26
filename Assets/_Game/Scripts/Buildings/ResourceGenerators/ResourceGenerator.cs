using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceGenerator : Building
{
    [Header("Generation Settings")] 
    [SerializeField]
    private List<Resource> _resourcePrefabs = new ();
    [SerializeField]
    private float _frequency;
    [SerializeField]
    private SpriteAnimation _animation;

    private float _timer;
    private int _counter;
    
    protected override void OnInit()
    {
        ResourceGeneratorScheduler.Instance.Register(this);
    }

    private void OnDestroy()
    {
        ResourceGeneratorScheduler.Instance.Remove(this);
    }
    
    public override bool CanTakeItem(Resource item) => false;
    public override void TakeItem(Resource item) { }
    
    public override void ExecuteStep(float deltaTime)
    {
        var totalCount = 0;
        var resourceCount = new Dictionary<Resource, int>();
        
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                if(!GridManager.Instance.Resources.TryGetValue(GridPosition + new Vector2Int(x, y), out var resource))
                    continue;
                
                if(!_resourcePrefabs.Contains(resource.ResourcePrefab))
                    continue;

                resourceCount.TryAdd(resource.ResourcePrefab, 0);
                resourceCount[resource.ResourcePrefab] += 1;
                totalCount += 1;
            }
        }
        
        if(totalCount == 0)
            return;

        _animation.enabled = true;
        
        _timer += deltaTime;
        if (_timer < 1f / (_frequency * totalCount))
            return;
        
        _timer -= 1f / (_frequency * totalCount);

        var count = 0;
        foreach (var resourceKey in resourceCount.Keys)
        {
            count += resourceCount[resourceKey];
            if (_counter >= count) 
                continue;
            
            SpawnResource(resourceKey);
            break;
        }
        
        _counter = (_counter + 1) % totalCount;
    }

    private void SpawnResource(Resource resource)
    {
        var (_, output) = GetIOConveyors();
        var availableOutputs = output.Where(x => x.CanTakeItem(resource)).ToList();
        if (availableOutputs.Count == 0)
            return;
        
        var randomOutput = availableOutputs[Random.Range(0, availableOutputs.Count)];
        var newResource = Instantiate(resource, randomOutput.transform.position, Quaternion.identity);
        randomOutput.TakeItem(newResource);
    }
}