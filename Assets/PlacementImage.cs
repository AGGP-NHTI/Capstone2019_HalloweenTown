using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlacementImage : MonoBehaviour {

    public Pawn player;

    int myPlayerScore;

    protected Image p_Image;
    public Sprite baseSprite;
    public Sprite[] placementImages = new Sprite[4];

	// Use this for initialization
	void Start () {
        p_Image = GetComponent<Image>();
        p_Image.sprite = baseSprite;
        
    }

    // Update is called once per frame
    void Update()
    {

        if(!player)
        {
            
            Debug.LogWarning("No pawn assigned for \"pawn\" on PlacementImage for " + name);
            return;
        }
        if (!player.MyController)
        {
            
            return;
        }

        myPlayerScore = Candy.Scoreboard[player.MyController];
        int positionNumber = 0;

        foreach (KeyValuePair<PlayerController, int> kvp in Candy.Scoreboard)
        {
            
            if (kvp.Key != player.MyController)
            {
                if(kvp.Value > myPlayerScore)
                {
                    positionNumber++;
                }
            }
        }

        positionNumber = Mathf.Clamp(positionNumber, 0, placementImages.Length - 1);

        if (p_Image)
        {
            
            p_Image.sprite = placementImages[positionNumber];
        }

    }
}
