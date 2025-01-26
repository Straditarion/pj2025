using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen Instance;
    
    [SerializeField] private float _fadeSpeed;
    [SerializeField] private Image _image;

    private void Awake()
    {
        Instance = this;

        _image.raycastTarget = false;
        
        DontDestroyOnLoad(gameObject);
    }

    public void StartLoading()
    {
        _image.color = new Color(0, 0, 0, 1);
        _image.raycastTarget = true;
    }

    public void Fade()
    {
        _image.raycastTarget = false;
        StartCoroutine(FadeCoroutine());
    }

    private IEnumerator FadeCoroutine()
    {
        _image.color = new Color(0, 0, 0, _image.color.a - (_fadeSpeed * Time.deltaTime));
        
        yield return new WaitForNextFrameUnit();
        
        if(_image.color.a > 0)
            StartCoroutine(FadeCoroutine());
    }
}