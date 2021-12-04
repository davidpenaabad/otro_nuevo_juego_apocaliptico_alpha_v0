using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skip : MonoBehaviour
{
    public GameObject animacion, mainMenu;
    static public bool close = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            CloseOpenWindow(animacion, mainMenu);
        }
    }

    static public void CloseOpenWindow(GameObject window, GameObject newWindow)
    {
        newWindow.SetActive(!close);
        window.SetActive(close);
        close = false;
    }
}
