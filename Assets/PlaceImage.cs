using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceImage : MonoBehaviour {

    Image p_Image;
    public Sprite firstPlace;
    public Sprite secondPlace;
    public Sprite thirdPlace;
    public Sprite lastPlace;

    void Start()
    {

        p_Image = GetComponent<Image>();

    }

    void Update()
    {

    }

    public void first()
    {
        p_Image.sprite = firstPlace;
    }

    public void second()
    {
        p_Image.sprite = secondPlace;
    }

    public void third()
    {
        p_Image.sprite = thirdPlace;
    }

    public void last()
    {
        p_Image.sprite = lastPlace;
    }
}