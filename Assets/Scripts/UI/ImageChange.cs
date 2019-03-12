using UnityEngine;
using UnityEngine.UI;

public class ImageChange: MonoBehaviour
{
    Image m_Image;//the Box mask

    public Sprite baseSprite;
    public Sprite ghostSprite;
    public Sprite witchSprite;
    public Sprite vampireSprite;
    public Sprite werewolfSprite;// each mask needs to be assigned a sprite

    void Start()
    {
        
        m_Image = GetComponent<Image>();// displays the Box mask image
    
    }

    void Update()
    {
        //Press space to change the Sprite of the Image will change to collision or trigger when you get a new mask  
    }

    public void whiteCircle()
    {
        m_Image.sprite = baseSprite;
    }

    public void ghost()
    {
        m_Image.sprite = ghostSprite;
    }

    public void witch()
    {
        m_Image.sprite = witchSprite;
    }

    public void vampire()
    {
        m_Image.sprite = vampireSprite;
    }

    public void werewolf()
    {
        m_Image.sprite = werewolfSprite;
    }
}