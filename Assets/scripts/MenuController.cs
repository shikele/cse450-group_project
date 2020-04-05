using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public static MenuController instance;

    // Outlets
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject levelMenu;
    

	void Awake(){
		instance = this;
        Hide();
	}
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show()
	{
        ShowMainMenu();
        gameObject.SetActive(true);
        Time.timeScale = 0;
        PlayerController.instance.isPaused = true;
	}

    public void Hide()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
        if (PlayerController.instance != null){
            PlayerController.instance.isPaused = false;
        }
    }

    void SwitchMenu(GameObject someMenu)
	{
        // Turn off all menus
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        levelMenu.SetActive(false);

        // Turn on requested menu
        someMenu.SetActive(true);
	}

    public void ShowMainMenu()
	{
        SwitchMenu(mainMenu);
	}

    public void ShowOptionsMenu()
	{
        SwitchMenu(optionsMenu);
	}

    public void ShowLevelMenu()
	{
        SwitchMenu(levelMenu);
	}

    public void LoadLevel1()
	{
        SceneManager.LoadScene("SampleScene");
	}

    public void LoadLevel2()
    {
        SceneManager.LoadScene("level2");
    }

    // public void LoadLevel3()
    // {
    //     SceneManager.LoadScene("Level3");
    // }
}
