using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusic : MonoBehaviour {

    public static BackgroundMusic instance;

    public AudioSource asource;
    public AudioClip menuMusic;
    public AudioClip momMusic;
    public AudioClip endgameMusic;
    public AudioClip gameMusic;
    public float pitch = 0.01f;

    Coroutine activePitchShift;

    RoundManager rm;

    void Start () {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(this);
            MenuMusic();
            instance = this;
        }
	}

    private void Update()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        /*if(rm != null)
        {
            if(rm.currentPhase == RoundManager.RoundPhase.ROUND_ENDING)
            {
                MomMusic();
            }
            else if (rm.currentPhase == RoundManager.RoundPhase.ROUND_OVER)
            {
                EndGameMusic();
            }
        }*/
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //StopMomMusic();
        if (scene.name == "MenuScene")
        {
            MenuMusic();
            rm = null;
        }
        else if (scene.name == "MainScene")
        {
            GameMusic();
           // rm = GameObject.FindGameObjectWithTag("RoundManager").GetComponent<RoundManager>();
        }
    }

    public void MenuMusic()
    {
        //StopMomMusic();
        asource.clip = menuMusic;
        asource.Play();
        asource.loop = true;
    }

    public void GameMusic()
    {

        //StopMomMusic();
        asource.clip = gameMusic;
        asource.Play();
        asource.loop = true;
    }

    public void MomMusic()
    {
        asource.clip = momMusic;
        asource.Play();
        asource.loop = true;
        if(activePitchShift != null)
        {
            StopCoroutine(activePitchShift);
        }
        activePitchShift = StartCoroutine(pitchChange());
    }
    public void EndGameMusic()
    {
        if (activePitchShift != null)
        {
            StopCoroutine(activePitchShift);
        }
        asource.pitch = 1;
        asource.clip = endgameMusic;
        asource.Play();
        asource.loop = true;
    }

    /*public void StopMomMusic()
    {
        StopCoroutine(pitchChange());
        asource.pitch = 1;
    }*/

    IEnumerator pitchChange()
    {
        while (asource.pitch < 2)
        {
            asource.pitch += Time.deltaTime * pitch;
            yield return null;
        }

        activePitchShift = null;
    }
}
