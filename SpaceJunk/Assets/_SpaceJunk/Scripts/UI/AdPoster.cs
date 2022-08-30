using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdPoster : MonoBehaviour
{
    public List<Texture> Posters;
    public float Timer = 15f;

    public bool dontChange;
    public bool randomChange;

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
        if (!dontChange && Posters.Count > 1)
        {
            Debug.Log("cc");
            if (nextTimer < 0f)
            {
                if (!randomChange)
                {
                    currentImage++;
                    if (currentImage >= Posters.Count) currentImage = 0;
                    myImage.texture = Posters[currentImage];
                }
                if (randomChange)
                {
                    int oldPoster = currentImage;
                    while(oldPoster == currentImage)
                    {
                        currentImage = Random.Range(0, Posters.Count);
                    }
                    myImage.texture = Posters[currentImage];
                }
                nextTimer = Timer;
            }
            nextTimer -= Time.deltaTime;
        }
    }
}
