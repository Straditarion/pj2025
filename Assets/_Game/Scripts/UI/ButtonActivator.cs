using UnityEngine;

public class ButtonActivator : MonoBehaviour
{

    public GameObject button;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke(nameof(ShowButton), 2f);
    }


    void ShowButton()
    {
        button.SetActive(true);
    }
}
