using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonInteraction : MonoBehaviour
{
    Vector2 defaultShadowPosition;
    string stringButton;
    char[] stringSuffleButton;
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject pauseMenu;
    string defaultButtonName;


    void Awake()
    {
        defaultShadowPosition = new Vector2(4.8f, -1f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void isMouseOver()
    {
        StartCoroutine(MoveShadow());
        this.GetComponentInChildren<Shadow>().enabled = true;
    }

    public void isMouseExit()
    {
        this.GetComponentInChildren<Shadow>().enabled = false;
    }

    public void isMouseClick()
    {
        defaultButtonName = this.transform.GetChild(0).GetComponent<Text>().text;
        captureText(this.transform.GetChild(0).GetComponent<Text>().text);
        StartCoroutine(SuffleTextAndExecute());
        //Debug.Log(stringSuffleButton[0]);
    }

    IEnumerator MoveShadow()
    {
        for(int i = 0; i <= 20; i++)
        {
            float x = Random.Range(-10, 10);
            float y = Random.Range(-10, 10);

            yield return new WaitForSeconds(0.03f);

            transform.GetChild(0).GetComponent<Shadow>().effectDistance += new Vector2(x, y);

            yield return new WaitForSeconds(0.03f);

            transform.GetChild(0).GetComponent<Shadow>().effectDistance = defaultShadowPosition;
        }
    }

    IEnumerator SuffleTextAndExecute()
    {
        for (int i = 0; i <= 10; i++)
        {
            string tempString = "";

            for (int j = 0; j < stringSuffleButton.Length; j++) {
                tempString += stringSuffleButton[j];
            }

            yield return new WaitForSeconds(0.03f);

            transform.GetChild(0).GetComponent<Text>().text = tempString;

            stringSuffleButton.Suffle(8);
        }

        if(this.name == "InitButton")
        {
            SceneManager.LoadScene("QuestScene");
            DefaultButton(this.transform.GetChild(0));
        }
        else if(this.name == "OptionButton")
        {
            settingsMenu.SetActive(true);
            mainMenu.SetActive(false);
            DefaultButton(this.transform.GetChild(0));
        }
        else if(this.name == "QuitButton")
        {
            Application.Quit();
        }
        else if (this.name == "BackButton")
        {
            this.transform.parent.transform.parent.GetChild(this.transform.parent.GetSiblingIndex() - 1).gameObject.SetActive(true);
            this.transform.parent.gameObject.SetActive(false);
            DefaultButton(this.transform.GetChild(0));
        }
    }

    public void captureText(string text)
    {
        stringButton = text;
        stringSuffleButton = stringButton.ToCharArray();
        stringSuffleButton.Suffle(8);
    }

    public void DefaultButton(Transform text) {
        text.GetComponent<Text>().text = defaultButtonName;
        text.GetComponent<Shadow>().enabled = false;
    }
}
