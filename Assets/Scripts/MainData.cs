using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MainData : MonoBehaviour
{
    public string fileCharacterPath, fileEnemyPath, fileScenePath;
    string jsonCharacterString, jsonEnemyString, jsonSceneString;
    string fileReadyDeckPath, fileBurnedDeckPath, fileExhaustedDeckPath;
    string jsonDecksString;
    CardList tempReadyDeck, tempBurnedDeck, tempExhaustedDeck;
    static public List<CharactersAndCards> activeCharacters;
    static public List<EnemiesAndCards> activeEnemies;
    public Scene activeScene;

    public string directoryPath = System.IO.Directory.GetCurrentDirectory() + "/Assets/Resources/";
   

    private void Awake()
    {
        Application.targetFrameRate = 60; // LIMITAR FPS

        activeScene = new Scene();

        CharacterList characters = new CharacterList(); // LISTA QUE GUARDA LA LISTA DE PERSONAJES QUE SALEN EN UNA EXPEDICIÓN. POR EL MOMENTO SÓLO ESTÁ UNO IMPLEMENTADO
        activeCharacters = new List<CharactersAndCards>(); // LISTA QUE GUARDA OBJETOS DE LA CLASE QUE PERMITE ALMACENAR LOS DATOS DE CADA PERSONAJE Y SUS MAZOS

        EnemyList enemies = new EnemyList(); //LISTA QUE GUARDA LOS ENEMIGOS QUE APARECEN EN UN ESCENARIO
        activeEnemies = new List<EnemiesAndCards>(); // LISTA QUE GUARDA OBJETOS DE LA CLASE QUE PERMITE ALMACENAR LOS DATOS DE CADA ENEMIGO Y SUS MAZOS

        tempReadyDeck = new CardList(); // LISTA DE CARTAS TEMPORAL QUE A CADA ITERACIÓN RECOGE LAS CARTAS DE CADA PERSONAJE O ENEMIGO
        tempBurnedDeck = new CardList();
        tempExhaustedDeck = new CardList();

        #region LECTURA DE PARAMETROS DE ESCENARIO

        fileScenePath = directoryPath + "JSON/Scenes/SceneData.json";
        jsonSceneString = File.ReadAllText(fileScenePath);
        activeScene = JsonUtility.FromJson<Scene>(jsonSceneString);


        #endregion

        #region LECTURA DE PERSONAJES

        fileCharacterPath = directoryPath + "JSON/Characters/Characters.json"; // DIRECCION DEL ARCHIVO JSON QUE GUARDA LAS ESTADISTICAS DE LOS PERSONAJES
        jsonCharacterString = File.ReadAllText(fileCharacterPath); //LECTURA DEL FICHERO JSON DE PERSONAJES
        characters = JsonUtility.FromJson<CharacterList>(jsonCharacterString);

        


        foreach (Character character in characters.characters) //LOOP QUE RECORRE LA LISTA DE PERSONAJES PARA ALMACENAR LOS DATOS DE LOS PERSONAJES Y SUS MAZOS
        {
            CharactersAndCards tempCharacterAndCards = new CharactersAndCards();

            fileReadyDeckPath = ReturnFilePath(character, null, "ready");
            jsonDecksString = File.ReadAllText(fileReadyDeckPath);
            tempReadyDeck = JsonUtility.FromJson<CardList>(jsonDecksString);

            //LECTURA DEL MAZO DE CARTAS QUE SE ENCUENTRAN AGOTADAS PORQUE YA HAN SIDO USADAS EN OTRO ENCUENTRO Y QUE SE PUEDEN RECUPERAR DURANTE EL ENCUENTRO ACTUAL POR LA ACCIÓN DE OTRAS CARTAS
            fileBurnedDeckPath = ReturnFilePath(character, null, "burned");
            jsonDecksString = File.ReadAllText(fileBurnedDeckPath);
            tempBurnedDeck = JsonUtility.FromJson<CardList>(jsonDecksString);

            // LECTURA DEL MAZO DE CARTAS QUE SE ENCUENTRAN AGOTADAS Y QUE SOLO SE PUEDEN RECUPERAR ENTRE PANTALLAS DE BUSQUEDA
            fileExhaustedDeckPath = ReturnFilePath(character, null, "exhausted");
            jsonDecksString = File.ReadAllText(fileExhaustedDeckPath);
            tempExhaustedDeck = JsonUtility.FromJson<CardList>(jsonDecksString);

            tempCharacterAndCards.character = character;
            tempCharacterAndCards.readyDeck = tempReadyDeck;
            tempCharacterAndCards.burnedDeck = tempBurnedDeck;
            tempCharacterAndCards.exhaustedDeck = tempExhaustedDeck;

            activeCharacters.Add(tempCharacterAndCards);
        }

        #endregion

        #region LECTURA DE ENEMIGOS

        fileEnemyPath = directoryPath + "JSON/Enemies/Scene01.json"; // DIRECCION DEL ARCHIVO JSON QUE GUARDA LAS ESTADISTICAS DE LOS ENEMIGOS DE CADA NIVEL
        jsonEnemyString = File.ReadAllText(fileEnemyPath); //LECTURA DEL FICHERO JSON DE ENEMIGOS
        enemies = JsonUtility.FromJson<EnemyList>(jsonEnemyString);


        foreach (Enemy enemy in enemies.enemies) //LOOP QUE RECORRE LA LISTA DE PERSONAJES PARA ALMACENAR LOS DATOS DE LOS ENEMIGOS Y SUS MAZOS
        {
            EnemiesAndCards tempEnemiesAndCards = new EnemiesAndCards();

            fileReadyDeckPath = ReturnFilePath(null, enemy, "ready");
            jsonDecksString = File.ReadAllText(fileReadyDeckPath);
            tempReadyDeck = JsonUtility.FromJson<CardList>(jsonDecksString);

            //LECTURA DEL MAZO DE CARTAS QUE SE ENCUENTRAN AGOTADAS PORQUE YA HAN SIDO USADAS EN OTRO ENCUENTRO Y QUE SE PUEDEN RECUPERAR DURANTE EL ENCUENTRO ACTUAL POR LA ACCIÓN DE OTRAS CARTAS
            fileBurnedDeckPath = ReturnFilePath(null, enemy, "burned");
            jsonDecksString = File.ReadAllText(fileBurnedDeckPath);
            tempBurnedDeck = JsonUtility.FromJson<CardList>(jsonDecksString);

            // LECTURA DEL MAZO DE CARTAS QUE SE ENCUENTRAN AGOTADAS Y QUE SOLO SE PUEDEN RECUPERAR ENTRE PANTALLAS DE BUSQUEDA
            fileExhaustedDeckPath = ReturnFilePath(null, enemy, "exhausted");
            jsonDecksString = File.ReadAllText(fileExhaustedDeckPath);
            tempExhaustedDeck = JsonUtility.FromJson<CardList>(jsonDecksString);

            tempEnemiesAndCards.enemy = enemy;
            tempEnemiesAndCards.readyDeck = tempReadyDeck;
            tempEnemiesAndCards.burnedDeck = tempBurnedDeck;
            tempEnemiesAndCards.exhaustedDeck = tempExhaustedDeck;

            activeEnemies.Add(tempEnemiesAndCards);
        }

        #endregion

        #region TEST DE LECTURA DE DATOS
        //Debug.Log(activeCharacters[0].character.damageDices.Count);
        //Debug.Log(dataActiveCharacters[0].character.hand);
        //Debug.Log(activeCharacters[0].readyDeck.cards.Count);
        //Debug.Log(dataActivePlayers[0].burnedDeck.cards.Count);
        //Debug.Log(dataActivePlayers[0].exhaustedDeck.cards.Count);
        //Debug.Log(activeEnemies[0].enemy.name);
        //Debug.Log(activeEnemies[0].readyDeck.cards[0].cardName);
        #endregion
    }


    public string ReturnFilePath(Character character, Enemy enemy, string deckType)
    {
        string filePath;
        string stringPath = "";

        if (character != null && enemy == null)
        {
            stringPath = character.name;
        }
        else if (character == null & enemy != null)
        {
            stringPath = enemy.ID;
        }

        switch (deckType)
        {
            case "ready":
                filePath = directoryPath + "JSON/Decks/" + stringPath + "/ReadyDeck.json";
                break;

            case "burned":
                filePath = directoryPath + "JSON/Decks/" + stringPath+ "/BurnedDeck.json";
                break;

            case "exhausted":
                filePath = directoryPath + "JSON/Decks/" + stringPath + "/ExhaustedDeck.json";
                break;

            default:
                filePath = "";
                break;
        }

        return filePath;
    }
}

