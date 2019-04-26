using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;

public class MenuScript : MonoBehaviour
{
    protected int activeMenuIndex = -1;
    protected int previousMenuIndex = -1;
    public PhotonManager photonManager;
    public GameObject[] MenuScreens;
    public int StartingMenu = 0;
    public EventSystem eventSystem;

    public bool PauseMenuExists = false;
    
    protected float runtimeTimeScale = 1.0f;
    
    public bool IsPaused
    {
        get;
        protected set;
    }

    private void Start()
    {
        foreach(GameObject menu in MenuScreens)
        {
            if(menu)
            {
                menu.SetActive(false);
            }
        }
        ChangeMenuTo(StartingMenu);
    }

    private void Update()
    {
        if(eventSystem.currentSelectedGameObject == null)
        {
            FocusMenuButton();
        }
    }

    //MAIN MENU FUNCTIONALITY
    //
    //

    //Close splash screen
    public void CloseSplash()
    {
        if(activeMenuIndex == 0)
        {
            ChangeMenuTo(1);
        }
    }

    public void FocusMenuButton()
    {
        MenuInfoHolder mih = MenuScreens[activeMenuIndex].GetComponent<MenuInfoHolder>();
        if (mih && eventSystem)
        {
            eventSystem.SetSelectedGameObject(mih.FirstSelected);
        }
    }

    public void LocalGame(int newMenuIndex)
    {
        PhotonNetwork.OfflineMode = true;
        ChangeMenuTo(newMenuIndex);
        //photonManager.AddController();
    }

    public void OnlineGame(int newMenuIndex)
    {
        PhotonNetwork.OfflineMode = false;
        photonManager.OnlineStart();
        if (photonManager.connectedtomaster)//make loading screen
        {
            ChangeMenuTo(newMenuIndex);
        }
    }

    //Navigate between menus.
    public void ChangeMenuTo(int newMenuIndex)
    {
        //Debug.Log("Previous:" + previousMenuIndex + ", Active:" + activeMenuIndex + ", New:" + newMenuIndex);
        if (newMenuIndex != activeMenuIndex)
        {
            if (0 <= activeMenuIndex && activeMenuIndex < MenuScreens.Length)
            {
                if (MenuScreens[activeMenuIndex])
                {
                    MenuScreens[activeMenuIndex].SetActive(false);
                }
            }
            if (0 <= newMenuIndex && newMenuIndex < MenuScreens.Length)
            {
                if (MenuScreens[newMenuIndex])
                {
                    MenuScreens[newMenuIndex].SetActive(true);
                }
            }

            previousMenuIndex = activeMenuIndex;
            activeMenuIndex = newMenuIndex;
            FocusMenuButton();
        }
    }

    //Navigate to previous menu
    public void BackToPreviousMenu()
    {
        ChangeMenuTo(previousMenuIndex);
    }

    //Also used in pause menu
    public void QuitGame()
    {
        Application.Quit();
    }

    //
    //
    //PAUSE MENU FUNCTIONALITY
    //
    //
    public void ResumeGame()
    {
        ChangeMenuTo(0);
        Time.timeScale = runtimeTimeScale;
        IsPaused = false;
    }

    public void TogglePause()
    {
        if (PauseMenuExists)
        {
            if (!IsPaused)
            {
                ChangeMenuTo(1);
                runtimeTimeScale = Time.timeScale;
                Time.timeScale = 0.0f;
                IsPaused = true;
            }
            else
            {
                ChangeMenuTo(0);
                Time.timeScale = runtimeTimeScale;
                IsPaused = false;
            }
        }
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = runtimeTimeScale;
        SceneManager.LoadScene("Menu");
    }
}
