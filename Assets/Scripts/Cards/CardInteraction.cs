using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;

public class CardInteraction : MonoBehaviour
{
    bool isDragging = false;
    bool isOverSlot = false;
    public bool isActive = false;

    GameObject colliderZone;

    Vector2 startPosition;
    int cardIndex;

    Animator zoom;
    bool endZoomIn;

    private void Start()
    {
        zoom = transform.GetComponent<Animator>();
        endZoomIn = false;
        transform.GetComponent<EventTrigger>().enabled = false;

    }

    void Update()
    {
        if (isDragging)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            //Debug.Log(transform.GetComponent<GameObjectCardData>().cardName);
        }

        if (isActive)
        {
            if (transform.GetComponent<GameObjectCardData>().effects.Length > 0 && transform.GetComponent<GameObjectCardData>().effects != null && endZoomIn)
            {
                ShowCardPanel();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        colliderZone = collision.gameObject;

        if (transform.GetComponent<GameObjectCardData>().type == transform.parent.GetComponent<BattleStats>().battleKind || transform.GetComponent<GameObjectCardData>().type == "special")
        {
            if (colliderZone.name == "PlayerSlots")
            {
                isOverSlot = true;
                colliderZone.GetComponent<Image>().color = new Vector4(255, 255, 255, 0.5f);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isOverSlot = false;

        if (colliderZone.name == "PlayerSlots")
        {
            colliderZone.GetComponent<Image>().color = new Vector4(255, 255, 255, 0f);
        }
    }

    public void StartDrag()
    {
        HiddenCardPanel();

        if (isActive && !isDragging)
        {
            startPosition = transform.position;
            isDragging = true;
            cardIndex = transform.GetSiblingIndex();
            zoom.SetTrigger("ZoomOut");
            transform.SetParent(transform.parent.transform.parent, false);
            transform.GetComponent<BoxCollider2D>().enabled = true;

            EnableEventTriggerPortrait(transform.parent.gameObject, false);
        }
    }

    public void EndDrag()
    {
        isDragging = false;

        if (isOverSlot)
        {                    
            if (colliderZone.name == "PlayerSlots")
            {
                int slotIndex = transform.parent.GetComponent<BattleStats>().fullPlayerSlots;
                transform.SetParent(colliderZone.transform, false);
                Destroy(transform.parent.GetChild(slotIndex).gameObject);
                transform.SetParent(colliderZone.transform, false);
                transform.SetSiblingIndex(slotIndex);
                transform.GetChild(transform.childCount - 2).gameObject.SetActive(true);
                transform.parent.transform.parent.GetComponent<BattleStats>().fullPlayerSlots++;
                Stack messageText = new Stack();
                messageText.Push(transform.GetComponent<GameObjectCardData>().level);
                messageText.Push(transform.GetComponent<GameObjectCardData>().ID);
                transform.parent.parent.SendMessage("CardAction", messageText);

                transform.GetComponent<EventTrigger>().enabled = false;
                transform.GetComponent<BoxCollider2D>().enabled = false;
                transform.GetComponent<Rigidbody2D>().Sleep();

                EnableEventTriggerPortrait(transform.parent.parent.gameObject, true);
            }  
        }
        else
        {
            transform.position = startPosition;
            transform.SetParent(transform.parent.Find("PlayerZone"));
            transform.SetSiblingIndex(cardIndex);
            transform.GetComponent<BoxCollider2D>().enabled = false;

            EnableEventTriggerPortrait(transform.parent.parent.gameObject, true);
        }

        
    }

    public void ZoomCardIn()
    {
        if (isActive)
        {
            zoom.SetTrigger("ZoomIn");
        }
    }

    public void ZoomCardOut()
    {

        if (isActive)
        {
            zoom.SetTrigger("ZoomOut");
            HiddenCardPanel();
        }
    }

    public void OnClick()
    {
        try
        {
            if (transform.parent.parent.GetComponent<BattleStats>().discardActive)
            {
                zoom.SetTrigger("BurnCard");
            }
        }
        catch
        {
        }
    }

    public void BurnCard()
    {
        CardClass tempCard = new CardClass();
        tempCard.ID = transform.GetComponent<GameObjectCardData>().ID;
        tempCard.cardName = transform.GetComponent<GameObjectCardData>().cardName;
        tempCard.cardDescription = transform.GetComponent<GameObjectCardData>().cardDescription;
        tempCard.type = transform.GetComponent<GameObjectCardData>().type;
        tempCard.level = transform.GetComponent<GameObjectCardData>().level;
        tempCard.levelMark = transform.GetComponent<GameObjectCardData>().levelMark;
        tempCard.frameworkType = transform.GetComponent<GameObjectCardData>().frameworkType;
        tempCard.imageArt = transform.GetComponent<GameObjectCardData>().imageArt;
        tempCard.effects = transform.GetComponent<GameObjectCardData>().effects;

        string directoryPath = transform.parent.parent.GetComponent<MainData>().directoryPath;
        string tempPath = "";
        string tempJSONString = "";

        if (transform.parent.name == "PlayerZone" || isActive)
        {
            tempPath = directoryPath + "JSON/Decks/" + transform.parent.parent.GetComponent<DrawCards>().activeCharacter.character.name + "/BurnedDeck.json";
            transform.parent.parent.GetComponent<DrawCards>().activeCharacter.burnedDeck.cards.Add(tempCard);
            //Debug.Log(transform.parent.parent.GetComponent<DrawCards>().activeCharacter.burnedDeck.cards[1].cardName);
            tempJSONString = JsonUtility.ToJson(transform.parent.parent.GetComponent<DrawCards>().activeCharacter.burnedDeck);
            //Debug.Log(tempJSONString);
        }
        else if (transform.parent.name == "EnemyZone")
        {
            tempPath = directoryPath + "JSON/Decks/" + transform.parent.parent.GetComponent<DrawCards>().activeEnemy.enemy.ID + "/BurnedDeck.json";
            transform.parent.parent.GetComponent<DrawCards>().activeEnemy.burnedDeck.cards.Add(tempCard);
        }

        File.WriteAllText(tempPath, tempJSONString);

        for (int i = 0; i < transform.childCount - 1; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        StartCoroutine(DestroyCard());
    }

    IEnumerator DestroyCard()
    {
        yield return new WaitForSeconds(1f);
        transform.parent.parent.GetComponent<BattleStats>().discardAmount++;
        Destroy(transform.gameObject);
        Time.timeScale = 1f;
    }

    void EnableEventTriggerPortrait(GameObject gameObject, bool activate)
    {
        gameObject.transform.Find("PlayerPortrait").GetComponent<EventTrigger>().enabled = activate;
        gameObject.transform.Find("EnemyPortrait").GetComponent<EventTrigger>().enabled = activate;
    }

    void ShowCardPanel()
    {
        float positionX = (float)(Input.mousePosition.x);
        float positionY = (float)(Input.mousePosition.y + 97.38);

        Time.timeScale = 0f;

        try
        {
            transform.parent.parent.Find("CardInfoPanel").GetChild(1).GetComponent<Text>().text = transform.GetComponent<GameObjectCardData>().effects;
            transform.parent.parent.Find("CardInfoPanel").position = new Vector3(positionX, positionY, 0);
            transform.parent.parent.Find("CardInfoPanel").gameObject.SetActive(true);
        }
        catch
        {
            transform.parent.Find("CardInfoPanel").GetChild(1).GetComponent<Text>().text = transform.GetComponent<GameObjectCardData>().effects;
            transform.parent.Find("CardInfoPanel").position = new Vector3(positionX, positionY, 0);
            transform.parent.Find("CardInfoPanel").gameObject.SetActive(true);
        }
    }

    void HiddenCardPanel()
    {
        Time.timeScale = 1f;
        endZoomIn = false;

        try
        {
            transform.parent.parent.Find("CardInfoPanel").gameObject.SetActive(false);
        }
        catch
        {
            transform.parent.Find("CardInfoPanel").gameObject.SetActive(false);
        }
    }

    public void EndZoomIn()
    {
        endZoomIn = true;
    }
}
