using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class WorldGenerator : MonoBehaviour
{
    [Serializable]
    public class WorldLayer
    {
        public LayerMask LayerMask;
        public float Depth;
    }
    
    [SerializeField] 
    public WorldBiome ParentBiome;
    
    [SerializeField]
    public int WorldSize;
    
    [SerializeField]
    public Camera BakeCamera;
    [SerializeField] 
    public WorldLayer[] BakeLayers;
    [SerializeField] 
    public Sprite BakeDrawSprite;
    [SerializeField] 
    public Material BakeDrawMaterial;

    private MaterialPropertyBlock _mpb;
    
    private GameObject _container;
    private RenderTexture[] _worldLayerTextures;

    private void Awake()
    {
        _mpb = new MaterialPropertyBlock();
    }

    private void OnEnable()
    {
        _container = new GameObject("Container");
        _container.transform.parent = transform;

        var tempContainer = new GameObject("Temp Container");
        tempContainer.transform.parent = transform;
        
        var context = new WorldContext()
        {
            Parent = _container,
            TempParent = tempContainer
        };

        var seed = Random.Range(-16777216, 16777216);
        
        for (int x = -WorldSize; x <= WorldSize; x++)
        {
            for (int y = -WorldSize; y <= WorldSize; y++)
            {
                ParentBiome.TryPlace(new Vector2Int(x, y), seed, 1f, context);
            }
        }
        
        _worldLayerTextures = new RenderTexture[BakeLayers.Length];
        
        for (int i = 0; i < BakeLayers.Length; i++)
        {
            var bakeLayer = BakeLayers[i];

            var bakeTexture = new RenderTexture(16 * (WorldSize * 2 + 1), 16 * (WorldSize * 2 + 1), 16, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
            bakeTexture.filterMode = FilterMode.Point;
            bakeTexture.Create();
            
            BakeCamera.targetTexture = bakeTexture;
            BakeCamera.ResetAspect();
            BakeCamera.orthographicSize = WorldSize + 0.5f;
            BakeCamera.cullingMask = bakeLayer.LayerMask;
            BakeCamera.Render();
            
            _worldLayerTextures[i] = bakeTexture;
        }
        
        Destroy(tempContainer);
        
        for (int i = 0; i < BakeLayers.Length; i++)
        {
            var bakeLayer = BakeLayers[i];
            var bakeTexture = _worldLayerTextures[i];
            
            var layerObject = new GameObject("Layer");
            layerObject.transform.parent = _container.transform;
            
            layerObject.transform.position += Vector3.forward * bakeLayer.Depth;
            layerObject.transform.localScale = Vector3.one * (WorldSize * 2 + 1);
            
            var layerSpriteRenderer = layerObject.AddComponent<SpriteRenderer>();
            layerSpriteRenderer.sprite = BakeDrawSprite;
            layerSpriteRenderer.sharedMaterial = BakeDrawMaterial;
            
            _mpb.SetTexture("_DrawThis", bakeTexture);
            layerSpriteRenderer.SetPropertyBlock(_mpb);
            _mpb.Clear();
        }
    }

    private void OnDisable()
    {
        if(_container != null)
            Destroy(_container);

        foreach (var worldLayerTexture in _worldLayerTextures)
        {
            Destroy(worldLayerTexture);
        }
    }
}
