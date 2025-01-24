using UnityEngine;

[CreateAssetMenu(fileName = "New World Tile", menuName = "World Tile")]
public class WorldTile : ScriptableObject
{
    [SerializeField]
    public Sprite FillSprite;
    [SerializeField]
    public float FillLayer;
    [SerializeField]
    public Sprite BackgroundSprite;
    [SerializeField]
    public float BackgroundLayer;

    public void Instantiate(Vector2Int position, GameObject parent)
    {
        {
            var container = new GameObject("Tile");
            container.transform.parent = parent.transform;
        
            container.transform.position = (Vector2)position;
            container.transform.position += new Vector3(0f, 0f, FillLayer);
            
            var renderer = container.AddComponent<SpriteRenderer>();
            renderer.sprite = FillSprite;
        }
        
        if(BackgroundSprite != null)
        {
            var container = new GameObject("Bg Tile");
            container.transform.parent = parent.transform;
        
            container.transform.position = (Vector2)position;
            container.transform.position += new Vector3(0f, 0f, BackgroundLayer + position.y / 1024f);

            var renderer = container.AddComponent<SpriteRenderer>();
            renderer.sprite = BackgroundSprite;
        }
    }
}
