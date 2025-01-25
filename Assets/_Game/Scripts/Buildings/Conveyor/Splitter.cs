using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Splitter : Building
{
    private int _outputStep;
    private int _inputStep;

    protected override void OnInit()
    {
        ConveyorScheduler.Instance.Register(this);
    }

    private void OnDestroy()
    {
        ConveyorScheduler.Instance.Remove(this);
    }
    
    public override bool CanTakeItem(Resource item) => false;

    public override void TakeItem(Resource item) { }

    public override void ExecuteStep(float deltaTime)
    {
        var (inputs, outputs) = GetIOConveyors();
        
        var correctOutputs = outputs.Where(x => x.Forward == Forward).ToList();
        if(correctOutputs.Count == 0)
            return;
        
        var correctInputs = inputs.Where(x => x.Forward == Forward && x.Item).ToList();
        if(correctInputs.Count == 0)
            return;

        var selectedInput = correctInputs[_inputStep % correctInputs.Count];

        var possibleOutputs = correctOutputs.Where(x => x.CanTakeItem(selectedInput.Item)).ToList();
        if(possibleOutputs.Count == 0)
            return;
        
        var selectedOutput = possibleOutputs[_inputStep % possibleOutputs.Count];

        selectedOutput.TakeItem(selectedInput.Item);
        selectedInput.Item.transform.position = selectedOutput.transform.position;
        selectedInput.Item = null;

        _inputStep = (_inputStep + 1) % (int.MaxValue - 1);
        _outputStep = (_outputStep + 1) % (int.MaxValue - 1);
    }
}