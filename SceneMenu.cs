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

    public static Menu menu = Menu.MAIN_MENU;

    public static Stage stage;

    static SceneMenu instance;

    int sceneCount;
    string[] scenes;

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

        sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
        scenes = new string[sceneCount];
        for (int i = 0; i < sceneCount; i++)
        {
            scenes[i] = System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i));
        }

        /*for (int i = 0; i < sceneCount; ++i)
            print(scenes[i].ToString());*/
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
                int nextScene = UnityEngine.Random.Range(1, sceneCount + 1);

                SceneManager.LoadScene(nextScene);
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
            int nextScene = UnityEngine.Random.Range(1, sceneCount + 1);

            SceneManager.LoadScene(nextScene);
            change = false;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            menu = Menu.MAIN_MENU;
            change = true;
        } /**/
    }
}
