using UnityEngine;
using UnityEngine.UI;

public class ImageChange: MonoBehaviour
{
    Image m_Image;//the Box mask

    public Sprite ghostSprite;
    public Sprite witchSprite;
    public Sprite vampireSprite;
    public Sprite warewolfSrpite;// each mask needs to be assigned a sprite

    void Start()
    {
        
        m_Image = GetComponent<Image>();// displays the Box mask image
    
    }

    void Update()
    {
        //Press space to change the Sprite of the Image will change to collision or trigger when you get a new mask

       
    }
}