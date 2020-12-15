using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{

    public enum Frequency
    {
        FIRST,
        SECOND,
    }

    public enum State
    {
        FIRST_STATE,
        SECOND_STATE,
        THIRDSTATE,
        ForthState,
    }

    public enum Hint
    {
        ARROW,
        FINGER,
        FLICKER,
        End,
    }

    public Scene scene;

    [System.Serializable]
    public class Scene
    {
        public string[] emoji = { "smile", "surprise", "scared", "angry", "dislike" };
        public GameObject NPC;
        public GameObject[] items = new GameObject[6];
        public GameObject[] Questions = new GameObject[2];
        public GameObject wellDone, YesOrNo, instruction, arrowR, arrowL;
        public GameObject[] hints = new GameObject[3];
        public GameObject[] yes_no = new GameObject[2];
    }

    int select = 0;
    public int ans;
    SceneMenu.Menu menu;
    SceneMenu.Stage stage;
    State state = State.FIRST_STATE;
    Hint hint = Hint.ARROW;
    GameObject ansObj;
    bool prompt = false;

    public InteractionSystem interactionSystem;
    public bool interrupt;
    public InteractionObject target;

    GameObject eye;

    private float shake;
    bool h3 = false;

    Animator anim;

    Record record = new Record();

    public static int masteryCriterion;
    public static int[] correct = new int[2];
    public static Frequency frequency = Frequency.FIRST;

    public GameObject menuButton;

    // Start is called before the first frame update
    void Awake()
    {
        if (stage == SceneMenu.Stage.WHAT_IS_SHE_LOOKINH_AT)
            scene.Questions[0].SetActive(true);

        ans = UnityEngine.Random.Range(0, 6);

        //ans = 1;

        eye = GameObject.Find("CC_Base_Eye");
        foreach (GameObject it in scene.hints)
            it.SetActive(false);
        foreach (GameObject it in scene.yes_no)
            it.SetActive(false);
        menuButton.SetActive(false);
        scene.Questions[1].SetActive(false);
        scene.wellDone.SetActive(false);
        scene.YesOrNo.SetActive(false);
        scene.arrowL.SetActive(false);
        scene.arrowR.SetActive(false);
        scene.instruction.SetActive(false);
        ansObj = scene.items[ans];

        target.transform.position = ansObj.transform.position;

        Vector3 targetDir = ansObj.transform.position - eye.transform.position;
        float angle = Vector3.Angle(targetDir, ansObj.transform.forward);

        if (ansObj.transform.position.x > scene.NPC.transform.position.x)
        {
            scene.arrowL.transform.position = (ansObj.transform.position + eye.transform.position) / 2;
            print(angle);
            scene.arrowL.transform.rotation = Quaternion.Euler(0f, 0f, 90 - angle);
        }
        else
        {
            scene.arrowR.transform.position = (ansObj.transform.position + eye.transform.position) / 2;
            print(angle);
            scene.arrowR.transform.rotation = Quaternion.Euler(0f, 0f, 90 - angle);
        }
    }

    private void Start()
    {
        print("Start");
        anim = scene.NPC.GetComponent<Animator>();
        anim.Play(scene.emoji[UnityEngine.Random.Range(0, 5)]);

        interactionSystem.lookAt.Look(target.transform, Time.time + 2.5f);
    }

    private void Update()
    {

        if (h3 == true)
        {
            shake += Time.deltaTime;

            if (shake % 1 > 0.5f)
                scene.items[ans].SetActive(true);
            else
                scene.items[ans].SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.W))
            if (target.transform.position.x < scene.NPC.transform.position.x)
                interactionSystem.StartInteraction(FullBodyBipedEffector.LeftHand, target, interrupt);
            else if (target.transform.position.x > scene.NPC.transform.position.x)
                interactionSystem.StartInteraction(FullBodyBipedEffector.RightHand, target, interrupt);

        if (Input.GetKeyDown(KeyCode.X))
            interactionSystem.lookAt.Look(target.transform, Time.time + (target.length * 0.5f));

        if (Input.GetKeyDown(KeyCode.P))
        {
            //record.selectObject(scene.items[0], stage, hint);
            select = UnityEngine.Random.Range(0, 6); ;
            scene.YesOrNo.transform.position = new Vector3(scene.items[select].transform.position.x, scene.items[select].gameObject.transform.position.y + float.Parse("0.5"), 2);
            scene.YesOrNo.SetActive(true);
            foreach (GameObject it in scene.yes_no)
                it.SetActive(true);
            scene.yes_no[0].gameObject.transform.position = new Vector3(scene.items[select].gameObject.transform.position.x - float.Parse("0.5"), scene.items[select].gameObject.transform.position.y, scene.items[select].gameObject.transform.position.z);
            scene.yes_no[1].gameObject.transform.position = new Vector3(scene.items[select].gameObject.transform.position.x + float.Parse("0.5"), scene.items[select].gameObject.transform.position.y, scene.items[select].gameObject.transform.position.z);
        }


        /*ansObj = scene.items[ans];

        target.transform.position = ansObj.transform.position;

        Vector3 targetDir = ansObj.transform.position - eye.transform.position;
        float angle = Vector3.Angle(targetDir, ansObj.transform.forward);

        if (ansObj.transform.position.x > scene.NPC.transform.position.x)
        {
            scene.arrowL.transform.position = (ansObj.transform.position + eye.transform.position) / 2;
            scene.arrowL.transform.rotation = Quaternion.Euler(0f, 0f, 90 - angle);
        }
        else if (ansObj.transform.position.x < scene.NPC.transform.position.x)
        {
            scene.arrowR.transform.position = (ansObj.transform.position + eye.transform.position) / 2;
            scene.arrowR.transform.rotation = Quaternion.Euler(0f, 0f, 90 - angle);
        }*/
    }

    void OnTriggerEnter(Collider other)
    {
        scene.instruction.SetActive(false);
        switch (state)
        {
            case State.FIRST_STATE: // Tsai: Use enumeration to define states
                select = Array.IndexOf(scene.items, other.gameObject);
                if (select == -1)
                    state = State.FIRST_STATE;
                else
                {
                    state = State.SECOND_STATE;
                    scene.YesOrNo.transform.position = new Vector3(scene.items[select].transform.position.x, scene.items[select].gameObject.transform.position.y + float.Parse("0.5"), 1);
                    scene.YesOrNo.SetActive(true);
                    foreach (GameObject it in scene.yes_no)
                        it.SetActive(true);
                    scene.yes_no[0].gameObject.transform.position = new Vector3(scene.items[select].gameObject.transform.position.x - float.Parse("0.5"), scene.items[select].gameObject.transform.position.y, scene.items[select].gameObject.transform.position.z);
                    scene.yes_no[1].gameObject.transform.position = new Vector3(scene.items[select].gameObject.transform.position.x + float.Parse("0.5"), scene.items[select].gameObject.transform.position.y, scene.items[select].gameObject.transform.position.z);
                }
                break;
            case State.SECOND_STATE: // Tsai: Use enumeration to define states
                int result2 = Array.IndexOf(scene.yes_no, other.gameObject);
                if (result2 == 0)
                {
                    scene.YesOrNo.SetActive(false);
                    foreach (GameObject it in scene.yes_no)
                        it.SetActive(false);
                    record.selectObject(other.gameObject, stage, hint);
                    if (ans == select)
                    {
                        state = State.THIRDSTATE;
                        scene.wellDone.SetActive(true);
                        menuButton.SetActive(true);
                        prompt = true;
                        break;
                    }
                    else
                    {
                        /*switch(stage)
                        {
                            case SceneMenu.Stage.WHAT_IS_SHE_LOOKINH_AT:
                                stage1();
                                break;
                            case SceneMenu.Stage.HOW_DEOS_SHE_FEEL_ABOUT:
                                stage2();
                                break;
                        }*/
                        switch (hint)
                        {
                            case Hint.ARROW: // Tsai: Use enumeration to define states
                                scene.hints[0].SetActive(true);
                                scene.instruction.SetActive(true);
                                if (ansObj.transform.position.x > scene.NPC.transform.position.x)
                                    scene.arrowL.SetActive(true);
                                else if (ansObj.transform.position.x < scene.NPC.transform.position.x)
                                    scene.arrowR.SetActive(true);
                                hint = Hint.FINGER;
                                state = State.FIRST_STATE;
                                break;
                            case Hint.FINGER: // Tsai: Use enumeration to define states
                                scene.hints[1].SetActive(true);
                                scene.instruction.SetActive(true);
                                hint = Hint.FLICKER;
                                state = State.FIRST_STATE;
                                if (target.transform.position.x < scene.NPC.transform.position.x)
                                    interactionSystem.StartInteraction(FullBodyBipedEffector.LeftHand, target, interrupt);
                                else
                                    interactionSystem.StartInteraction(FullBodyBipedEffector.RightHand, target, interrupt);
                                break;
                            case Hint.FLICKER: // Tsai: Use enumeration to define states
                                scene.hints[2].SetActive(true);
                                scene.instruction.SetActive(true);
                                hint = Hint.End;
                                state = State.FIRST_STATE;
                                h3 = true;
                                break;
                            case Hint.End: // Tsai: Use enumeration to define states
                                menuButton.SetActive(true);
                                state = State.THIRDSTATE;
                                break;
                        }
                    }
                }
                else if (result2 == 1)
                {
                    scene.YesOrNo.SetActive(false);
                    foreach (GameObject it in scene.yes_no)
                        it.SetActive(false);
                    state = State.FIRST_STATE;
                }
                else
                    state = State.SECOND_STATE;
                break;
            case State.THIRDSTATE: // Tsai: Use enumeration to define states

                if (other.gameObject == menuButton.gameObject)
                {
                    SceneMenu.menu = SceneMenu.Menu.MAIN_MENU;
                    SceneMenu.change = true;
                }

                masteryCriterion++;
                record.Mastery_Critirion(masteryCriterion);
                record.Frequency((int)frequency);
                switch (frequency)
                {
                    case Frequency.FIRST:
                        if (!prompt)
                            correct[0]++;

                        if (masteryCriterion == 20)
                        {
                            masteryCriterion = 0;
                            if (correct[0] == 20)
                            {
                                stage = SceneMenu.Stage.HOW_DEOS_SHE_FEEL_ABOUT;
                                return;
                            }
                            frequency = Frequency.SECOND;                            
                        }
                        break;
                    case Frequency.SECOND:
                        if (!prompt)
                            correct[1]++;

                        if (masteryCriterion == 20)
                        {
                            masteryCriterion = 0;
                            frequency = Frequency.FIRST;
                            if (correct[0] >= 18 && correct[1] >= 18)
                            {
                                stage = SceneMenu.Stage.HOW_DEOS_SHE_FEEL_ABOUT;
                                return;
                            }
                            correct[0] = 0;
                            correct[1] = 0;
                        }
                        break;
                }
                
                break;
        }
    }

    void stage1()
    {
        switch (hint)
        {
            case Hint.ARROW: // Tsai: Use enumeration to define states
                scene.hints[0].SetActive(true);
                scene.instruction.SetActive(true);
                if (ansObj.transform.position.x > scene.NPC.transform.position.x)
                    scene.arrowL.SetActive(true);
                else if (ansObj.transform.position.x < scene.NPC.transform.position.x)
                    scene.arrowR.SetActive(true);
                hint = Hint.FINGER;
                state = State.FIRST_STATE;
                break;
            case Hint.FINGER: // Tsai: Use enumeration to define states
                scene.hints[1].SetActive(true);
                scene.instruction.SetActive(true);
                hint = Hint.FLICKER;
                state = State.FIRST_STATE;
                if (target.transform.position.x < scene.NPC.transform.position.x)
                    interactionSystem.StartInteraction(FullBodyBipedEffector.LeftHand, target, interrupt);
                else
                    interactionSystem.StartInteraction(FullBodyBipedEffector.RightHand, target, interrupt);
                break;
            case Hint.FLICKER: // Tsai: Use enumeration to define states
                scene.hints[2].SetActive(true);
                scene.instruction.SetActive(true);
                hint = Hint.End;
                state = State.FIRST_STATE;
                h3 = true;
                break;
            case Hint.End: // Tsai: Use enumeration to define states
                menuButton.SetActive(true);
                state = State.THIRDSTATE;
                break;
        }
    }

    public void selectObject()
    {
        print("Click!");
    }
}
