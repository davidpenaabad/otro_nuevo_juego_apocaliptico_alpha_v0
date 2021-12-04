using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;


public class DrawCards : MonoBehaviour
{
    public Canvas mainCanvas;
    //public GameObject card;
    public GameObject playerZone;
    public GameObject enemyZone;
    public GameObject deckReadyPlayerZone, deckReadyEnemyZone;
    public GameObject cardPlayerConvoyPrefab, cardEnemyConvoyPrefab;
    string fileReadyDeckPath, fileBurnedDeckPath, fileExhaustedDeckPath;
    string jsonDecksString;

    private Sprite imageForArt;
    static public string triggerPlayer = "Harry";
    static public string triggerEnemy = "0001";
    public CharactersAndCards activeCharacter;
    public EnemiesAndCards activeEnemy;
    public List<GameObject> cardsGameObjectsPlayer;
    public List<GameObject> cardsGameObjectsEnemy;

    public Sprite discardEnable, discardDisable;

    //private string relativePath = "Assets/Resources/Images/Cards/";
    //private string extension = ".png";


    void Awake()
    {

        discardEnable = Resources.Load("Images/IconsDisplays/DiscardFrameworkEnable", typeof(Sprite)) as Sprite;
        discardDisable = Resources.Load("Images/IconsDisplays/DiscardFrameworkDisable", typeof(Sprite)) as Sprite;

        #region CARGA DEL MAZO DEL JUGADOR Y DE LA MANO INICIAL

        activeCharacter = new CharactersAndCards();
        cardsGameObjectsPlayer = new List<GameObject>();

        activeEnemy = new EnemiesAndCards();
        cardsGameObjectsEnemy = new List<GameObject>();


        //LOOP PARA SELECCIONAR EL PERSONAJE QUE LANZA EL EVENTO O QUE ES EL OBJETIVO DE UN EVENTO ENEMIGO
        foreach (CharactersAndCards character in MainData.activeCharacters)
        {
            if (character.character.name == triggerPlayer) {
                //LECTURA DE LOS MAZOS DE CARTAS DEL JUGADOR: PREPARADAS Y ACTIVAS PARA EL ENCUENTRO, QUEMADAS Y EXHAUSTAS
                activeCharacter.character = character.character;
                activeCharacter.readyDeck = character.readyDeck;
                activeCharacter.burnedDeck = character.burnedDeck;
                activeCharacter.exhaustedDeck = character.exhaustedDeck;
            }
        }

        //LOOP PARA SELECCIONAR EL ENEMIGO QUE LANZA EL EVENTO O QUE ES EL OBJETIVO DE UN EVENTO DEL JUGADOR
        foreach (EnemiesAndCards enemy in MainData.activeEnemies)
        {
            if (enemy.enemy.ID == triggerEnemy)
            {
                //LECTURA DE LOS MAZOS DE CARTAS DEL ENEMIGO: PREPARADAS Y ACTIVAS PARA EL ENCUENTRO, QUEMADAS Y EXHAUSTAS
                activeEnemy.enemy = enemy.enemy;
                activeEnemy.readyDeck = enemy.readyDeck;
                activeEnemy.burnedDeck = enemy.burnedDeck;
                activeEnemy.exhaustedDeck = enemy.exhaustedDeck;
            }
        }

        #region TEST DE LECTURA DE DATOS
        //Debug.Log(activeCharacter.character.aimBasePercentage);
        //Debug.Log(activeCharacter.exhaustedDeck.cards.Count);
        //Debug.Log(activeEnemy.enemy.defenseBasePercentage);
        //Debug.Log(activeEnemy.readyDeck.cards.Count);
        //Debug.Log(activeCharacter.burnedDeck.cards[0].cardName);
        #endregion

        activeCharacter.readyDeck.cards.Suffle(50); //FUNCIÓN PARA BARAJAR EL MAZO
        activeEnemy.readyDeck.cards.Suffle(50);

        MakeDecks(activeCharacter.readyDeck, "player");
        MakeDecks(activeEnemy.readyDeck, "enemy");

        StartCoroutine(DealCards(cardsGameObjectsPlayer, activeCharacter.character.hand, "player"));
        StartCoroutine(DealCards(cardsGameObjectsEnemy, activeEnemy.enemy.hand, "enemy"));

        #endregion


    }

