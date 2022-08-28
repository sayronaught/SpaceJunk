using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdPoster : MonoBehaviour
{
    public List<Texture> Posters;
    public float Timer = 15f;

    private RawImage myImage;

    private int currentImage = 0;
    private float nextTimer = 200f;

    // Start is called before the first frame update
    void Start()
    {
        myImage = GetComponent<RawImage>();
        nextTimer = Timer;
    }

    // Update is called once per frame
    void Update()
    {
        if ( nextTimer < 0f)
        {
            currentImage++;
            if (currentImage >= Posters.Count) currentImage = 0;
            myImage.texture = Posters[currentImage];
            nextTimer = Timer;
        }
        nextTimer -= Time.deltaTime;
    }
}