#region CLASES SERIALIZABLES DE JUGADORES, ENEMIGOS, CARTAS Y ESCENARIO

[System.Serializable]
public class Character
{
    public string name;
    public int level;
    public int energy;
    public int health;
    public int hand;
    public int defaultDices;
    public List<string> damageDices;
    public double aimBasePercentage;
    public int aimBaseSlots;
    public double defenseBasePercentage;
    public int defenseBaseSlots;
    public double harvestBasePercentage;
    public int harvestBaseSlots;
    public int supportingBaseSlots;
    public int consumptionFoodPerDay;
    public int consumptionWaterPerDay;
    public string specialHabilityName;
    public string specialHabilityDescription;
    public string characterDescription;
    public List<string> effects;
}

[System.Serializable]
public class Enemy
{
    public string ID;
    public string name;
    public int level;
    public int energy;
    public int health;
    public int hand;
    public List<string> damageDices;
    public double aimBasePercentage;
    public int aimBaseSlots;
    public double defenseBasePercentage;
    public int defenseBaseSlots;
    public string specialHabilityName;
    public string specialHabilityDescription;
    public string characterDescription;
    public List<string> effects;
}

[System.Serializable]
public class CharacterList
{
    public List<Character> characters;
}

[System.Serializable]

public class EnemyList
{
    public List<Enemy> enemies;
}


[System.Serializable]
public class CardClass
{
    public string ID;
    public string cardName;
    public string cardDescription;
    public string type;
    public int level;
    public string levelMark;
    public string frameworkType;
    public string imageArt;
    public string effects;
}

[System.Serializable]
public class DeckList
{
    public List<CardList> decks;
}

[System.Serializable]
public class CardList
{
    public List<CardClass> cards;
}

[System.Serializable]
public class CharactersAndCards
{
    public Character character;
    public CardList readyDeck;
    public CardList burnedDeck;
    public CardList exhaustedDeck;
}

[System.Serializable]
public class EnemiesAndCards
{
    public Enemy enemy;
    public CardList readyDeck;
    public CardList burnedDeck;
    public CardList exhaustedDeck;
}

[System.Serializable]

public class Scene
{
    public int noise;
}

#endregion