    public void MakeDecks(CardList cardList, string trigger) //FUNCIÓN PARA CREAR LOS MAZOS DEL JUGADOR O DEL ENEMIGO
    {
        for (int i = 0; i < cardList.cards.Count; i++)
        {
            GameObject cardPrefab = Resources.Load("Prefabs/Card", typeof(GameObject)) as GameObject;
            cardPrefab.transform.GetChild(0).GetComponent<Image>().sprite = ReturnArtworkCard(cardList.cards[i]);//CARGAR LA IMAGEN DE FONDO DE LA CARTA
            //GetChild(1) CORRESPONDE A LA CAPA BACKGROUND QUE NO VARIA
            cardPrefab.transform.GetChild(2).GetComponent<Image>().sprite = ReturnFrameworkType(cardList.cards[i]); //CARGAR EL TIPO DE MARCO DE CARTA
            cardPrefab.transform.GetChild(3).GetComponent<Image>().sprite = ReturnLevelMark(cardList.cards[i]); //CARGAR EL INDICADOR DEL NIVEL DE LA CARTA
            cardPrefab.transform.GetChild(4).GetComponent<Text>().text = cardList.cards[i].cardName != null ? cardList.cards[i].cardName : ""; // CARGAR EL NOMBRE DE LA CARTA
            cardPrefab.transform.GetChild(5).GetComponent<Text>().text = cardList.cards[i].cardDescription != null ? cardList.cards[i].cardDescription : ""; // CARGAR LA DESCRIPCION DE LA CARTA
            //SE CARGAN LOS DATOS GUARDADOS EN LA CLASE CARTA A CADA UNO DE LOS GAMEOBJECTS DE LA CARTA
            cardPrefab.transform.GetComponent<GameObjectCardData>().ID = cardList.cards[i].ID;
            cardPrefab.transform.GetComponent<GameObjectCardData>().cardName = cardList.cards[i].cardName;
            cardPrefab.transform.GetComponent<GameObjectCardData>().cardDescription = cardList.cards[i].cardDescription;
            cardPrefab.transform.GetComponent<GameObjectCardData>().type = cardList.cards[i].type;
            cardPrefab.transform.GetComponent<GameObjectCardData>().level = cardList.cards[i].level;
            cardPrefab.transform.GetComponent<GameObjectCardData>().levelMark = cardList.cards[i].levelMark;
            cardPrefab.transform.GetComponent<GameObjectCardData>().frameworkType = cardList.cards[i].frameworkType;
            cardPrefab.transform.GetComponent<GameObjectCardData>().imageArt = cardList.cards[i].imageArt;
            cardPrefab.transform.GetComponent<GameObjectCardData>().effects = cardList.cards[i].effects;

            if (cardList.cards[i].effects != null && cardList.cards[i].effects.Length > 0)
            {
                cardPrefab.transform.GetChild(6).gameObject.SetActive(true);
            }
            else
            {
                cardPrefab.transform.GetChild(6).gameObject.SetActive(false);
            }

            cardPrefab = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity);

            if (trigger == "player")
            {
                cardPrefab.transform.SetParent(deckReadyPlayerZone.transform, false);
                cardsGameObjectsPlayer.Add(cardPrefab);
            }
            else if (trigger == "enemy")
            {
                cardPrefab.transform.SetParent(deckReadyEnemyZone.transform, false);
                cardsGameObjectsEnemy.Add(cardPrefab);
            }

        }
    }

    public IEnumerator DealCards(List<GameObject> gameObjectsList, int numberOfCards, string trigger)
    {
        for (int i = 0; i < numberOfCards; i++)
        {
            //deck[i].transform.SetParent(playerZone.transform, true);
            yield return new WaitForSeconds(0.085f * (float)numberOfCards);
            //objectsReadyDeck.Remove(objectsReadyDeck[i]);
            StartCoroutine(FlipCard(gameObjectsList[i], trigger));
        }
    }

    public IEnumerator FlipCard(GameObject card, string trigger)
    {
        yield return new WaitForSeconds(0.08f);

        GameObject cardConvoy;

        if (trigger == "player")
        {
            cardConvoy = Instantiate(cardPlayerConvoyPrefab, deckReadyPlayerZone.GetComponent<Transform>().position, Quaternion.identity);
            cardConvoy.transform.SetParent(mainCanvas.transform, true);
            card.transform.SetParent(cardConvoy.transform, false);
        }
        else if (trigger == "enemy")
        {
            cardConvoy = Instantiate(cardEnemyConvoyPrefab, deckReadyEnemyZone.GetComponent<Transform>().position, Quaternion.identity);
            cardConvoy.transform.SetParent(mainCanvas.transform, false);
            card.transform.SetParent(cardConvoy.transform, false);
            cardConvoy.transform.GetComponent<Animator>().SetTrigger("InitialAnim");
        }

        
    }


    private Sprite ReturnArtworkCard(CardClass card) //FUNCION PARA RETORNAR LA IMAGEN DE LA CARTA DE ACUERDO A LOS DATOS DEL FICHERO JSON
    {
        Sprite art;


        if (card.imageArt != null && card.imageArt != "") //&& AssetDatabase.FindAssets(card.imageArt) != null)
        {
            //art = AssetDatabase.LoadAssetAtPath(relativePath + card.imageArt + extension, typeof(Sprite)) as Sprite;
            art = Resources.Load("Images/Cards/" + card.imageArt, typeof(Sprite)) as Sprite;
        }
        else
        {
            //art = AssetDatabase.LoadAssetAtPath(relativePath + "Default" + extension, typeof(Sprite)) as Sprite;
            art = Resources.Load("Images/Cards/" + "Default", typeof(Sprite)) as Sprite;
        }

        return art;
    }

    private Sprite ReturnFrameworkType(CardClass card) //FUNCION PARA RETORNAR LA IMAGEN DEL MARCO DE LA CARTA DE ACUERDO A LOS DATOS DEL FICHERO JSON
    {
        Sprite framework;

        if (card.frameworkType != null && card.frameworkType != "")// && AssetDatabase.FindAssets(card.frameworkType) != null)
        {
            //framework = AssetDatabase.LoadAssetAtPath(relativePath + card.frameworkType + extension, typeof(Sprite)) as Sprite;
            framework = Resources.Load("Images/Cards/" + card.frameworkType, typeof(Sprite)) as Sprite;
        }
        else
        {
            //framework = AssetDatabase.LoadAssetAtPath(relativePath + "DefaultFramework" + extension, typeof(Sprite)) as Sprite;
            framework = Resources.Load("Images/Cards/" + "DefaultFramework", typeof(Sprite)) as Sprite;
        }

        return framework;
    }

    private Sprite ReturnLevelMark(CardClass card) //FUNCION PARA RETORNAR LA IMAGEN QUE INDICA EL NIVEL DE LA CARTA DE ACUERDO A LOS DATOS DEL FICHERO JSON
    {
        Sprite levelMark;

        if (card.levelMark != null && card.levelMark != "")// && AssetDatabase.FindAssets(card.levelMark) != null)
        {
            //levelMark = AssetDatabase.LoadAssetAtPath(relativePath + card.levelMark + extension, typeof(Sprite)) as Sprite;
            levelMark = Resources.Load("Images/Cards/" + card.levelMark, typeof(Sprite)) as Sprite;
        }
        else
        {
            //levelMark = AssetDatabase.LoadAssetAtPath(relativePath + "Level1" + extension, typeof(Sprite)) as Sprite;
            levelMark = Resources.Load("Images/Cards/" + "Level1", typeof(Sprite)) as Sprite;
        }

        return levelMark;
    }
}


