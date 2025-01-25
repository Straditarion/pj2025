using UnityEngine;
using UnityEngine.Serialization;

public abstract class Building : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField]
    public Vector2Int Size;

    public SpriteRenderer SpriteRenderer { get; private set; }
    public int Rotation { get; private set; }
    public Vector2Int GridPosition { get; private set; }
    
    public Vector2Int Forward => Rotation switch
    {
        0 => Vector2Int.up,
        1 => Vector2Int.right,
        2 => Vector2Int.down,
        3 => Vector2Int.left,
    };
    public Vector2Int Right => Rotation switch
    {
        0 => Vector2Int.right,
        1 => Vector2Int.down,
        2 => Vector2Int.left,
        3 => Vector2Int.up,
    };
    public Vector2Int Back => -Forward;
    public Vector2Int Left => -Right;
    
    public abstract void ExecuteStep(float deltaTime);

    public void Initialize(Vector2Int gridPosition, int rotation)
    {
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        
        Rotation = rotation;
        SpriteRenderer.transform.rotation = Quaternion.Euler(0, 0, SpriteRenderer.transform.eulerAngles.z + Rotation * -90);
        GridPosition = gridPosition;
        
        OnInit();
    }

    public void AsGhost(Vector2Int gridPosition, int rotation)
    {
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        
        Rotation = rotation;
        SpriteRenderer.transform.rotation = Quaternion.Euler(0, 0, SpriteRenderer.transform.eulerAngles.z + Rotation * -90);
        GridPosition = gridPosition;
    }
    
    protected virtual void OnInit()
    {
        
    }

    public abstract bool CanTakeItem(Resource item);
    public abstract void TakeItem(Resource item);
}