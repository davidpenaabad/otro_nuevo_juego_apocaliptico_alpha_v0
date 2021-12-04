using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEditor;

public class InfoPanel : MonoBehaviour
{
    public GameObject infoPanel;
    public GameObject deckPlayer;
    public Canvas mainCanvas;
    Character activePlayer;
    public Text nameText, levelText, healthText, handText, aimPercentageText, aimSlotsText, defensePercentageText, defenseSlotsText, cardsInDeckText, specialHabilityNameText, specialHabilityDescText, descriptionText;
    public Image photo;
    string photoPath;

    private void Awake()
    {
        activePlayer = mainCanvas.GetComponent<DrawCards>().activeCharacter.character;
    }

    public void ShowPanel()
    {
        if(transform.tag == "Player")
        {
            nameText.text = activePlayer.name;
            levelText.text = activePlayer.level.ToString();
            healthText.text = activePlayer.health.ToString();
            handText.text = activePlayer.hand.ToString();
            aimPercentageText.text = activePlayer.aimBasePercentage.ToString();
            aimSlotsText.text = activePlayer.aimBaseSlots.ToString();
            defensePercentageText.text = activePlayer.defenseBasePercentage.ToString();
            defenseSlotsText.text = activePlayer.defenseBaseSlots.ToString();
            cardsInDeckText.text = deckPlayer.transform.childCount.ToString();
            specialHabilityNameText.text = activePlayer.specialHabilityName;
            specialHabilityDescText.text = activePlayer.specialHabilityDescription;
            descriptionText.text = activePlayer.characterDescription;

            photoPath = "Sprites/" + activePlayer.name + "/" + activePlayer.name + "Photo";
            photo.sprite = Resources.Load(photoPath, typeof(Sprite)) as Sprite;
        }

        infoPanel.SetActive(true);
        StartCoroutine(PositionPanel());
    }

    public void HiddenPanel()
    {
        infoPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    IEnumerator PositionPanel()
    {
        float xPosition = Input.mousePosition.x + (infoPanel.GetComponent<RectTransform>().rect.width / 2);
        float yPosition = Input.mousePosition.y + (infoPanel.GetComponent<RectTransform>().rect.height / 2);

        infoPanel.transform.position = new Vector2(xPosition, yPosition);

        yield return new WaitForEndOfFrame();
        Time.timeScale = 0f;
    }
}
