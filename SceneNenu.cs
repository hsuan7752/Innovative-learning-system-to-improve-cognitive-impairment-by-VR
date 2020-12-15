using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class SceneMenu : MonoBehaviour
{
    public static bool change = false;

    public enum Stage
    {
        WHAT_IS_SHE_LOOKINH_AT,
        HOW_DEOS_SHE_FEEL_ABOUT,
    }

    public enum Menu
    {
        MAIN_MENU,
        STAGE_ONE,
        STAGE_TWO,
        STAGE_THREE,
        LAST_TIME,
        RECORD,
    }

    public enum Scenes
    {
        RESTAURANT,
    }

    public static Menu menu = Menu.MAIN_MENU;

    public static Stage stage;

    static SceneMenu instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (this != instance)
        {
            Destroy(gameObject);
        }

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {        
        while (change == true)
        {
            if (menu == Menu.STAGE_ONE)
            {
                stage = Stage.WHAT_IS_SHE_LOOKINH_AT;
                SceneManager.LoadScene(1);
                change = false;
            }
            
            if (menu == Menu.MAIN_MENU)
            {
                SceneManager.LoadScene(0);
                change = false;
            }
        }

        /*if (menu == Menu.STAGE_ONE)
        {
            stage = Stage.WHAT_IS_SHE_LOOKINH_AT;

            SceneManager.LoadScene(1);
        }*/

        if (Input.GetKeyDown(KeyCode.R))
        {
            menu = Menu.STAGE_ONE;
            stage = Stage.WHAT_IS_SHE_LOOKINH_AT;
            change = true;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            menu = Menu.MAIN_MENU;
            change = true;
        } /**/
    }
}
