using UnityEngine;

[CreateAssetMenu(fileName = "New World Tile", menuName = "World Tile")]
public class WorldTile : ScriptableObject
{
    [SerializeField]
    public Sprite FillSprite;
    [SerializeField]
    public Sprite EdgeSprite;
    [SerializeField]
    public Sprite CornerSprite;

    public void Instantiate(Vector2Int position, GameObject parent)
    {
        var container = new GameObject("Tile");
        container.transform.parent = parent.transform;
        
        container.transform.position = (Vector2)position;
        
        container.AddComponent<SpriteRenderer>().sprite = FillSprite;
    }
}
