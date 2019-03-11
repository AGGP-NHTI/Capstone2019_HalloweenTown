using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelInfo : MonoBehaviour
{
    #region Static Stuff
    protected static LevelInfo _instance;

    public static Mom GetMom()
    {
        if (!_instance)
        {
            return null;
        }

        return _instance.Mom;
    }

    public static Canvas GetScoreBoard()
    {
        if(!_instance)
        {
            return null;
        }

        return _instance.scoreBoard;
    }

    public static Text[] GetPlayerScores()
    {
        if (!_instance)
        {
            return null;
        }

        return _instance.playerScore;
    }

    public static GameObject GetIntroCutscene()
    {
        if (!_instance)
        {
            return null;
        }

        return _instance.IntroCutscene;
    }

    public static GameObject GetMomCutscene()
    {
        if (!_instance)
        {
            return null;
        }

        return _instance.MomCutscene;
    }
    #endregion

    #region Instance Stuff
    public Mom Mom;
    public GameObject IntroCutscene;
    public GameObject MomCutscene;
    public Canvas scoreBoard;
    public Text[] playerScore;

    protected void Awake()
    {
        _instance = this;
    }

    protected void OnDestroy()
    {
        _instance = null;
    }
    #endregion
}
