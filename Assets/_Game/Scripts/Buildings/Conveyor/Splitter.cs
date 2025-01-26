using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Splitter : Building
{
    [Header("Splitter Settings")]
    [SerializeField]
    private float _splitterSpeed;
    
    private int _outputStep;
    private int _inputStep;

    private List<SplitterItem> _items = new();

    [System.Serializable]
    private class SplitterItem
    {
        public Resource resource;
        public bool isReady;
        public Vector3 destination;
    }
    
    protected override void OnInit()
    {
        ConveyorScheduler.Instance.Register(this);
    }

    public override void OnDeconstruct()
    {
        foreach (var splitterItem in _items)
        {
            GlobalInventoryState.Instance.AddResource(splitterItem.resource.Name);
        }
        
        OnDestroy();
    }
    
    private void OnDestroy()
    {
        ConveyorScheduler.Instance.Remove(this);
        
        foreach (var splitterItem in _items)
        {
            Destroy(splitterItem.resource.gameObject);
        }
    }
    
    public override bool CanTakeItem(Resource item) => false;

    public override void TakeItem(Resource item) { }

    public override void ExecuteStep(float deltaTime)
    {
        var (inputs, outputs) = GetIOConveyors();
        
        ObtainNewItems(deltaTime, inputs);
        MoveUnreadyItems(deltaTime);
        SelectOutput(deltaTime, outputs);
    }

    private void ObtainNewItems(float deltaTime, List<Conveyor> inputs)
    {
        if(_items.Count == 2)
            return;
        
        var correctInputs = inputs.Where(x => x.Forward == Forward && x.Item &&
                                              Vector2.Distance(x.Item.transform.position, x.transform.position) < 2 * deltaTime).ToList();
        if(correctInputs.Count == 0)
            return;
        
        var selectedInput = correctInputs[_inputStep % correctInputs.Count];
        
        _items.Add(new SplitterItem
        {
            resource = selectedInput.Item,
            isReady = false,
            destination = selectedInput.transform.position + 
                          new Vector3(selectedInput.Forward.x, selectedInput.Forward.y, 0)
        } );
        
        selectedInput.Item = null;
        
        _inputStep = (_inputStep + 1) % (int.MaxValue - 1);
    }

    private void MoveUnreadyItems(float deltaTime)
    {
        if(_items.Count == 0)
            return;

        foreach (var item in _items.Where( x => !x.isReady ))
        {
            var distanceToCenter = Vector2.Distance(item.resource.transform.position, item.destination);
            
            if (distanceToCenter < 2 * deltaTime)
            {
                item.isReady = true;
                continue;
            }
            
            var travelDistance = Mathf.Min(distanceToCenter, _splitterSpeed * deltaTime);
            var travelDirection = (item.destination - item.resource.transform.position).normalized;
            item.resource.transform.position += travelDirection * travelDistance;
        }
    }

    private void SelectOutput(float deltaTime, List<Conveyor> outputs)
    {
        if(_items.Count == 0)
            return;
        
        var correctOutputs = outputs.Where(x => x.Forward == Forward).ToList();
        if(correctOutputs.Count == 0)
            return;

        var toRemove = new List<SplitterItem>();
        foreach (var item in _items.Where( x => x.isReady ))
        {
            var possibleOutputs = correctOutputs.Where(x => x.CanTakeItem(item.resource)).ToList();
            if(possibleOutputs.Count == 0)
                continue;
            
            var selectedOutput = possibleOutputs[_inputStep % possibleOutputs.Count];
            selectedOutput.TakeItem(item.resource);
            item.resource.transform.position = selectedOutput.transform.position +
                                               new Vector3(selectedOutput.Back.x, selectedOutput.Back.y, 0);
            toRemove.Add(item);
            
            _outputStep = (_outputStep + 1) % (int.MaxValue - 1);
        }
        
        toRemove.ForEach( x => _items.Remove(x) );
        toRemove.Clear();
    }
}