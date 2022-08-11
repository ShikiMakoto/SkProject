using UnityEngine;
using UnityEngine.UI;

public class Fps : MonoBehaviour
{
    public Text fpsTexta;

    private int counta;
    private float deltaaTime;
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        counta++;
        deltaaTime += Time.deltaTime;

        if (counta % 60 == 0)
        {
            counta = 1;
            var fps = 60f / deltaaTime;
            deltaaTime = 0;
            fpsTexta.text = $"FPS: {Mathf.Ceil(fps)}";
        }
    }
}