using UnityEngine;

public class Resource : MonoBehaviour
{
    public int OrderIndex;
    public SpriteRenderer SpriteRenderer => transform.GetChild(0).GetComponent<SpriteRenderer>();
    public Sprite Sprite => SpriteRenderer.sprite;
    public string Name => gameObject.name.Replace("(Clone)", "").Trim();
}