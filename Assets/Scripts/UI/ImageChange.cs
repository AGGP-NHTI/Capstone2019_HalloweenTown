using UnityEngine;
using UnityEngine.UI;

public class ImageChange: MonoBehaviour
{
    Image m_Image;//the Box mask

    public Sprite m_Sprite; // each mask needs to be assigned a sprite

    void Start()
    {
        
        m_Image = GetComponent<Image>();// displays the Box mask image
    
    }

    void Update()
    {
        //Press space to change the Sprite of the Image will change to collision or trigger when you get a new mask

        if (Input.GetKey(KeyCode.Space))
        {
            m_Image.sprite = m_Sprite;
        }
    }
}