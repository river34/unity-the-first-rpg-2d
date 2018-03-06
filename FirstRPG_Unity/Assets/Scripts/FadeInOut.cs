using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    public BoolObject InLoading;

    public FloatObject LoadingTime;

    public RawImage Background;

    [SerializeField]
    public float loadingTime;

    private Coroutine fading;
    private const float MIN_LOADING_TIME = 2;

    private void Start()
    {
        InLoading.OnUpdated += OnInLoadingUpdatedHandler;

        if (loadingTime < MIN_LOADING_TIME)
        {
            loadingTime = MIN_LOADING_TIME;
        }

        LoadingTime.Value = loadingTime;
    }

    IEnumerator FadingInOut()
    {
        Color color = Background.color;
        color.a = 0;
        Background.color = color;
        float speed = 1 / loadingTime * 2;
        while (Background.color.a < 1)
        {
            color.a += speed * Time.deltaTime;
            Background.color = color;
            yield return new WaitForEndOfFrame();
        }
        color.a = 1;
        Background.color = color;
        
        while (Background.color.a > 0)
        {
            color.a -= speed * Time.deltaTime;
            Background.color = color;
            yield return new WaitForEndOfFrame();
        }
        color.a = 0;
        Background.color = color;
        fading = null;
        InLoading.Value = false;
    }

    void OnInLoadingUpdatedHandler()
    {
        if (InLoading.Value == true)
        {
            StartCoroutine(FadingInOut());
        }
    }
}
