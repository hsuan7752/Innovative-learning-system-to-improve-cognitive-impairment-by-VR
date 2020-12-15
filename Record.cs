using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;

public class Record : MonoBehaviour
{
    public TextAsset text;

    public void get_scene(SceneMenu.Scenes scenes)
    {
        scene = scenes.ToString();
    }

    public void selectObject(GameObject obj, SceneMenu.Stage stage, PlayerController.Hint hint)
    {
        switch (stage)
        {
            case SceneMenu.Stage.WHAT_IS_SHE_LOOKINH_AT:
                recordStage = (int)SceneMenu.Stage.WHAT_IS_SHE_LOOKINH_AT;
                switch(hint)
                {                    
                    case PlayerController.Hint.ARROW:
                        stage1_answers[0] = obj.ToString();
                        break;
                    case PlayerController.Hint.FINGER:
                        stage1_answers[1] = obj.ToString();
                        break;
                    case PlayerController.Hint.FLICKER:
                        stage1_answers[2] = obj.ToString();
                        break;
                    case PlayerController.Hint.End:
                        stage1_answers[3] = obj.ToString();
                        break;
                }
                break;
            case SceneMenu.Stage.HOW_DEOS_SHE_FEEL_ABOUT:
                break;
        }
    }

    public void Mastery_Critirion(int masteryCritirion)
    {
        this.masteryCritirion = masteryCritirion;
    }

    public void Frequency(int frequency)
    {
        this.frequency = frequency;
    }

    public void loadRecord()
    {
        string filePath = Application.streamingAssetsPath + "/record2.csv";

        string[] fileData = File.ReadAllLines(filePath);

        for (int i = 0; i < fileData.Length; i++)
        {
            string[] lineData = fileData[i].Split(',');
            for (int j = 0; j < lineData.Length; j++)
            {
                Debug.Log(lineData[j] + "\n");
            }
        }
    }

    public void writeRecord()
    {
        string filePath = Application.streamingAssetsPath + "/record2.csv";


        StreamWriter writer = new StreamWriter(filePath);

        
        writer.WriteLine(scene);
        writer.WriteLine(recordStage.ToString);
        writer.WriteLine(step.ToString);
        for (int i = 0; i < 4; i++)
            writer.WriteLine(stage1_answers[i]);
        for (int i = 0; i < 10; i++)
            writer.WriteLine(stage2_answers[i]);
        for (int i = 0; i < 13; i++)
            writer.WriteLine(stage3_answers[i]);

        writer.Flush();
   
        writer.Close();
    }

    public void stopStage()
    {
        for (int i = 0; i < stage1_answers.Length; ++i)
        {
            if (stage1_answers[i] == null)
            {
                SceneMenu.stage = SceneMenu.Stage.WHAT_IS_SHE_LOOKINH_AT;
                return;
            }
        }
    }

    private string scene;
    private int recordStage;
    private int step;
    private string[] stage1_answers = new string [4];
    private string[] stage2_answers = new string [10];
    private string[] stage3_answers = new string [13];
    private int masteryCritirion;
    private int frequency;
}
