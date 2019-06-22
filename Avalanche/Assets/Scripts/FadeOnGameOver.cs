using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOnGameOver : MonoBehaviour {

    public GameObject player;
    public float fadeTime;
    public float opaqueTime;

    private Image image;
    private bool fading;
    private float fadeTimer;

    private void Start()
    {
        image = GetComponent<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.0f);
        fading = false;
        fadeTimer = 0.0f;
    }

    private void Update()
    {
        if (player == null)
        {
            fading = true;
        }

        if (fading)
        {
            fadeTimer += Time.deltaTime;

            if (fadeTimer <= fadeTime)
            {
                image.color += Color.black * Time.deltaTime / fadeTime;
            }

            if (fadeTimer >= fadeTime + opaqueTime)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0.0f);
            }
        }
    }
}
