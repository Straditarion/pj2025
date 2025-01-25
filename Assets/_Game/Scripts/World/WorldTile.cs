using UnityEngine;

[CreateAssetMenu(fileName = "New World Tile", menuName = "World Tile")]
public class WorldTile : ScriptableObject
{
    [SerializeField] 
    public bool Obstacle;
    [SerializeField]
    public GameObject Prefab;
    [SerializeField]
    public Sprite FillSprite;
    [SerializeField]
    public float FillLayer;
    [SerializeField]
    public int FillObjectLayer;
    [SerializeField]
    public Sprite BackgroundSprite;
    [SerializeField]
    public float BackgroundLayer;
    [SerializeField]
    public int BackgroundObjectLayer;

    public void Instantiate(Vector2Int position, WorldContext context)
    {
        if (Obstacle || Prefab != null)
        {
            GameObject container;
            if (Prefab == null)
            {
                container = new GameObject("Tile Prefab");
            }
            else
            {
                container = Instantiate(Prefab);
            }

            container.transform.parent = context.Parent.transform;
        
            container.transform.position = (Vector2)position;
            
            if(container.TryGetComponent<ResourceOre>(out var resourceOre))
                GridManager.Instance.Resources.Add(position, resourceOre);
            
            if(Obstacle)
                GridManager.Instance.Obstacles.Add(position, container);
        }
        
        {
            var container = new GameObject("Tile");
            container.layer = FillObjectLayer;
            container.transform.parent = context.TempParent.transform;
        
            container.transform.position = (Vector2)position;
            container.transform.position += new Vector3(0f, 0f, FillLayer + position.y / 1024f);
            
            var renderer = container.AddComponent<SpriteRenderer>();
            renderer.sprite = FillSprite;
        }
        
        if(BackgroundSprite != null)
        {
            var container = new GameObject("Bg Tile");
            container.layer = BackgroundObjectLayer;
            container.transform.parent = context.TempParent.transform;
        
            container.transform.position = (Vector2)position;
            container.transform.position += new Vector3(0f, 0f, BackgroundLayer + position.y / 1024f);

            var renderer = container.AddComponent<SpriteRenderer>();
            renderer.sprite = BackgroundSprite;
        }
    }
}
