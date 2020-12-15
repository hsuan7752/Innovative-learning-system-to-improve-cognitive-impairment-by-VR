using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject Stage1;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            SceneMenu.menu = SceneMenu.Menu.STAGE_ONE;

            Debug.Log(SceneMenu.menu);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Stage1)
        {
            Stage1.SetActive(false);
            SceneMenu.menu = SceneMenu.Menu.STAGE_ONE;
            SceneMenu.change = true;
        }
    }
}
