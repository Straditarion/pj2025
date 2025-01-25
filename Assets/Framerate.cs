using System;
using TMPro;
using UnityEngine;

public class Framerate : MonoBehaviour
{
    [SerializeField]
    public TMP_Text text;

    private void Update()
    {
        text.text = System.Math.Round(1f / Time.unscaledDeltaTime, 1).ToString();
    }
}
